using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    Hero hero;
    public Minion minion;
    public bool isFightPhase = false;

    int minionAtk = 0;
    int heroAtk = 0;
    int minionDef = 0;
    int heroDef = 0;

    // Start is called before the first frame update
    void Start()
    {
        hero = FindAnyObjectByType<Hero>().GetComponent<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFightPhase)
        {
            if (hero.getHealth() > 0 && minion.getHealth() > 0)
            {
                turn();
            }
            else
            {
                isFightPhase = false;
            }
        }
    }

    void turn()
    {
        minionTurn();
        heroTurn();
    }

    void minionTurn()
    {
        minionAtk = dieRoll(minion.getATK_die(), minion.getATK_bonus());
        heroDef = dieRoll(hero.getDEF_die(), hero.getDEF_bonus());

        if (heroDef > minionAtk) { heroDef = minionAtk; }

        if( minion.getATK_mult() > 0 )
        {
            Debug.Log("The minion attacked the Hero " + minion.getATK_mult() + " times for " + minionAtk + " each but the hero's defences reduced each by " + heroDef + "! \nThe hero's health is down to " + hero.damage((minionAtk - heroDef) * minion.getATK_mult()));
        }
        else{
            Debug.Log("The minion attacked the Hero for " + minionAtk + " but the hero's defences reduced it by " + heroDef + "! \nThe hero's health is down to " + hero.damage(minionAtk - heroDef));
        }
    }

    void heroTurn()
    {
        heroAtk = dieRoll(hero.getATK_die(), hero.getATK_bonus());
        minionDef = dieRoll(minion.getDEF_die(), minion.getDEF_bonus());

        if (minionDef > heroAtk) { minionDef = heroAtk; }


        Debug.Log("The Hero attacked the minion for " + heroAtk + " but the minion's defences reduced it by " + minionDef + "! \nThe minion's health is down to " + minion.damage(heroAtk - minionDef));
        
    }

    int dieRoll(int die, int bonus)
    {
        return UnityEngine.Random.Range(1, die) + bonus;
    }
}
