using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public bool isLayoutPhase = false; // flag for entire layout phase
    public GameObject EndPlacementButton; // To set the state of the button

    // public variables for pick_up_and_drag
    [Tooltip("ALL Resources must be on resource layer and have coliders!")]
    public GameObject selectedResource; // object for clicking and dragging
    [Tooltip("ALL rooms must be on room layer and have coliders!")]
    public GameObject selectedROOM; // object for checking collision with room
    [SerializeField] LayerMask room;
    [SerializeField] LayerMask resource;
    [Tooltip("Offset from clicker when moving items")]
    public Vector3 mouseOffset; // mouseOffset from click and drag mouse position
    [Tooltip("Y Level which resources reset to/")]
    public float resetYLevel = -6.9f;



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
        GameObject insideResource; // for resources inside rooms, so doesn't conflict with selectedResource loop

        // Raycast, does every loop unfortunately but what can you do. Could be optimized to only do when mouse is down
        Collider2D targetResource = Physics2D.OverlapPoint(mousePosition, resource); // resource colider cast, only checks on the resource layer mask
        Collider2D targetROOM = Physics2D.OverlapPoint(mousePosition, room); // room colider cast, only checks room layer mask

        if (Input.GetMouseButtonDown(0))
        {
            if (targetResource)
            {
                selectedResource = targetResource.transform.gameObject;
                mouseOffset = selectedResource.transform.position - mousePosition;
            }
            if (targetROOM)  // if resource already there remove from object and select
            {
                selectedROOM = targetROOM.transform.gameObject;
                roomControl = selectedROOM.GetComponent<RoomControl>();
                if (roomControl.resource) // if there's a resource remove it!
                {
                    insideResource = roomControl.resource;
                    float x_pos = insideResource.transform.position.x;
                    float z_pos = insideResource.transform.position.z;
                    insideResource.transform.position = new Vector3(x_pos, resetYLevel, z_pos); // move to y level

                    Collider2D resourceCollider = insideResource.GetComponent<Collider2D>();
                    resourceCollider.enabled = true; // re-enabling the colider component to be clicker

                    roomControl.resource = null; // remove it from the room
                }
            }
        }
        if (selectedResource)
        {
            selectedResource.transform.position = mousePosition + mouseOffset;
        }
        
        if (Input.GetMouseButtonUp(0) && selectedResource) // letting go with a resource in hand
        {
            
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
                    selectedResource.transform.position =  new Vector3(x_pos, resetYLevel, z_pos); // Guard: sets down to bottom below where let go!
                    Debug.Log("Resource already in the room! Can only have one per room.");
                }
                else if (selectedROOM.TryGetComponent<FirstRoom>(out FirstRoom firstroom)) // Guard: Tries placing in first room
                {
                    selectedResource.transform.position = new Vector3(x_pos, resetYLevel, z_pos); // sets down to bottom below where let go!
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
