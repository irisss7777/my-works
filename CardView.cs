using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardView: MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Animator anim;

    public void PointerEnter()
    {
        anim.SetBool("Selecte", true);
    }

    public void PointerExit()
    {
        anim.SetBool("Selecte", false);
    }

    public void Click(int touchCount)
    {
        StartCoroutine(ChooseCard(touchCount));
    }

    private IEnumerator ChooseCard(int touchCount)
    {
        anim.SetTrigger("Choose");
        yield return new WaitForSeconds(0.01f);
        cardManager.StopChoose();
    }
}
