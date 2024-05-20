using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerMovement : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public float speed;
    private FloatingJoystick variableJoystick;
    private FloatingJoystick rotateJoystick;
    [SerializeField] private float rotateSpeed;
    public GameObject cameraPoint;
    private Rigidbody rb;
    private Animator anim;
    private PhotonView view;
    [SerializeField] private float deathZone;
    private Vector3 normal;
    private Vector3 directionPlayer;
    [SerializeField] private float maxDisToGround;
    private bool isGrounded;
    [SerializeField] private GameObject groundChecker;
    [SerializeField] private float groundChekerRange;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject cum = GameObject.FindGameObjectWithTag("MainCamera");
        if (view.IsMine)
        {
            variableJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystick>();
            rotateJoystick = GameObject.FindGameObjectWithTag("Joystick1").GetComponent<FloatingJoystick>();
        }  
        canvas.SetActive(view.IsMine);
    }

    public void FixedUpdate()
    {
        if (view.IsMine)
        {
            //Default
            isGrounded = false;
            Collider[] grounds = Physics.OverlapSphere(groundChecker.transform.position, groundChekerRange, groundLayer);
            foreach (Collider ground in grounds)
            {
                isGrounded = true;
            }
            if (isGrounded)
            {
                rb.useGravity = false;
                anim.SetBool("Fall", false);
            }
            else
            {
                rb.useGravity = true;
                anim.SetBool("Fall", true);
            }
            if (isGrounded)
            {
                Movement();
                Rotate();
            }
        }
    }

    public void Movement()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit ground, 10f))
        {
            float disToGround = Vector3.Distance(transform.position, ground.point);
            float angle = Vector3.Angle(ground.normal, Vector3.up);
            if (angle <= 50 && stunned == false)
            {
                //Movement
                normal = ground.normal;
                Vector3 surfaceCoardinate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles - Vector3.forward;
                float sin = (float)Math.Sin(surfaceCoardinate.y / 57.295736719f) * -1;
                float cos = (float)Math.Cos(surfaceCoardinate.y / 57.295736719f);
                Vector3 direction = new Vector3(sin, 0, -cos) * -variableJoystick.Vertical + new Vector3(cos, 0, sin) * variableJoystick.Horizontal;
                Vector3 directionSurface = direction.normalized - Vector3.Dot(direction.normalized, normal) * normal;

                Vector3 directionSurface1 = directionSurface;
                directionPlayer = directionSurface;
                if (direction.x > deathZone || direction.x < -deathZone || direction.z > deathZone || direction.z < -deathZone && shot == false)
                {
                    float endSpeed = 0;
                    float sum = Math.Abs(direction.x) + Math.Abs(direction.z);
                    if (sum > deathZone)
                    {
                        if (inInvisible == false)
                        {
                            endSpeed = speed * 1.6f;
                        }
                        else
                        {
                            endSpeed = speed;
                        }
                    }
                    else
                    {
                        endSpeed = speed;
                    }
                    rb.velocity = new Vector3(directionSurface1.x * endSpeed, (directionSurface1.y * endSpeed) - 0.01f, directionSurface1.z * endSpeed);
                }
                else
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
                //Animation
                if (direction.x > deathZone || direction.x < -deathZone || direction.z > deathZone || direction.z < -deathZone && shot == false)
                {
                    int info3 = number;
                    RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    SendOptions sOption = new SendOptions { Reliability = true };
                    float sum = Math.Clamp(Math.Abs(direction.x) + Math.Abs(direction.z), 0, 1);
                    if (sum > 0.9f)
                    {
                        if (inInvisible == false)
                        {
                            anim.SetFloat("Speed", 2.1f);
                            PhotonNetwork.RaiseEvent(76, info3, option, sOption);
                        }
                        else
                        {
                            anim.SetFloat("Speed", 1.1f);
                            PhotonNetwork.RaiseEvent(77, info3, option, sOption);
                        }
                    }
                    else
                    {
                        anim.SetFloat("Speed", 1.1f);
                        PhotonNetwork.RaiseEvent(77, info3, option, sOption);
                    }
                }
                else
                {
                    int info3 = number;
                    RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    SendOptions sOption = new SendOptions { Reliability = true };
                    anim.SetFloat("Speed", 0f);
                    PhotonNetwork.RaiseEvent(78, info3, option, sOption);
                }
                //Rotate
                float check = direction.x + direction.z;
                if (check != 0 && spray == false)
                {
                    if (direction.x > deathZone || direction.x < -deathZone || direction.z > deathZone || direction.z < -deathZone)
                    {
                        float rot = 0;
                        if (direction.z > 0)
                        {
                            float directionX = Math.Clamp(direction.x, -1, 1);
                            rot = directionX * 90;
                        }
                        else
                        {
                            float directionX = Math.Clamp(direction.x, -1, 1);
                            float x = directionX + 2;
                            rot = x * -90;
                        }
                        Quaternion rotation = Quaternion.Euler(0, rot, 0);
                        transform.rotation = rotation;
                    }
                }
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
    }

    public void Rotate()
    {
        GameObject.FindGameObjectWithTag("MainCamera").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0),rotateJoystick.Horizontal * rotateSpeed * Time.deltaTime);
    }

   
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundChecker.transform.position, groundChekerRange);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 76:
                int info5 = (int)photonEvent.CustomData;
                if (info5 == number)
                {
                    anim.SetFloat("Speed", 2.1f);
                }
                break;
            case 77:
                int info6 = (int)photonEvent.CustomData;
                if (info6 == number)
                {
                    anim.SetFloat("Speed", 1.1f);
                }
                break;
            case 78:
                int info7 = (int)photonEvent.CustomData;
                if (info7 == number)
                {
                    anim.SetFloat("Speed", 0f);
                }
                break;         
        }
    }
}

