using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomCollection
{
    private const float MINDISBETWEENROOM = 5.5f;
    private List<RoomModel> roomList = new List<RoomModel>(100);
    private float smallestXDistance, biggestXDistance, smallestZDistance, biggestZDistance;
    private GameCamera gameCamera;

    public RoomCollection(GameCamera gCamera, RoomModel mainRoom)
    {
        gameCamera = gCamera;
        roomList.Add(mainRoom);
    }

    public void AddRoom(RoomModel room)
    {
        if(roomList.Count > 0 && roomList != null)
        {
            if(room.transform.position.x > biggestXDistance)
            {
                biggestXDistance = room.transform.position.x;
            }
            if(room.transform.position.x < smallestXDistance)
            {
                smallestXDistance = room.transform.position.x; 
            }
            if(room.transform.position.z > biggestZDistance)
            {
                biggestZDistance = room.transform.position.z;
            }
            if(room.transform.position.z < smallestZDistance)
            {
                smallestZDistance = room.transform.position.z; 
            }
            roomList.Add(room);
        }
        else
        {
            if(room.transform.position.x > 0)
            {
                biggestXDistance = room.transform.position.x;
            }
            if(room.transform.position.x < 0)
            {
                smallestXDistance = room.transform.position.x; 
            }
            if(room.transform.position.z > 0)
            {
                biggestZDistance = room.transform.position.z;
            }
            if(room.transform.position.z < 0)
            {
                smallestZDistance = room.transform.position.z; 
            }
            roomList.Add(room);
        }
        float[] newCords = {smallestXDistance, biggestXDistance, 
        smallestZDistance, biggestZDistance};
        gameCamera.ChangeBorderCameraPosition(newCords);
    }

    public void DeleteRoom(RoomModel room)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            if(room == roomList[i])
            {
                roomList.Remove(roomList[i]);
            }
        }
    }

    public bool CheckThisPosition(Vector3 position)
    {
        bool canSetRoom = false;
        for(int i = roomList.Count - 1; i >= 0; i--)
        {
            if(Vector3.Magnitude(roomList[i].transform.position - position) <= MINDISBETWEENROOM)
            {
                canSetRoom = true;
            }
        }
        return canSetRoom;
    }

    public List<RoomModel> NearbyRoom(RoomModel room)
    {
        List<RoomModel> newRoomList = new List<RoomModel>(4);
        int currentNearbyRoomCount = 0;
         for(int i = roomList.Count - 1; i >= 0; i--)
        {
            if(Vector3.Magnitude(roomList[i].transform.position - room.transform.position) <= MINDISBETWEENROOM)
            {
                newRoomList.Add(roomList[i]);
                currentNearbyRoomCount++;
                if(currentNearbyRoomCount >= 4)
                {
                    break;
                }
            }
        }
        return newRoomList;
    } 
}
