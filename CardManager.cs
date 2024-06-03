using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class CardManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private CardView cardView;
    public int number;
    public int cost;
    private PhotonView view;
    private bool canClick;

    public void OnPointerEnter()
    {
        if (view.IsMine)
        {
            cardView.PointerEnter();
        }
    }

    public void OnPointerExit()
    {
        if (view.IsMine)
        {
            cardView.PointerExit();
        }
    }

    public void OnClick()
    {
        if (canClick == false && view.IsMine)
        {
            canClick = true;
            cardView.Click(Input.touchCount);
        }
    }

    public void StopChoose()
    {
        canClick = false;
    }
}
