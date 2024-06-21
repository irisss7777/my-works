using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatFactory : MonoBehaviour
{
    private int maxFoodCount;
    private Dictionary<string, int> foodType;

    public abstract void Init(GameObject cPrefab, Transform cTransform);

    public abstract void CreateCat();

    public virtual void InitCat(Cat cat, CatView cView)
    {
        cat.Init(maxFoodCount, foodType, cView);
        cView.Init(cat);
    }

}
