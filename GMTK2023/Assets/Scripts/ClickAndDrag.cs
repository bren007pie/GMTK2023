using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stolen from: https://gamedevbeginner.com/how-to-move-an-object-with-the-mouse-in-unity-in-2d/#overlap_point
public class ClickAndDrag : MonoBehaviour
{
    // Only allow clicking and dragging when in layout phase
    
    public GameObject selectedObject;
    Vector3 offset;
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                // check target object is TRAP or MINNION
                // or try using layermasks here
                selectedObject = targetObject.transform.gameObject;
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
            // here it checks that you placed it over a map place
            // Make sure only one RESOURCE here
            // Make sure not in FIRST ROOM or WIN ROOM
        }
    }
}