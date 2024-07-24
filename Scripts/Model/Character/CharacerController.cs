using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Photon.Pun;

public class CharacerController : MonoBehaviourPun
{
    private Vector3 targetPosition;
    private bool canMove = false;
    private float moveSpeed = 0.05f;
    private float minDistanceToPoint = 0.1f;

    [Range (0, 3)][SerializeField] private int meleeDamage;
    [Range (0, 3)][SerializeField] private int distanceDamage;
    [Range (0, 3)][SerializeField] private int magicDamage;

    private CharactersBaseModel charactersBaseModel;
    private CharacterInput characterInput;
    private CharacterView characterView;
    private GameCamera myCamera;
    public delegate void AttackDelegate();
    public event AttackDelegate OnEndMove;
    public CharacterSync characterSync;

    public void Init(int maxHp, int characterType, Camera mainCamera, BonusCollector bCollector)
    {
        try
        {
            characterSync = GetComponent<CharacterSync>();
        }
        catch (MissingComponentException)
        {
            Debug.Log("Miss photon view in current character, fix it!");
        }
        myCamera = mainCamera.gameObject.GetComponent<GameCamera>();
        characterView = this.gameObject.GetComponent<CharacterView>();
        int[] attackDamage = { meleeDamage, distanceDamage, magicDamage, 0 };
        charactersBaseModel = new CharactersBaseModel(maxHp, attackDamage, characterView, bCollector, this);
        switch(characterType)
        {
            case 0:
            characterInput = gameObject.GetComponent<SpiritistianInput>();
            break;
        }
        if(characterInput != null)
        {
            characterInput.Init(this, charactersBaseModel, mainCamera, bCollector);
        }
        myCamera.GoToCharacterMode(this);
    }

    private void Update()
    {
        if(canMove)
        {
            MoveToTarget();
        }
    }

    public void SetTarget(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition.ConvetToTarget();
        characterSync.SendTarget(targetPosition);
        characterView.PlayMoveAnimation();
        canMove = true;
    }

    public void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
        if(Vector3.Magnitude(transform.position - targetPosition) <= minDistanceToPoint)
        {
            transform.position = targetPosition;
            canMove = false;
            characterView.StopMoveAnimation();
            Debug.Log("Character reached the point");
            OnEndMove.Invoke();
        }
    }

    private void OnDestroy()
    {
        myCamera.AwayFromCharacterMode();
        charactersBaseModel.UnSubscribe();
    }
}
