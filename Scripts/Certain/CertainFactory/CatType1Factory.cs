using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatType1Factory : CatFactory
{
    private int maxFoodCount;
    private Dictionary<string, int> foodType;
    private GameObject catPrefab;
    private Transform catTransform;

    public override void Init(GameObject cPrefab, Transform cTransform)
    {
        maxFoodCount = 10;
        foodType = new Dictionary<string, int>()
        {
            { "pig", 1 },
            { "chicken", 1 },
            { "fish", 1 },
        };
        catPrefab = cPrefab;
        catTransform = cTransform;
    }

    public override void CreateCat()
    {
        CatView cView = Instantiate(catPrefab, catTransform.position, Quaternion.identity).GetComponent<CatType1View>();
        Cat cat = new CatType1();
        InitCat(cat, cView);
    }
}
