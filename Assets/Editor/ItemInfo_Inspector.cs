using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item_Info))]
[CanEditMultipleObjects]
public class ItemInfo_Inspector : Editor
{

    int selectedRoom;
    int selectedItem;

    public override void OnInspectorGUI()
    {

        if (JSONManager.Rooms == null)
        {
            JSONManager.ReadFile();
        }


        Item_Info popUpItem = (Item_Info)target;

        DrawDefaultInspector();

        EditorGUILayout.PrefixLabel("Rooms");
        //EditorGUILayout.DropdownButton
        if (JSONManager.Rooms != null)
        {
            selectedRoom = EditorGUILayout.Popup(selectedRoom, JSONManager.GetRoomNames());
            selectedItem = EditorGUILayout.Popup(selectedItem, JSONManager.GetRoomItems(selectedRoom));
            //Debug.Log(JSONManager.GetRoomNames()[1]);
        }

        if (GUILayout.Button("Reload JSON"))
        {
            RefreshJson();
        }
        if (GUILayout.Button("Set Data"))
        {
            popUpItem.SetInfoPoint(JSONManager.GetInfoPoint(selectedRoom, selectedItem));
            PrefabUtility.RecordPrefabInstancePropertyModifications(popUpItem);
            serializedObject.ApplyModifiedProperties();
            
        }
        
        

        //popUpItem.itemInfo = JSONManager.Instance.Rooms[selectedIndex].infoPoints;
    }

    public void RefreshJson()
    {
        JSONManager.ReadFile();
    }
}
