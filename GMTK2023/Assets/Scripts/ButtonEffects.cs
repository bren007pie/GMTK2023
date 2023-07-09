using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonEffects : MonoBehaviour
{
    public Color buttonTintColour = new Color(212.0f, 212.0f, 212.0f);
    Color originalcolour;
    AudioSource buttonAudio;
    Image buttonSprite;

    private void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
        originalcolour = buttonSprite.color;
        buttonTintColour = new Color(212.0f, 212.0f, 212.0f);
    }



    private void OnMouseEnter()
    {
        buttonSprite.color = buttonTintColour;
        buttonAudio.Play();
    }

    private void OnMouseDown()
    {
        
    }


    private void OnMouseExit()
    {
        buttonSprite.color = originalcolour;

    }


}
