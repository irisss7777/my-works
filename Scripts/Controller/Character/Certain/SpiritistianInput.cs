using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritistianInput : CharacterInput
{
    protected override void SetTarget(RoomModel room)
    {
        Debug.Log("Clicked on room");
        int[] openSides;
        int[] wallSides;
        room.GetOpenSides( out openSides, out wallSides);
        float distance = Vector3.Magnitude(transform.position - room.transform.position);
        bool canSetTarget = false;
        if(distance <= MINDISTOROOM)
        {
            if(transform.position.z - room.transform.position.z > 0 && openSides[0] == 1 && currentRoomOpenSides[2] == 1)
            {
                canSetTarget = true;
            }
            if(transform.position.z - room.transform.position.z < 0 && openSides[2] == 1 && currentRoomOpenSides[0] == 1)
            {
                canSetTarget = true;  
            }
            if(transform.position.x - room.transform.position.x > 0 && openSides[3] == 1 && currentRoomOpenSides[1] == 1)
            {
                canSetTarget = true;
            }
            if(transform.position.x - room.transform.position.x < 0 && openSides[1] == 1 && currentRoomOpenSides[3] == 1)
            {
                canSetTarget = true;
            }
        }
        if(canSetTarget)
        {
            Debug.Log("Can set target!");
            bool canMove = bonusCollector.MoveToTarget();
            bool newRoom = true;
            for(int i = 0; i < roomCollection.Count; i++)
            {
                if(roomCollection[i] == room)
                {
                    newRoom = false;
                    break;
                }
            }
            if(canMove && newRoom)
            {
                currentRoomOpenSides = openSides;
                roomCollection.Add(room);  
                GetRoomInfo(room);
                myMoveController.SetTarget(room.transform.position);
            }
            else
            {

            }
        }
    }
}
