using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // for now attack and defense debuffs should be negative

    [Tooltip("HERO Defense die debuff on traps, must be negative value ")]
    [SerializeField] int DEF_die_debuff = 0;
    [Tooltip("HERO Defense bonus debuff on traps, must be negative value ")]
    [SerializeField] int DEF_bonus_debuff = 0;
    [Tooltip("HERO Attack die debuff on traps, must be negative value ")]
    [SerializeField] int ATK_die_debuff = 0;
    [Tooltip("HERO Attack bonus debuff on traps, must be negative value ")]
    [SerializeField] int ATK_bonus_debuff = 0;



    // checks to make sure, if change in editor during playing we're fucked
    private void Awake()
    {
        if (DEF_die_debuff > 0)
        {
            Debug.LogWarning("DEFENSE DIE DEBUFF MUST BE NEGATIVE");
        }
        if (DEF_bonus_debuff > 0)
        {
            Debug.LogWarning("DEFENSE BONUS DEBUFF MUST BE NEGATIVE");
        }
        if (ATK_die_debuff > 0)
        {
            Debug.LogWarning("ATTACK DIE DEBUFF MUST BE NEGATIVE");
        }
        if (ATK_bonus_debuff > 0)
        {
            Debug.LogWarning("ATTACK BONUS DEBUFF MUST BE NEGATIVE");
        }
    }

    // TODO add for X number of layers
}
