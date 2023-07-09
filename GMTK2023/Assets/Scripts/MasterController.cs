using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    public GameObject heroToken;
    public Hero hero;
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
    public LayoutController layoutController;
    public GameObject CombatView; // The Parent object of all the combat ones
    public GameObject BigGoob;
    public GameObject ThreeGobalins;

    // UI screens
    public GameObject MainUi;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    // Music
    public AudioSource PlaceMusic;
    public AudioSource CombatMusic;
    public AudioSource GameOverSound;
    public AudioSource BossVictorySound;
    public AudioSource PowerUpSound;
    public AudioSource DeathSound;

    // flags for controlling phases
    bool isSetupPhase = true;
    bool isLayoutPhase = false;
    bool isWalkingPhase = false;
    bool isRespondingPhase = false;
    bool isFightPhase = false;


    public float stepSize = 0.05f;

    // Setup Phase
    void Start()
    {

        setupHeroToken();

        distributePowerUps();

        endPhase();

        startLayoutPhase();

    }


    // Main loop with all the phases going together
    void Update()
    {
        if (isLayoutPhase) // this blocks control until layout is done
        {
            layoutPhaseLoop();
        }

        else if (isRespondingPhase)
        {
            respondingPhaseLoop();
        }


        else if (isFightPhase)
        {
            fightPhaseLoop();
        }


        else if (isWalkingPhase)
        {
            walkingPhaseLoop();
        }
    }

    private void startFightPhase()
    {
        isFightPhase = true;


        minionInCurrentRoom = null;
        trapInCurrentRoom = null;
        powerUpInCurrentRoom = null;

        if (currentRoom.resource != null)
        {
            minionInCurrentRoom = currentRoom.resource.GetComponent<Minion>();
            trapInCurrentRoom = currentRoom.resource.GetComponent<Trap>();

        }
        if(currentRoom.powerUp != null)
        {
            powerUpInCurrentRoom = currentRoom.powerUp.GetComponent<PowerUp>();
        }

        if(minionInCurrentRoom != null) { 
            fightManager.minion = minionInCurrentRoom; 
            fightManager.isFightPhase = true;



            // making screen active, only goes when isFightPhase is active
            MainUi.SetActive(false);
            CombatView.SetActive(true); // combat screen up
            if (minionInCurrentRoom.name == "Big Goob")
            {
                BigGoob.SetActive(true);
            }
            else if (minionInCurrentRoom.name == "3 Gobalins")
            {
                ThreeGobalins.SetActive(true);
            }
            else
            {
                Debug.LogWarning("The minion in room " + currentRoom.name + " does not match any of the minion names");
            }

        }
        if(trapInCurrentRoom != null) { handleTrap(); }
        
    }

    private void handleTrap()
    {
        hero.changeATK_bonus(trapInCurrentRoom.ATK_bonus_debuff);
        hero.changeATK_die(trapInCurrentRoom.ATK_die_debuff);
        hero.changeDEF_bonus(trapInCurrentRoom.DEF_bonus_debuff);
        hero.changeDEF_die(trapInCurrentRoom.DEF_die_debuff);

        if (trapInCurrentRoom.ATK_bonus_debuff + trapInCurrentRoom.ATK_die_debuff < 0) { Debug.Log("The Hero's leg got caught in a bear trap in the dungeon room! His ATK has gone down!"); }
        if (trapInCurrentRoom.DEF_bonus_debuff + trapInCurrentRoom.DEF_die_debuff < 0) { Debug.Log("A vat of acid fell onto the Hero's armour in the dungeon room! His DEF has gone down!"); }
    }

    private void fightPhaseLoop()
    {
        if (hero.getHealth() < 1)
        {
            lose();
        }
        if(fightManager.isFightPhase == false)
        {
            if(hero.getHealth() > 0 && powerUpInCurrentRoom != null) { handlePowerup(); }
            CombatView.SetActive(false); // combat screen down and remove goblins
            MainUi.SetActive(true);
            BigGoob.SetActive(false);
            ThreeGobalins.SetActive(false);
            endPhase();
            startRespondingPhase();
        }
    }


    private void handlePowerup()
    {
        if (powerUpInCurrentRoom.win == true)
        {
            win();
        }
        hero.heal(powerUpInCurrentRoom.healing);
        hero.changeATK_bonus(powerUpInCurrentRoom.atk_bonus);
        hero.changeATK_die(powerUpInCurrentRoom.atk_die);
        hero.changeDEF_bonus(powerUpInCurrentRoom.def_bonus);
        hero.changeDEF_die(powerUpInCurrentRoom.atk_die);

        PowerUpSound.Play();

        if (powerUpInCurrentRoom.healing > 0) { Debug.Log("The Hero was healed for " + powerUpInCurrentRoom.healing + " HP by the health potion he found in the dungeon room."); }
        if (powerUpInCurrentRoom.atk_bonus + powerUpInCurrentRoom.atk_die > 0) { Debug.Log("The Hero found a new sword in the dungeon room. His ATK has gone up!"); }
        if (powerUpInCurrentRoom.def_bonus + powerUpInCurrentRoom.def_die > 0) { Debug.Log("The Hero found a new shield in the dungeon room. His DEF has gone up!"); }
    }


    private void startRespondingPhase()
    {
        isRespondingPhase = true;
    }

    private void startLayoutPhase()
    {
        isLayoutPhase = true;
        layoutController.isLayoutPhase = true; // sets the LayoutController.cs variable to be active so will start

    }


    private void layoutPhaseLoop()
    {
        // while wait and hand over "control" to the layout phase
        if (layoutController.isLayoutPhase == false)
        {
            // Changing music when exiting layout phase
            PlaceMusic.Stop();
            CombatMusic.Play();
            endPhase();
            startWalkingPhase();
        }
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
        if ((heroToken.transform.position - nextPosition).magnitude > 0.06) { step(); }

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
            int[] roomRating = new int[availableRooms.Length];
            int highestValue = -10;
            int highestIndex = -10;
            
            for(int i = 0; i < roomRating.Length; i++)
            {
                roomRating[i] = 0;
                if (availableRooms[i].powerUp == null || availableRooms[i].powerUp.healing>0)
                {
                    roomRating[i] -= 1;
                }
                else if ( (float)hero.getHealth()/ (float)hero.getMaxHealth() < 0.5f && availableRooms[i].powerUp.healing>0)
                {
                    roomRating[i] += 4;
                }

                if (availableRooms[i].resource == null)
                {
                    roomRating[i] += 1;
                }

                if (roomRating[i] > highestValue)
                {
                    highestIndex = i;
                    highestValue = roomRating[i];
                }
            }

            nextRoom = availableRooms[highestIndex];

            //nextRoom = availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)];
        }
        nextPosition = nextRoom.transform.position + Vector3.up * 2;
    }


    void distributePowerUps()
    {
        for (int i = 0; i < availablePowerUpsForRooms.Length; i++)
        {
            getRandomRoom();
            availablePowerUpsForRooms[i].transform.position = roomForPowerUp.transform.position + Vector3.right * 1f + Vector3.down * 0.5f;
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

    private void win()
    {
        endPhase();
        MainUi.SetActive(false);
        LoseScreen.SetActive(true);
        CombatMusic.Stop();
        BossVictorySound.Play();
        DeathSound.Play();
        GameObject.Destroy(MainUi); // Fuckin nuke it
        GameObject.Destroy(fightManager);
        Time.timeScale = 0f; // pauses game so we don't have to fix a bug!
        GameObject.Destroy(this);
    }
    private void lose()
    {
        endPhase();
        MainUi.SetActive(false);
        WinScreen.SetActive(true);
        CombatMusic.Stop();
        GameOverSound.Play();
        GameObject.Destroy(MainUi);
        GameObject.Destroy(fightManager);
        Time.timeScale = 0f; // pauses game so we don't have to fix a bug!
        GameObject.Destroy(this);
    }

}
