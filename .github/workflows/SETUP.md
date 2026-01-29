# Unity Build GitHub Actions Setup

This document explains how to set up the GitHub Actions workflow for building your Unity project with a free license.

## Prerequisites

- Unity 2021.3.6f1
- A Unity account (free tier is sufficient)
- GitHub repository with this project

## Setup Instructions

### Step 1: Get Unity License File

Since you're using a free Unity license, you need to generate a license file:

#### Method 1: Manual Activation (Recommended for Free License)

1. **First Run**: Push your code to GitHub. The workflow will run and fail, but it will generate an activation file.

2. **Download Activation File**: 
   - Go to your GitHub repository's Actions tab
   - Click on the failed workflow run
   - Download the `.alf` file from the artifacts

3. **Request License**:
   - Go to https://license.unity3d.com/manual
   - Upload the `.alf` file
   - Download the Unity license file (`.ulf`)

4. **Convert License to Base64**:
   
   **On Windows (PowerShell):**
   ```powershell
   [Convert]::ToBase64String([System.IO.File]::ReadAllBytes("Unity_v2021.x.ulf"))
   ```
   
   **On macOS/Linux:**
   ```bash
   base64 Unity_v2021.x.ulf
   ```

#### Method 2: Local Activation

If you have Unity installed locally:

1. Find your Unity license file:
   - **Windows**: `C:\ProgramData\Unity\Unity_lic.ulf`
   - **macOS**: `/Library/Application Support/Unity/Unity_lic.ulf`
   - **Linux**: `~/.local/share/unity3d/Unity/Unity_lic.ulf`

2. Convert to Base64 (see commands above)

### Step 2: Add GitHub Secrets

Add the following secrets to your GitHub repository:

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret** and add:

   - **UNITY_LICENSE**: The base64-encoded content of your `.ulf` file
   - **UNITY_EMAIL**: Your Unity account email (optional, but recommended)
   - **UNITY_PASSWORD**: Your Unity account password (optional, but recommended)

### Step 3: Trigger the Build

Once secrets are configured:

1. Push a commit to the `main` branch, or
2. Create a pull request, or
3. Manually trigger the workflow from the Actions tab

## Build Outputs

The workflow builds for three platforms:
- **Windows 64-bit** (StandaloneWindows64)
- **Linux 64-bit** (StandaloneLinux64)
- **WebGL**

Build artifacts will be available in the Actions tab after each successful run.

## Customization

### Change Target Platforms

Edit [.github/workflows/build.yml](.github/workflows/build.yml) and modify the `matrix.targetPlatform` section:

```yaml
matrix:
  targetPlatform:
    - StandaloneWindows64    # Windows
    - StandaloneLinux64       # Linux
    - StandaloneOSX           # macOS
    - WebGL                   # WebGL
    - iOS                     # iOS (requires macOS runner)
    - Android                 # Android
```

### Change Unity Version

Update the `unityVersion` in all steps from `2021.3.6f1` to your desired version.

### Build Only on Tags

To build only when creating a release tag, change the workflow trigger:

```yaml
on:
  push:
    tags:
      - 'v*'
```

## Troubleshooting

### License Activation Fails

- Ensure `UNITY_LICENSE` secret contains the base64-encoded `.ulf` file content
- Verify the license is for Unity Personal/Plus/Pro (not expired)
- Check that the Unity version matches (2021.3.6f1)

### Build Fails

- Check the Unity version in ProjectSettings/ProjectVersion.txt matches the workflow
- Ensure all required packages are in the repository
- Check build logs for specific compilation errors

### Out of GitHub Actions Minutes

- Free GitHub accounts have limited Actions minutes
- Consider building only on specific branches or tags
- Use caching (already configured) to speed up builds

## Additional Resources

- [GameCI Documentation](https://game.ci/docs)
- [Unity-Builder Action](https://github.com/game-ci/unity-builder)
- [Unity License Manual Activation](https://game.ci/docs/github/activation)
