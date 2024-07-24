using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToRoom : MonoBehaviourPunCallbacks
{
    private int maxPlayers = 6;

    public void OnClick()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else
        {
            Debug.Log("Lost connection");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }
}
