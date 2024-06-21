using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatView : MonoBehaviour
{
    private Cat cat;

    public virtual void Init(Cat catt)
    {
        cat = catt;
    }

    public abstract void FoodIsEatenView();
}
