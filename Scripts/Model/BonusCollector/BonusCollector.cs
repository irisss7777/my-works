using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollector
{
    private int[] bonusDamage = { 0, 0, 0, 0};

    private int bonusHealth;
    private int keyCount = 0;
    private int moneyCount = 0;
    private int chestCount = 0;
    private int maxMoveCount = 5;
    private int currentMoveCount;

    public void AddBonusDamage(int damageType, int damageCount)
    {
        if(damageType < bonusDamage.Length)
        {
            bonusDamage[damageType] += damageCount;
        }
    }

    public int[] GetBonusDamage()
    {
        return bonusDamage;
    }

    public void AddDonusHealth(int bHealth)
    {
        bonusHealth += bHealth;
    }

    public int GetBonusHealth()
    {
        return bonusHealth;
    }

    public void AddKeyCount(int count)
    {
        keyCount += count;
    }

    public void AddMoveCount(int count)
    {
        maxMoveCount += count;
    }

    public void AddMoney(int count)
    {
        moneyCount += count;
    }

    public void SetMoveCountToStartPosition()
    {
        currentMoveCount = maxMoveCount;
    }

    public bool MoveToTarget()
    {
        bool canMove = true;
        if(currentMoveCount - 1 >= 0)
        {
            currentMoveCount -= 1;
        }
        else
        {
            canMove = false;
        }
        return canMove;
    }
}
