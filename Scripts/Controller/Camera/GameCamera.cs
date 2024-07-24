using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public delegate void OnCharacterStateChange();
    public event OnCharacterStateChange CharacterStateOn;
    public event OnCharacterStateChange CharacterStateOff;
    private const float OFFSETDISTANCE = 10f;
    private const float CHARACTEROFFSETDISTANCE = 5f;
    private bool inCharacterMode;
    private CharacerController myCharacter;
    private Vector3 startMousePos, currentMousePos, targetPosition, startPosition;
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float timeToNewPosition;
    [SerializeField] private float timeToNewCharacterPosition;
    [SerializeField] private float minPositionX, maxPositionX, minPositionZ, maxPositionZ;

    private void Awake()
    {
        startPosition = transform.position;
        targetPosition = transform.position;
    }

    void Update()
    {
        if(inCharacterMode)
        {
            CameraControlInCharacterMode();
        }
        else
        {
            CameraControlInDefaultMode();
        }
    }

    private void CameraControlInCharacterMode()
    {
        transform.position = new UnityEngine.Vector3(Mathf.Lerp(transform.position.x, myCharacter.transform.position.x, timeToNewCharacterPosition * Time.deltaTime), 
        transform.position.y, Mathf.Lerp(transform.position.z, myCharacter.transform.position.z - CHARACTEROFFSETDISTANCE, timeToNewCharacterPosition * Time.deltaTime));
    }

    private void CameraControlInDefaultMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentMousePos = Input.mousePosition;
            UnityEngine.Vector3 direction = currentMousePos - startMousePos;
            targetPosition = new UnityEngine.Vector3(gameObject.transform.position.x - direction.x / cameraMovementSpeed, gameObject.transform.position.y, gameObject.transform.position.z - direction.y / cameraMovementSpeed);
            direction *= -1;
        }
        transform.position = new UnityEngine.Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, timeToNewPosition * Time.deltaTime), 
        transform.position.y, Mathf.Lerp(transform.position.z, targetPosition.z, timeToNewPosition * Time.deltaTime));
        transform.position = new UnityEngine.Vector3(Mathf.Clamp(transform.position.x, minPositionX, maxPositionX), 
        transform.position.y, Mathf.Clamp(transform.position.z, minPositionZ - OFFSETDISTANCE, maxPositionZ));
    }

    public void GoToCharacterMode(CharacerController myChar)
    {
        CharacterStateOn.Invoke();
        myCharacter = myChar;
        inCharacterMode = true;
    }

    public void AwayFromCharacterMode()
    {
        CharacterStateOff.Invoke();
        targetPosition = startPosition;
        inCharacterMode = false;
        myCharacter = null; 
    }

    public void ChangeBorderCameraPosition(float[] newBorder)
    {
        minPositionX = newBorder[0];
        maxPositionX = newBorder[1];
        minPositionZ = newBorder[2];
        maxPositionZ = newBorder[3];
    }
}
