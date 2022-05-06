using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class JSONManager
{
    public static TextAsset jsonFiles = Resources.Load<TextAsset>("101");
    public static RoomItemList Rooms;


    public static void ReadFile()
    {
        Rooms = JsonUtility.FromJson<RoomItemList>(jsonFiles.text);
        Debug.Log("File Read");
    }

    public static string[] GetRoomNames()
    {
        if (Rooms.room.Length != 0)
        {
            string[] names = new string[Rooms.room.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = Rooms.room[i].roomName;
            }
            return names;
        }
        return null;
    }

    public static string[] GetRoomItems(int roomNum)
    {
        if (Rooms.room.Length != 0)
        {
            string[] items = new string[Rooms.room[roomNum].infoPoint.Length];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = Rooms.room[roomNum].infoPoint[i].itemName;
            }
            return items;
        }
        return null;
    }

    public static infoPoint GetInfoPoint(int roomNum, int itemNum)
    {
        return Rooms.room[roomNum].infoPoint[itemNum];
    }


}
