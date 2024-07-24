using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using ExitGames.Client.Photon;

public class RoomFactory : MonoBehaviour
{
    private RoomCollection roomCollection;
    [SerializeField] private RoomModel mainRoom;
    [SerializeField] private GameObject[] roomsPrefabs;
    [SerializeField] private GameCamera gameCamera;
    private bool canInstantiateRoom;
    private int selectedRoomType;
    public delegate void OnRoomInstantiate();
    public event OnRoomInstantiate RoomInstantiated;
    private PhotonView photonView;
    [SerializeField] private LayerMask roomLayer;

    void Awake()
    {
        if(gameCamera != null)
        {
            roomCollection = new RoomCollection(gameCamera, mainRoom);
            gameCamera.CharacterStateOn += LockFactory;
            gameCamera.CharacterStateOff += UnlockFactory;
        }
        else
        {
            throw new System.Exception("GameCamera not found in RoomFactory");
        }

        try
        {
            photonView = GetComponent<PhotonView>();
        }
        catch (MissingComponentException)
        {
            Debug.Log("Miss photon view in current room, fix it!");
        }
    }

    public void InstantiateRoom(Vector3 position)
    {
        if(canInstantiateRoom)
        {
            if(roomCollection.CheckThisPosition(position))
            {
                GameObject newRoom = PhotonNetwork.Instantiate(roomsPrefabs[selectedRoomType].name, position, Quaternion.identity);
                newRoom.GetComponent<RoomModel>().Init(roomCollection, this);
                photonView.RPC("SyncInstantiatedRoom", RpcTarget.Others, position.x, position.y, position.z);
            }
        }
    }

    public void SyncInstantiatedRoom(float posX, float posY, float posZ)
    {
        //Vector3 roomPos = new Vector3(posX, posY, posZ);
        //Collider[] hitObjects = Physics.OverlapSphere(roomPos, 0.1f, roomLayer);
        //foreach(Collider someCollider in hitObjects)
        //{
        //    if(someCollider.gameObject.transform.position == roomPos)
        //    {
        //        try
        //        {
        //            someCollider.gameObject.GetComponent<RoomModel>().Init(roomCollection, this);
        //        }
        //        catch(MissingComponentException)
        //        {
        //            Debug.Log("Component RoomMode() not found");
        //        }
        //        break;
        //    }
        //}
        Debug.Log("AAAA");
    }

    public void EndBuildRoom()
    {
        RoomInstantiated.Invoke();
    }

    public void LockFactory()
    {
        canInstantiateRoom = false;
    }

    public void UnlockFactory()
    {
        canInstantiateRoom = true;
    }
}
