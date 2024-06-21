using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatType1 : Cat
{
    private CatView catView;

    public override void FoodIsEaten()
    {
        catView.FoodIsEatenView();
        Debug.Log("Cat Type 1 Eat");
    }
}
