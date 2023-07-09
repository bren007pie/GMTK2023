using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descriptor : MonoBehaviour
{
    string description = "";

    public string get_map_description()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string get_map_description()
    {
        description = "";
        if (gameObject.TryGetComponent<Trap>(out Trap trap))
        {
            if (trap.ATK_bonus_debuff < 0)
            {
                description = "Bear trap\n-" + trap.ATK_bonus_debuff + " to ATK";
            }
            else if (trap.DEF_bonus_debuff < 0)
            {
                description = "Acid vat\n-" + trap.DEF_bonus_debuff + " to DEF";
            }
        }
        else if (gameObject.TryGetComponent<Minion>(out Minion minion))
        {
            description = minion.name + "\nHP " + minion.getHealth() + "/" + minion.getMaxHealth() + "\nATK " + minion.getATK_mult() + "D" + minion.getATK_die() + " + " + minion.getATK_bonus() + "\n DEF D" + minion.getDEF_die() + " + " + minion.getDEF_bonus();
        }
        else if (gameObject.TryGetComponent<PowerUp>(out PowerUp powerUp))
        {
            if (powerUp.healing > 0)
            {
                description = "Potion\n" + powerUp.healing + "HP";
            }
            else if (powerUp.atk_die > 0)
            {
                description = "Sword\n+ D" + powerUp.atk_die + " + " + powerUp.atk_bonus;
            }
            else if (powerUp.def_die > 0)
            {
                description = "Shield\n+ D" + powerUp.def_die + " + " + powerUp.def_bonus;
            }
        }
        else if (gameObject.TryGetComponent<Hero>(out Hero hero))
        {
            description = "Hero\nHP " + hero.getHealth() + "/" + hero.getMaxHealth() + "\nATK " + "D" + hero.getATK_die() + " + " + hero.getATK_bonus() + "\n DEF D" + hero.getDEF_die() + " + " + hero.getDEF_bonus();
        }

        return description;
    }
}
