using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    public GameObject heroToken;
    public PowerUp[] availablePowerUpsForRooms;
    public RoomControl[] availableRoomsForPowerUps;
    RoomControl roomForPowerUp;
    RoomControl currentRoom;
    RoomControl nextRoom;
    Vector3 nextPosition;
    RoomControl[] availableRooms;

    bool isSetupPhase = true;
    bool isLayoutPhase = false;
    bool isWalkingPhase = false;
    bool isRespondingPhase = false;
    bool isFightPhase = false;


    public float stepSize = 0.03f;
    
    // Start is called before the first frame update
    void Start()
    {

        setupHeroToken();

        distributePowerUps();

        endPhase();

        startLayoutPhase();

    }


    // Update is called once per frame
    void Update()
    {
        if (isLayoutPhase)
        {
            layoutPhaseLoop();
        }

        if (isWalkingPhase)
        {
            walkingPhaseLoop();
        }

        if (isFightPhase)
        {
            fightPhaseLoop();
        }

        if (isRespondingPhase)
        {
            respondingPhaseLoop();
        }
    }

    private void startFightPhase()
    {
        isFightPhase = true;
    }
    private void fightPhaseLoop()
    {

    }

    private void startLayoutPhase()
    {
        isLayoutPhase = true;
    }


    private void layoutPhaseLoop()
    {

    }

    private void respondingPhaseLoop()
    {

        selectNextRoom();
        endPhase();
        startWalkingPhase();
    }

    private void startWalkingPhase()
    {
        isWalkingPhase = true;
    }

    void walkingPhaseLoop()
    {
        if ((heroToken.transform.position - nextPosition).magnitude > 0.05) { step(); }

        else
        {
            heroToken.transform.position = nextPosition;
            currentRoom = nextRoom;
            endPhase();
            startFightPhase();
        }
    }


    void endPhase()
    {
        bool isSetupPhase = false;
        bool isLayoutPhase = false;
        bool isWalkingPhase = false;
        bool isRespondingPhase = false;
        bool isFightPhase = false;
    }

    void step()
    {
        heroToken.transform.position += (nextPosition - heroToken.transform.position).normalized * stepSize;
    }

    void selectNextRoom()
    {
        availableRooms = currentRoom.rooms;
        if(availableRooms.Length > 0)
        {
            nextRoom = availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)];
        }
        nextPosition = nextRoom.transform.position + Vector3.up * 1;
    }


    void distributePowerUps()
    {
        for (int i = 0; i < availablePowerUpsForRooms.Length; i++)
        {
            getRandomRoom();
            availablePowerUpsForRooms[i].transform.position = roomForPowerUp.transform.position + Vector3.right * 0.5f;
            roomForPowerUp.powerUp = availablePowerUpsForRooms[i];
        }
    }

    void setupHeroToken()
    {
        currentRoom = FindAnyObjectByType<FirstRoom>().GetComponent<RoomControl>();
        availableRooms = currentRoom.rooms;
        nextRoom = availableRooms[UnityEngine.Random.Range(0, availableRooms.Length - 1)];
        nextPosition = nextRoom.transform.position + Vector3.up * 1;
    }

    void getRandomRoom()
    {
        do
        {
            roomForPowerUp = availableRoomsForPowerUps[UnityEngine.Random.Range(0, availableRoomsForPowerUps.Length)];
        } while (roomForPowerUp.powerUp != null);
    }
}
