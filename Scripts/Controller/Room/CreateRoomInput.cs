using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomInput : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private const float MINCELLDISTANCE = 5f;
    [SerializeField] private RoomFactory roomFactory;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ClickToSpawnRoom();
        }
    }

    public void ClickToSpawnRoom()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.collider.gameObject.tag == "mainplane")
            {
                float xPosition = hitInfo.point.x;
                float zPosition = hitInfo.point.z;
                float correctPositionX = ((float)Math.Round(xPosition / MINCELLDISTANCE)) * 5;
                float correctPositionZ = ((float)Math.Round(zPosition / MINCELLDISTANCE)) * 5;

                Vector3 roomPosition = new Vector3(correctPositionX, 0, correctPositionZ);
                roomFactory.InstantiateRoom(roomPosition);
            }
        }
    }
}
