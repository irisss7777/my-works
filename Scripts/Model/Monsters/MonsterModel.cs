using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour, IDamageble
{
    [SerializeField] private int[] myDamageTypeNCount = { 0, 2, 1, 0};
    private int meleeDamage = 0;
    private int distanceDamage = 0;
    private int magicDamage = 0;
    [Range (0, 3)][SerializeField] private int universalDamage;
    [Range (0, 2)][SerializeField] private int[] bonusDamage = new int[4];
    [Range (0, 2)][SerializeField] private int bonusHealth;
    [Range (0, 2)][SerializeField] private int bonusKey;
    [Range (0, 1)][SerializeField] private int bonusMove;
    [Range (0, 4)][SerializeField] private int bonusMoney;

    private MonstersView monstersView;

    private void Start()
    {
        monstersView = this.gameObject.GetComponent<MonstersView>();
    }

    public void ApplyDamage(int[] damage)
    {
        monstersView.PlayDeathAnimation();
        Debug.Log("Monster is dead");
    }

    public void Attack(IDamageble character)
    {
        int[] damage = { meleeDamage, distanceDamage, magicDamage, universalDamage };
        character.TryAttackThis(damage, this);
        monstersView.PlayAttackAnimation();
    }

    public bool TryAttackThis(int[] damageTypeNCount, IDamageble character)
    {
        bool iCanAttackThis = true;
        for(int i = 0; i < 3; i++)
        {
            if(damageTypeNCount[i] > 0)
            {
                if(damageTypeNCount[i] < myDamageTypeNCount[i])
                {
                    iCanAttackThis = false;
                }
            }
        }
        if(iCanAttackThis)
        {
            ApplyDamage(damageTypeNCount);
        }
        else
        {
            Attack(character);
        }
        return iCanAttackThis;
    }

    public void GetBonus(out int bonusType, out int bonusValue)
    {
        bonusValue = 0;
        bonusType = 0;
        for(int i = 0; i < bonusDamage.Length; i++)
        {
            if(bonusDamage[i] > 0)
            {
                bonusValue = bonusDamage[i];
                bonusType = i;
                break;
            }
        }
        if(bonusHealth > 0)
        {
            bonusValue = bonusHealth;
            bonusType = 4;
        }
        if(bonusKey > 0)
        {
            bonusValue = bonusKey;
            bonusType = 5;
        }
        if(bonusMove > 0)
        {
            bonusValue = bonusMove;
            bonusType = 6;
        }
        if(bonusMoney > 0)
        {
            bonusValue = bonusMoney;
            bonusType = 7;
        }
    }
}
