using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    // State Variables
    public RoomControl[] rooms;
    public GameObject resource; // BOSS RESOURCE (trap or minion or win room) in the ROOM
    public PowerUp powerUp; // the powerup the HERO gets for beating the ROOM

    // Start is called before the first frame update
    void Start()
    {
    }


}
