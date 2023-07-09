using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public bool isLayoutPhase = false; // flag for entire layout phase
    public GameObject EndPlacementButton; // To set the state of the button

    // public variables for pick_up_and_drag
    public GameObject selectedResource; // object for clicking and dragging
    [Tooltip("ALL ROOMS MUST BE ON LAYER 6 (ROOM LAYER)")]
    public GameObject selectedROOM; // object for checking collision with room
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

        if (isLayoutPhase) // if it's in the layout phase
        {
            EndPlacementButton.SetActive(true); // turn on button until done placing

            pick_up_and_drag(); // execute clicking up and down action
        }




    }

    // For the button to set the isLayoutPhase flag to be off once it's pressed
    public void toggle_isLayoutPhase()
    {
        isLayoutPhase = !isLayoutPhase;
    }

    // Click and drag stolen from: https://gamedevbeginner.com/how-to-move-an-object-with-the-mouse-in-unity-in-2d/#overlap_point
    void pick_up_and_drag()
    {
        // function scoped state variables 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RoomControl roomControl; // for the selected room to insert data

        Collider2D targetResource = Physics2D.OverlapPoint(mousePosition, resource); // dos the cast, only checks on the resource layer mask

        if (Input.GetMouseButtonDown(0))
        {
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
        
        if (Input.GetMouseButtonUp(0) && selectedResource) // letting go with a resource in hand
        {
            Collider2D targetROOM = Physics2D.OverlapPoint(selectedResource.transform.position, room); // does collision between resource and place where dropped
            if (targetROOM) // checks targetROOM 
            {
                // check to make sure it's a room!
                // gets objects and components
                selectedROOM = targetROOM.transform.gameObject; // fuck me lmao, it selects itself haha, use layermasks
                roomControl = selectedROOM.GetComponent<RoomControl>();
                float x_pos = selectedResource.transform.position.x;
                float z_pos = selectedResource.transform.position.z;
                if (roomControl.resource) // if already has a resource in there reset it
                {
                    selectedResource.transform.position =  new Vector3(x_pos, -6.9f, z_pos); // Guard: sets down to bottom below where let go!
                    Debug.Log("Resource already in the room! Can only have one per room.");
                }
                else if (selectedROOM.TryGetComponent<FirstRoom>(out FirstRoom firstroom)) // Guard: Tries placing in first room
                {
                    selectedResource.transform.position = new Vector3(x_pos, -6.9f, z_pos); // sets down to bottom below where let go!
                    Debug.Log("THIS IS THE FIRST ROOM! Can't be in first room.");
                }
                else if (roomControl) // if nothing in there
                {
                    // puts in the right place
                    roomControl.resource = selectedResource; // puts resource in room
                    selectedResource.transform.position = selectedROOM.transform.position + new Vector3(-0.5f, 0, -0.5f); // aligns to the left and infront of room
                    targetResource.enabled = false; // disable colider component so can't be clicked
                }

            }

            selectedResource = null; // drops the selected thing
        }


    }
}
