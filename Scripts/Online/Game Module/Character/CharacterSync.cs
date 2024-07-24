using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterSync : MonoBehaviour
{
    private PhotonView myPhotonView;
    private CharacerController myCharacerController;

    private void Awake()
    {
        try
        {
            myPhotonView = GetComponent<PhotonView>();
        }
        catch (MissingComponentException)
        {
            Debug.Log("Miss photon view in current character, fix it!");
        }
        try
        {
            myCharacerController = GetComponent<CharacerController>();
        }
        catch (MissingComponentException)
        {
            Debug.Log("Miss characer controller in current character, fix it!");
        }
    }

    public void SendTarget(Vector3 newTargetPosition)
    {
        myPhotonView.RPC("ReceiveTarget", RpcTarget.Others, newTargetPosition.x, newTargetPosition.y, newTargetPosition.z);
    }

    public void ReceiveTarget(float X, float Y, float Z)
    {
        myCharacerController.SetTarget(new Vector3(X, Y, Z));       
    }
}
