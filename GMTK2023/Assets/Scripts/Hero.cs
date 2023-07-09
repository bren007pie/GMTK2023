using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    [SerializeField] int Health = 50;
    [SerializeField] int MaxHealth = 50;
    [SerializeField] int DEF_die = 6;
    [SerializeField] int DEF_bonus = 0;
    [SerializeField] int ATK_die = 8;
    [SerializeField] int ATK_bonus = 2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getHealth() { return Health; }
    public int getDEF_die() { return DEF_die; }
    public int getDEF_bonus() { return DEF_bonus; }
    public int getATK_die() { return ATK_die; }
    public int getATK_bonus() { return ATK_bonus; }

    public int damage(int damage)
    {
        if (Health - damage > 0) { Health -= damage; }
        else { Health = 0; dead(); }
        return Health;
    }

    public int heal(int healing)
    {
        if (Health + healing < MaxHealth) { Health += healing; }
        else { Health = MaxHealth; }
        return Health;
    }

    public int changeDEF_die(int change)
    {
        DEF_die += change;
        return DEF_die;
    }

    public int changeATK_die(int change)
    {
        ATK_die += change;
        return ATK_die;
    }

    public int changeDEF_bonus(int change)
    {
        DEF_bonus += change;
        return DEF_bonus;
    }

    public int changeATK_bonus(int change)
    {
        ATK_bonus += change;
        return ATK_bonus;
    }

    public int getMaxHealth() { return MaxHealth; }
    public int setMaxHealth(int newMax)
    {
        if (Health > newMax) { Health = newMax; }
        MaxHealth = newMax;

        return MaxHealth;
    }

    void dead()
    {

    }

}
