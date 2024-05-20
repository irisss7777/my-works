using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class AvatarContoller : MonoBehaviourPunCallbacks
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Transform movePosition;
    private PhotonView view;
    private RaycastHit hitInfo;
    public bool canMove;
    private Rigidbody rb;
    private Animator anim;
    public bool inUI;
    private StartGameCardGame startGame;
    [SerializeField] private GameObject clickPrefab;
    public bool dragCard;
    private CameraCardGame cum;


    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startGame = GameObject.FindGameObjectWithTag("CardManager").GetComponent<StartGameCardGame>();
        cum = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraCardGame>();
    }

    void Update()
    {
        if(view.IsMine)
        {
            if (Input.GetMouseButtonUp(0) && dragCard == false)
            {
                if (GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardGameManager>().inAttack == false)
                {
                    if (canMove == false)
                    {
                        GameObject[] pcard = GameObject.FindGameObjectsWithTag("PCard");
                        if (pcard != null && startGame.isStarted)
                        {
                            Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hitInfo))
                            {
                                if (hitInfo.collider.gameObject.tag == "CanSetTarger")
                                {
                                    if (inUI == false)
                                    {
                                        canMove = true;
                                        StartCoroutine(ClickOnGround(hitInfo));
                                    }
                                }
                                else
                                {
                                    canMove = false;
                                    anim.SetBool("Run", false);
                                }
                            }
                        }
                    }
                    else
                    {
                        canMove = false;
                        anim.SetBool("Run", false);
                        GameObject[] pcard = GameObject.FindGameObjectsWithTag("PCard");
                        if (pcard != null && startGame.isStarted)
                        {
                            Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hitInfo))
                            {
                                if (hitInfo.collider.gameObject.tag == "CanSetTarger")
                                {
                                    if (inUI == false)
                                    {
                                        canMove = true;
                                        StartCoroutine(ClickOnGround(hitInfo));
                                    }
                                }
                                else
                                {
                                    canMove = false;
                                    anim.SetBool("Run", false);
                                }
                            }
                        }
                    }
                }
            }
            if(canMove)
            {
                anim.SetBool("Run", true);
                if (walkIsStarted == false)
                {
                    walkIsStarted = true;
                    cum.StartWalkSound();
                }
                Quaternion targetRotation = Quaternion.LookRotation(hitInfo.point - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, hitInfo.point, moveSpeed * Time.deltaTime);
                if (transform.position == hitInfo.point)
                {
                    walkIsStarted = false;
                    canMove = false;
                    anim.SetBool("Run", false);
                }
            }
            else
            {
                cum.StopWalkSound();
            }
        }
    }

    private IEnumerator ClickOnGround(RaycastHit clickT)
    {
        Vector3 p = clickT.point;
        GameObject clickObj = Instantiate(clickPrefab, new Vector3(p.x, -1.6f, p.z), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(clickObj);
    }
}
