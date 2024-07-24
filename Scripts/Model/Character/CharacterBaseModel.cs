using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharactersBaseModel : IDamageble
{
    private int maxHealth;
    private int currentHealth;
    private int CurrentHealth
    {
        set
        {
            if(value > 0)
            {
                if(currentHealth - value > 0)
                {
                    characterView.PlayApplyDamageAnimation();
                }
                else
                {
                    characterView.PlayDeathAnimation();
                }
                currentHealth -= value;
            }
        }
    }
    private int[] myDamageTypeNCount = { 0, 0, 0, 0};
    private CharacterView characterView;
    private BonusCollector bonusCollector;
    private IDamageble lastTarget;
    private CharacerController characerController;

    public CharactersBaseModel(int newMaxhealth, int[] newAttackDamage, CharacterView charView, BonusCollector bCollector, CharacerController charController)
    {
        characerController = charController;
        Subscribe();
        bonusCollector = bCollector;
        bonusCollector.SetMoveCountToStartPosition();
        maxHealth = newMaxhealth + bonusCollector.GetBonusHealth();
        currentHealth = maxHealth;
        Debug.Log(maxHealth);
        if(newAttackDamage[0] > 0 || newAttackDamage[1] > 0 || newAttackDamage[2] > 0)
        {
            int[] bonusDamage = bonusCollector.GetBonusDamage();
            for(int i = 0; i < newAttackDamage.Length; i++)
            {
                myDamageTypeNCount[i] = newAttackDamage[i] + bonusDamage[i];
            }
        }
        else
        {
            Debug.Log("Not correct damage in constructor on object: ");
        }
        characterView = charView;
    }
    
    public void Attack(IDamageble target)
    {
        if(target is MonsterModel)
        {
            bool attackIsSucessesful  = target.TryAttackThis(myDamageTypeNCount, this);
            if(attackIsSucessesful)
            {
                characterView.PlayAttackAnimation();
                lastTarget = null;
                int bonusType;
                int bonusValue;
                target.GetBonus(out bonusType,  out bonusValue);
                GiveBonus(bonusType, bonusValue);
            }
            else
            {
                Debug.Log("I'm not have so many damage...");
            }
        }
    }

    public void ApplyDamage(int[] newDamage)
    {
        int damage = newDamage[3];
        CurrentHealth = damage;
        Debug.Log("Ouch, i'm get hit - " + newDamage[3]);
    }

    public bool TryAttackThis(int[] damageTypeNCount, IDamageble target)
    {
        bool canAttackThis = true;;
        if(target is MonsterModel)
        {
            Debug.Log("Character Has been atacked");
            ApplyDamage(damageTypeNCount);
            return canAttackThis;
        }
        else
        {
            canAttackThis = false;
            return canAttackThis;
        }
    }

    public void TryAttackOnEndMove()
    {
        if(lastTarget != null)
        {
            Debug.Log("Can Attack");
            Attack(lastTarget);
        }
        else
        {
            Debug.Log("Empty target");
        }
    }

    public void GiveBonus(int bonusType, int bonusValue)
    {
        if(bonusType <= 3)
        {
            myDamageTypeNCount[bonusType] += bonusValue;
            bonusCollector.AddBonusDamage(bonusType, bonusValue);
        }
        else
        {
        switch(bonusType)
        {
            case 4:
            maxHealth += bonusValue;
            currentHealth += bonusValue;
            bonusCollector.AddDonusHealth(bonusValue);
            Debug.Log(maxHealth + currentHealth);
            break;
            case 5:
            bonusCollector.AddKeyCount(bonusValue);
            break;
            case 6:
            bonusCollector.AddMoveCount(bonusValue);
            break;
            case 7:
            bonusCollector.AddMoney(bonusValue);
            break;
        }
        }
    }

    public void GetBonus(out int bonusType, out int bonusValue)
    {
        bonusType = 0;
        bonusValue = 0;
    }

    public void SetTarget(IDamageble target)
    {
        lastTarget = target;
    }

    public void Subscribe()
    {
        characerController.OnEndMove += TryAttackOnEndMove;
    }

    public void UnSubscribe()
    {
        characerController.OnEndMove -= TryAttackOnEndMove;
        Debug.Log("Unsubscribe!");
    }
}
