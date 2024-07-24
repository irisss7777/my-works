using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefab;
    private int[] charactersHealth = { 3, 3, 3, 3, 3, 5};
    [SerializeField] private Camera mainCamera;
    private BonusCollector bonusCollector;

    private void Start()
    {
        bonusCollector = new BonusCollector();
    }

    public void InstantiateCharacter(int characterType)
    {
        if(characterType >= 0 && characterType < characterPrefab.Length)
        {
            if(mainCamera != null && bonusCollector != null)    
            {
                PhotonNetwork.Instantiate(characterPrefab[characterType].name, new Vector3(0, 0, 0), Quaternion.identity).
                GetComponent<CharacerController>().Init(charactersHealth[characterType], characterType, mainCamera, bonusCollector);
            }
            else
            {
                Debug.Log("Dependency error on InstantiateCharacter()");
            }
        }
        else
        {
            Debug.Log("Away from array on InstantiateCharacter()");
        }
    }
}
