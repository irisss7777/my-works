using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Photon.Pun;   

public class RoomModel : MonoBehaviour
{

    [Range (0, 1)][SerializeField] private int[] openSides = new int[4];
    [Range (0, 1)][SerializeField] private int[] wallSides = new int[4];
    private bool isApplied;
    [SerializeField] private MonsterModel monsterInCurrentRoom;
    private RoomCollection roomCollection;
    [SerializeField] private GameObject changeCanvas;
    [SerializeField] private GameObject insideRoom;
    private RoomFactory roomFactory;
    private RoomModel[] nearbyRoom;
    private bool correctRotation;
    private PhotonView photonView;

    public void Init(RoomCollection rCollection, RoomFactory rFactory)
    {

        roomFactory = rFactory;
        roomCollection = rCollection;
        nearbyRoom = roomCollection.NearbyRoom(this).ToArray();  
        CheckCorrectRotation();
    }

    public void Awake()
    {
        try
        {
            photonView = GetComponent<PhotonView>();
        }
        catch (MissingComponentException)
        {
            Debug.Log("Miss photon view in current room, fix it!");
        }
        if (photonView != null)
        {
            if (!photonView.IsMine)
            {
                changeCanvas.SetActive(false);
            }
        }
    }

    public void RotateRoom(string rotateDirection)
    {
        if(!isApplied)
        {
        if(rotateDirection == "right" || rotateDirection == "left")
        {
            if(rotateDirection == "right")
            {
                openSides = openSides.Skip(openSides.Length - 1).
                Take(1).Concat(openSides.Take(openSides.Length - 1)).
                ToArray();

                wallSides = wallSides.Skip(wallSides.Length - 1).
                Take(1).Concat(wallSides.Take(wallSides.Length - 1)).
                ToArray();
                insideRoom.transform.Rotate(0, 90, 0);
            }
            if(rotateDirection == "left")
            {
                openSides = openSides.Skip(1).
                Take(openSides.Length - 1).Concat(openSides.Take(1)).
                ToArray();

                wallSides = wallSides.Skip(1).
                Take(wallSides.Length - 1).Concat(wallSides.Take(1)).
                ToArray();
                insideRoom.transform.Rotate(0, -90, 0);
            }
            CheckCorrectRotation();
        }
        else
        {
            Debug.Log("Not correct rotate direction");
        }
        }
    }

    private void CheckCorrectRotation()
    {
            correctRotation = true;
            for(int i = 0; i < nearbyRoom.Length; i++)
            {
                int[] openS;
                int[] wallS;
                nearbyRoom[i].GetOpenSides(out openS, out wallS);
                if(transform.position.z - nearbyRoom[i].transform.position.z > 0 && transform.position.x == nearbyRoom[i].transform.position.x)
                {
                    if(openSides[0] == 1)
                    {
                        if(openS[2] == 1)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                    else
                    {
                        if(openS[2] == 0)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                }
                if(transform.position.z - nearbyRoom[i].transform.position.z < 0 && transform.position.x == nearbyRoom[i].transform.position.x)
                {
                    if(openSides[2] == 1)
                    {
                        if(openS[0] == 1)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                    else
                    {
                        if(openS[0] == 0)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                }
                if(transform.position.x - nearbyRoom[i].transform.position.x > 0 && transform.position.z == nearbyRoom[i].transform.position.z)
                {
                    if(openSides[3] == 1)
                    {
                        if(openS[1] == 1)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                    else
                    {
                        if(openS[1] == 0)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                }
                if(transform.position.x - nearbyRoom[i].transform.position.x < 0 && transform.position.z == nearbyRoom[i].transform.position.z)
                {
                    if(openSides[1] == 1)
                    {
                        if(openS[3] == 1)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                    else
                    {
                        if(openS[3] == 0)
                        {

                        }
                        else
                        {  
                            correctRotation = false;
                            Debug.Log("Incorrect rotation 1");
                        }
                    }
                }
            }
    }

    public void ApplyRotateRoom()
    {
        if(correctRotation)
        {
            changeCanvas.SetActive(false);
            isApplied = true;
            roomCollection.AddRoom(this);
            roomFactory.EndBuildRoom();
        }
    }

    public void GetOpenSides(out int[] openS, out int[] wallS)
    {
        openS = openSides;
        wallS = wallSides;
    }

    public int[] GetRoomInfo()
    {
        int[] roomInfo = { 0, 0, 0};
        if(monsterInCurrentRoom != null)
        {
            roomInfo[0] = 1;
            Debug.Log("Room is not empty!");
        }
        else
        {
            Debug.Log("Room is empty!");
        }
        return roomInfo;
    }

    public IDamageble GetMonsterInRoom()
    {
        return monsterInCurrentRoom as IDamageble;
    }

    public void DestroyRoomInEditMode()
    {
        if(!isApplied)
        {
            Destroy(this.gameObject);
        }
    }

    public void DestroyRoomInPlayMode()
    {
        roomCollection.DeleteRoom(this);
    }
}
