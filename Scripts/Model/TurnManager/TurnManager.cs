using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private RoomFactory roomFactory;
    [SerializeField] private CharacterFactory characterFactory;
    private int currentCharacterType;
    private bool canExit;

    private void EndTurn()
    {
        if(canExit)
        {
            characterFactory.InstantiateCharacter(currentCharacterType);
            canExit = false;
        }
        else
        {
            roomFactory.LockFactory();
        }
        UnSubscribe();
    }

    public void StartTurn()
    {
        roomFactory.UnlockFactory();
        Subscribe();
    }

    public void IWannaExit()
    {
        canExit = true;
    }

    private void Subscribe()
    {
        roomFactory.RoomInstantiated += EndTurn;
    }

    private void UnSubscribe()
    {
        roomFactory.RoomInstantiated -= EndTurn;
    }
}
