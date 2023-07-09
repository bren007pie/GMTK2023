using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Descriptor : MonoBehaviour
{
    // Descripton box state variables
    [SerializeField] GameObject descriptorBox;
    [Tooltip("The offset of the tip")]
    public Vector3 tipOffset = new Vector3(82.5f, 79.0f, 0.0f); 
    Text descriptorBoxText;
    RectTransform descriptorBoxPosition;

    string description = "";

    private void Start()
    {
        descriptorBoxText = descriptorBox.GetComponentInChildren<Text>();
        descriptorBoxPosition = descriptorBox.GetComponentInChildren<RectTransform>();
        tipOffset = new Vector3(82.5f, 79.0f, 0.0f);
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

    void OnMouseOver()
    {
        // why the heck didn't I use the OnMouse Monobehaviour functions? WAY easier than raycasting
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
        spawnDescriptionBox(Input.mousePosition);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
        hideDescriptionBox();
    }

    void spawnDescriptionBox(Vector3 tipPoint)
    {
        descriptorBoxPosition.anchoredPosition3D = tipPoint + tipOffset; // set to tip point in pixel space
        descriptorBoxText.text = get_map_description(); // send the text to the box
        descriptorBox.SetActive(true); // turn er on
    }

    void hideDescriptionBox()
    {
        descriptorBox.SetActive(false);
    }
}
