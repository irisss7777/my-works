using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cat
{
    private int foodCount;
    public int FoodCount
    {
        get
        {
            return foodCount;
        }
        set
        {
            if(value < 0) value = 0;
            if(value > maxFoodCount) value = maxFoodCount;
            foodCount = value;
        }
    }
    private int maxFoodCount;
    private Dictionary<string, int> foodType;
    private CatView catView;

    public virtual void Init(int fMaxCount, Dictionary<string, int> fType, CatView cView)
    {
        maxFoodCount = fMaxCount;
        FoodCount = maxFoodCount;
        foodType = fType;
        catView = cView;
    }

    public virtual void EatFood(int foodValue, string foodName)
    {
        if (foodType[foodName] == 1)
        {
            this.FoodCount += foodValue;
            FoodIsEaten();
        }
    }

    public abstract void FoodIsEaten();

}
