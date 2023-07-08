using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public bool isLayoutPhase = false; // flag
    public GameObject EndPlacementButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLayoutPhase) // if is layout phase should take control of the sequence
        {
            EndPlacementButton.SetActive(true);
            Debug.Log("I AM LAYOUT IT ALL OUT!");
        }
        

    }

    // For the button to set the isLayoutPhase flag to be off once it's pressed
    public void toggle_isLayoutPhase()
    {
        isLayoutPhase = !isLayoutPhase;
    }
}
