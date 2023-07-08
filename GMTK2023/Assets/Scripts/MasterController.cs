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
    public FightManager fightManager;
    Minion minionInCurrentRoom;
    Trap trapInCurrentRoom;
    PowerUp powerUpInCurrentRoom;
    RoomControl roomForPowerUp;
    RoomControl currentRoom;
    RoomControl nextRoom;
    Vector3 nextPosition;
    RoomControl[] availableRooms;

    // flags for controlling phases
    bool isSetupPhase = true;
    bool isLayoutPhase = false;
    bool isWalkingPhase = false;
    bool isRespondingPhase = false;
    bool isFightPhase = false;


    public float stepSize = 0.03f;

    // Setup Phase
    void Start()
    {

        setupHeroToken();

        distributePowerUps();

        endPhase();

        startFightPhase();

    }


    // Main loop with all the phases going together
    void Update()
    {
        if (isLayoutPhase)
        {
            layoutPhaseLoop();
        }

        else if (isWalkingPhase)
        {
            walkingPhaseLoop();
        }

        else if (isFightPhase)
        {
            fightPhaseLoop();
        }

        else if (isRespondingPhase)
        {
            respondingPhaseLoop();
        }
    }

    private void startFightPhase()
    {
        isFightPhase = true;

        minionInCurrentRoom = null;
        trapInCurrentRoom = null;

        if(currentRoom.resource != null)
        {
            minionInCurrentRoom = currentRoom.resource.GetComponent<Minion>();
            trapInCurrentRoom = currentRoom.resource.GetComponent<Trap>();

        }
        //powerUpInCurrentRoom = currentRoom.powerUp.GetComponent<PowerUp>();

        if(minionInCurrentRoom != null) { fightManager.minion = minionInCurrentRoom; fightManager.isFightPhase = true; }
        //if(trapInCurrentRoom != null) { }
        
    }
    private void fightPhaseLoop()
    {
        if(fightManager.isFightPhase == false)
        {
            endPhase();
            startRespondingPhase();
        }
    }

    private void startRespondingPhase()
    {
        isRespondingPhase = true;
    }

    private void startLayoutPhase()
    {
        isLayoutPhase = true;
    }


    private void layoutPhaseLoop()
    {
        // this is where we'll put the selection
    }

    private void respondingPhaseLoop()
    {
        endPhase();
        startWalkingPhase();
    }

    private void startWalkingPhase()
    {
        selectNextRoom();
        isWalkingPhase = true;
    }

    void walkingPhaseLoop()
    {
        if ((heroToken.transform.position - nextPosition).magnitude > 0.02) { step(); }

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
        isSetupPhase = false;
        isLayoutPhase = false;
        isWalkingPhase = false;
        isRespondingPhase = false;
        isFightPhase = false;
    }

    void step()
    {
        heroToken.transform.position += (nextPosition - heroToken.transform.position).normalized * stepSize;
        //heroToken.transform.position = nextPosition;
    
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
    }

    void getRandomRoom()
    {
        do
        {
            roomForPowerUp = availableRoomsForPowerUps[UnityEngine.Random.Range(0, availableRoomsForPowerUps.Length)];
        } while (roomForPowerUp.powerUp != null); // placing thing in the public variable of the room
    }
}
