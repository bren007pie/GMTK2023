using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public bool isLayoutPhase = false; // flag for entire layout phase
    public GameObject EndPlacementButton; // To set the state of the button

    public GameObject selectedResource; // object for clicking and dragging
    [Tooltip("ALL ROOMS MUST BE ON LAYER 6 (ROOM LAYER)")]
    public GameObject selectedROOM; // object for checking collision with room
    RoomControl roomControl; // for the selected room to insert data
    [SerializeField] LayerMask room;
    [SerializeField] LayerMask resource;


    Vector3 offset; // offset from click and drag mouse position

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLayoutPhase) // if is layout phase should take control of the sequence
        {
            EndPlacementButton.SetActive(true); // turn on button until done placing
        }

        // Click and drag stolen from: https://gamedevbeginner.com/how-to-move-an-object-with-the-mouse-in-unity-in-2d/#overlap_point
        if (isLayoutPhase) // Only allow clicking and dragging when in layout phase
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetResource = Physics2D.OverlapPoint(mousePosition, resource); // Layer Mask: ONLY selects things on that layer, use layermask object not an int
                if (targetResource)
                {
                    selectedResource = targetResource.transform.gameObject;
                    offset = selectedResource.transform.position - mousePosition;
                }
            }
            if (selectedResource)
            {
                selectedResource.transform.position = mousePosition + offset;
            }
            // when lets go checks if you placed it over a ROOM
            if (Input.GetMouseButtonUp(0) && selectedResource)
            {
                Collider2D targetROOM = Physics2D.OverlapPoint(selectedResource.transform.position, room); // does collision between resource and place where dropped
                if (targetROOM) // checks targetROOM 
                {
                    // check to make sure it's a room!
                    // gets objects and components
                    selectedROOM = targetROOM.transform.gameObject; // fuck me lmao, it selects itself haha
                    roomControl = selectedROOM.GetComponent<RoomControl>();
                    if (roomControl)
                    {
                        // puts in the right place
                        roomControl.resource = selectedResource; // puts resource in room
                        selectedResource.transform.position = selectedROOM.transform.position + new Vector3(-0.5f, 0, -0.5f); // aligns to the left and infront of
                    }
                    
                }

                selectedResource = null; // gets rid of the selected thing
            }

        }


    }

    // For the button to set the isLayoutPhase flag to be off once it's pressed
    public void toggle_isLayoutPhase()
    {
        isLayoutPhase = !isLayoutPhase;
    }
}
