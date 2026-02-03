using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[System.Serializable]
public class SceneBundleManifest
{
    public List<string> scenes = new List<string>();
}

public class BundleBuilder
{
    [MenuItem("Tools/Build Scene Bundles")]
    public static void BuildSceneBundles()
    {
        // Get the currently loaded scene
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.IsValid())
        {
            Debug.LogError("No valid scene is currently loaded.");
            return;
        }

        string scenePath = currentScene.path;
        string sceneName = currentScene.name;

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("Current scene has not been saved. Please save the scene first.");
            return;
        }

        Debug.Log($"Building bundles for scene: {sceneName}");

        // Get all dependencies for the scene
        string[] dependencies = AssetDatabase.GetDependencies(scenePath, true);

        // Filter out dependencies from the Boilerplate folder and the scene itself
        List<string> assetDependencies = new List<string>();
        
        foreach (string dependency in dependencies)
        {
            // Skip if it's the scene itself
            if (dependency == scenePath)
                continue;
                
            // Skip if it's in the Boilerplate folder
            if (dependency.Contains("Assets/Boilerplate/"))
                continue;

            // Skip if its a package dependency - this will be in the base build
            if (dependency.StartsWith("Packages/"))
                continue;

            // Don't include scripts, they can't be in asset bundles
            if (dependency.EndsWith(".cs") || dependency.EndsWith(".js"))
                continue;
                
            assetDependencies.Add(dependency);
        }

        Debug.Log($"Found {assetDependencies.Count} dependencies (excluding Boilerplate)");
        // List out all dependencies found in the console
        foreach (string asset in assetDependencies)
        {
            Debug.Log($"- {asset}");
        }

        // Create asset bundle builds
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        // Build for assets
        if (assetDependencies.Count > 0)
        {
            AssetBundleBuild assetsBuild = new AssetBundleBuild();
            assetsBuild.assetBundleName = $"{sceneName}-assets";
            assetsBuild.assetNames = assetDependencies.ToArray();
            builds.Add(assetsBuild);
            
            Debug.Log($"Assets bundle will include: {string.Join(", ", assetDependencies.Take(10))}...");
        }
        else
        {
            Debug.LogWarning("No asset dependencies found outside of Boilerplate folder.");
        }

        // Build for the scene
        AssetBundleBuild sceneBuild = new AssetBundleBuild();
        sceneBuild.assetBundleName = $"{sceneName}-scene";
        sceneBuild.assetNames = new string[] { scenePath };
        builds.Add(sceneBuild);

        // Create output directory
        string outputPath = "AssetBundles";
        if (!System.IO.Directory.Exists(outputPath))
        {
            System.IO.Directory.CreateDirectory(outputPath);
        }

        // Build the bundles
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
            outputPath,
            builds.ToArray(),
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );



        if (manifest != null)
        {
            Debug.Log($"<color=green>Asset bundles created successfully in {outputPath}/</color>");
            Debug.Log($"- {sceneName}-assets (contains {assetDependencies.Count} assets)");
            Debug.Log($"- {sceneName}-main (contains the scene)");
        }
        else
        {
            Debug.LogError("Failed to build asset bundles.");
        }
    }

    [MenuItem("Tools/Build All Scene Bundles from Manifest")]
    public static void BuildAllSceneBundles()
    {
        string manifestPath = "Assets/SceneBundleManifest.json";
        
        if (!File.Exists(manifestPath))
        {
            Debug.LogError($"Scene bundle manifest not found at {manifestPath}");
            return;
        }

        string json = File.ReadAllText(manifestPath);
        SceneBundleManifest manifest = JsonUtility.FromJson<SceneBundleManifest>(json);

        if (manifest == null || manifest.scenes == null || manifest.scenes.Count == 0)
        {
            Debug.LogError("No scenes found in manifest.");
            return;
        }

        Debug.Log($"<color=cyan>Building bundles for {manifest.scenes.Count} scenes from manifest...</color>");

        // Store the currently active scene to restore later
        Scene originalScene = SceneManager.GetActiveScene();
        string originalScenePath = originalScene.path;

        int successCount = 0;
        int failCount = 0;

        foreach (string scenePath in manifest.scenes)
        {
            if (!File.Exists(scenePath))
            {
                Debug.LogWarning($"Scene not found: {scenePath}");
                failCount++;
                continue;
            }

            Debug.Log($"\n<color=yellow>=== Processing scene: {scenePath} ===</color>");

            // Open the scene
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            if (!scene.IsValid())
            {
                Debug.LogError($"Failed to load scene: {scenePath}");
                failCount++;
                continue;
            }

            // Build bundles for this scene
            bool success = BuildBundlesForScene(scene);
            
            if (success)
            {
                successCount++;
            }
            else
            {
                failCount++;
            }
        }

        // Restore original scene if it was valid
        if (!string.IsNullOrEmpty(originalScenePath) && File.Exists(originalScenePath))
        {
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        }

        Debug.Log($"\n<color=green>===== Bundle Build Complete =====</color>");
        Debug.Log($"<color=green>Success: {successCount}</color>");
        if (failCount > 0)
        {
            Debug.Log($"<color=red>Failed: {failCount}</color>");
        }
    }

    public static void BuildAllSceneBundlesCommandLine()
    {
        Debug.Log("Starting command-line bundle build...");
        BuildAllSceneBundles();
        
        // Check if there were any errors and exit with appropriate code
        // Note: Unity will exit with code 0 if no exceptions were thrown
    }

    private static bool BuildBundlesForScene(Scene scene)
    {
        string scenePath = scene.path;
        string sceneName = scene.name;

        Debug.Log($"Building bundles for scene: {sceneName}");

        // Get all dependencies for the scene
        string[] dependencies = AssetDatabase.GetDependencies(scenePath, true);

        // Filter out dependencies from the Boilerplate folder and the scene itself
        List<string> assetDependencies = new List<string>();
        
        foreach (string dependency in dependencies)
        {
            // Skip if it's the scene itself
            if (dependency == scenePath)
                continue;
                
            // Skip if it's in the Boilerplate folder
            if (dependency.Contains("Assets/Boilerplate/"))
                continue;

            // Skip if its a package dependency - this will be in the base build
            if (dependency.StartsWith("Packages/"))
                continue;

            // Don't include scripts, they can't be in asset bundles
            if (dependency.EndsWith(".cs") || dependency.EndsWith(".js"))
                continue;
                
            assetDependencies.Add(dependency);
        }

        Debug.Log($"Found {assetDependencies.Count} dependencies (excluding Boilerplate)");

        // Create asset bundle builds
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        // Build for assets
        if (assetDependencies.Count > 0)
        {
            AssetBundleBuild assetsBuild = new AssetBundleBuild();
            assetsBuild.assetBundleName = $"{sceneName}-assets";
            assetsBuild.assetNames = assetDependencies.ToArray();
            builds.Add(assetsBuild);
            
            Debug.Log($"Assets bundle will include {assetDependencies.Count} assets");
        }
        else
        {
            Debug.LogWarning("No asset dependencies found outside of Boilerplate folder.");
        }

        // Build for the scene
        AssetBundleBuild sceneBuild = new AssetBundleBuild();
        sceneBuild.assetBundleName = $"{sceneName}-scene";
        sceneBuild.assetNames = new string[] { scenePath };
        builds.Add(sceneBuild);

        // Create output directory
        string outputPath = "AssetBundles";
        if (!System.IO.Directory.Exists(outputPath))
        {
            System.IO.Directory.CreateDirectory(outputPath);
        }

        // Build the bundles
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
            outputPath,
            builds.ToArray(),
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );

        if (manifest != null)
        {
            Debug.Log($"<color=green>Asset bundles created successfully for {sceneName}</color>");
            Debug.Log($"- {sceneName}-assets (contains {assetDependencies.Count} assets)");
            Debug.Log($"- {sceneName}-scene (contains the scene)");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to build asset bundles for {sceneName}.");
            return false;
        }
    }
}
#endif
