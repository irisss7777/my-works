using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CharacterInput : MonoBehaviour
{
    protected const float MINDISTOROOM= 5.5f;
    protected int[] currentRoomOpenSides = { 1, 1, 1, 1};
    protected Camera mainCamera;
    protected CharacerController myMoveController;
    protected CharactersBaseModel myBaseModel;
    protected BonusCollector bonusCollector;
    protected List<RoomModel> roomCollection = new List<RoomModel>(100);
    protected bool isInit;
    private RoomModel lastRoom;
    private RoomModel currentRoom;

    public virtual void Init(CharacerController moveController, CharactersBaseModel baseModel, Camera mainCam, BonusCollector bCollector)
    {
        bonusCollector = bCollector;
        myMoveController = moveController;
        myBaseModel = baseModel;
        mainCamera = mainCam;
        isInit = true;
    }
    
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && isInit)
        {
            RoomClickCheck();
        }
    }

    protected void RoomClickCheck()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.collider.gameObject.tag == "room")
            {
                SetTarget(hitInfo.collider.gameObject.GetComponent<RoomModel>());
            }
        }
    }

    protected abstract void SetTarget(RoomModel room);

    protected virtual void GetRoomInfo(RoomModel room)
    {
        int[] roomInfo = room.GetRoomInfo();
        if(roomInfo[0] == 1)
        {
            IDamageble monster = room.GetMonsterInRoom();
            myBaseModel.SetTarget(monster);
            myMoveController.OnEndMove += DestroyLastRoom;
            if(currentRoom == null)
            {
                currentRoom = room;
            }
            else
            {
                lastRoom = currentRoom;
                currentRoom = room;
            }
        }
    }

    protected virtual void DestroyLastRoom()
    {
        if(lastRoom != null)
        {
            Destroy(lastRoom.gameObject);
        }
        myMoveController.OnEndMove -= DestroyLastRoom;
    }
}
