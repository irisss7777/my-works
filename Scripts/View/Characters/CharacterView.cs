using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private Animator myAnim;

    private void Start()
    {
        myAnim = this.gameObject.GetComponent<Animator>();
    }

    public void PlayMoveAnimation()
    {
        // myAnim.SetBool("Move", true);
    }

    public void StopMoveAnimation()
    {
        // myAnim.SetBool("Move", false);
    }

    public void PlayAttackAnimation()
    {
        // myAnim.SetTrigger("Attack");
    }

    public void PlayApplyDamageAnimation()
    {
        // myAnim.SetTrigger("ApplyDamage");
    }

    public void PlayDeathAnimation()
    {
        // myAnim.SetTrigger("Death");
        Destroy(this.gameObject);
    }
}
