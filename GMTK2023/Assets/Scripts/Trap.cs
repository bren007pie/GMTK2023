using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // for now attack and defense debuffs should be negative

    [Tooltip("HERO Attack debug on traps, must be negative value ")]
    public int Attack_Debuff; // 
    [Tooltip("HERO defense debug on traps, must be negative value ")]
    public int Defense_Debuff;

    // checks to make sure, if change in editor during playing we're fucked
    private void Awake()
    {
        if (Attack_Debuff > 0)
        {
            Debug.LogWarning("ATTACK DEBUG MUST BE NEGATIVE");
        }
        if (Defense_Debuff > 0)
        {
            Debug.LogWarning("DEFENSE DEBUG MUST BE NEGATIVE");
        }
    }
}
