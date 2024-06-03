using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.Burst.CompilerServices;
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
    [SerializeField] private AvatarView avatarView;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>()
    }

    private void FixedUpdate()
    {
        if(view.IsMine)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (canMove == false)
            {
                Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "CanSetTarger")
                    {
                        canMove = true;
                        avatarView.StartMove();
                    }
                }
            }
            else
            {
                canMove = false;
                Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "CanSetTarger")
                    {
                        canMove = false;
                        avatarView.StopMove();
                    }
                }
            }
        }
        if (canMove)
        {
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
                canMove = false;
                avatarView.StopMove();
            }
        }
    }
}
