using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    // State Variables
    public GameObject next_room_1;  // can have up to three new rooms but does not need to be populated
    public GameObject next_room_2;
    public GameObject next_room_3;
    public GameObject[] resources; // array of BOSS RESOURCEs (traps and minions) in the ROOM
    public GameObject player; // the player object

    // Start is called before the first frame update
    void Start()
    {
        // get object componenets
        /* RoomControl next_room_1_component = GetComponent<RoomControl>();
        RoomControl next_room_2_component = GetComponent<RoomControl>();
        RoomControl next_room_3_component = GetComponent<RoomControl>();*/
        // RESOURCE[] resources; // array of BOSS RESOURCEs (traps and minions) in the ROOM
        // PLAYER player; // the player object

    }


}
