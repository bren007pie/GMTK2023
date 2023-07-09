using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    Hero hero;
    public Minion minion;
    public bool isFightPhase = false;
    public float turnTime = 0.5f;
    public Transform heroBar;
    public Transform minionBar;
    Vector3 heroBarStart;
    Vector3 minionBarStart;
    Vector3 heroBarEnd;
    Vector3 minionBarEnd;
    float timeSinceLastTurn = 0f;
    bool isHeroTurn = false;

    public AudioSource heroAttackSound;
    public AudioSource heroHitSound;
    public AudioSource goblinAttackSound;
    public AudioSource bigGoonAttackSound;
    public AudioSource goblinHitSound;
    public AudioSource bigGoonHitSound;
    public AudioSource goblinDieSound;
    public AudioSource bigGoonDieSound;

    int minionAtk = 0;
    int heroAtk = 0;
    int minionDef = 0;
    int heroDef = 0;

    // Start is called before the first frame update
    void Start()
    {
        hero = FindAnyObjectByType<Hero>().GetComponent<Hero>();


        heroBarStart = heroBar.transform.position;
        heroBarEnd = heroBarStart + Vector3.left * 11f;
        minionBarStart = minionBar.transform.position;
        minionBarEnd = minionBarStart + Vector3.right * 11f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFightPhase)
        {
            // sound effects here in fight script
            if(timeSinceLastTurn == 0f)
            {
                timeSinceLastTurn = Time.time;
            }

            if (hero.getHealth() > 0 && minion.getHealth() > 0)
            {
                if(Time.time - timeSinceLastTurn > turnTime)
                {
                    turn();
                    timeSinceLastTurn = Time.time;
                    heroBar.position = Vector3.Lerp(heroBarEnd, heroBarStart, (float)hero.getHealth() / (float)hero.getMaxHealth());
                    minionBar.position = Vector3.Lerp(minionBarEnd, minionBarStart, (float)minion.getHealth() / (float)minion.getMaxHealth());
                }
            }
            else
            {
                if (minion.getHealth()<=0)
                {
                    if (minion.name == "3 Gobalins")
                    {
                        goblinDieSound.Play();
                    }
                    else
                    {
                        bigGoonDieSound.Play();
                    }
                }
                
                isFightPhase = false;
            }
        }
    }

    void turn()
    {
        if (isHeroTurn)
        {
            heroTurn();
            heroAttackSound.Play();
            if (minion.name == "3 Gobalins")
            {
                goblinHitSound.Play();
            }
            else
            {
                bigGoonHitSound.Play();
            }
            isHeroTurn = false;
        }
        else
        {
            minionTurn();
            heroHitSound.Play();
            if(minion.name == "3 Gobalins")
            {
                goblinAttackSound.Play();
            }
            else
            {
                bigGoonAttackSound.Play();
            }
            isHeroTurn = true;
        }
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
