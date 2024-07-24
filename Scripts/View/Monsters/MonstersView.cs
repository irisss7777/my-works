using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersView : MonoBehaviour
{
    private Animator myAnim;

    private void Start()
    {
        myAnim = this.gameObject.GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        // myAnim.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        // myAnim.SetTrigger("Death");
    }
}
