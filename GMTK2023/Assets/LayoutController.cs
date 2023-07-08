using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public bool isLayoutPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLayoutPhase) // if is layout phase should take control of the sequence
        {
            Debug.Log("I AM LAYOUT IT ALL OUT!");
        }
        isLayoutPhase = false;

    }
}
