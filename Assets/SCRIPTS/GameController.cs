using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance = null;

    public struct PlayerData
    {
        public int player_id;
        public int selection_id;
        public float last_input_time;
        public int controller_id;
        public bool active;
        public bool made_choice;
    }

    public PlayerData[] players;

    public int max_players;

    // Use this for initialization
    void Awake()
    {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        // initialize players
        players = new PlayerData[max_players];
        for (int i = 0; i < max_players; i++)
        {
            players[i].player_id = i;
            players[i].selection_id = -1;
            players[i].last_input_time = 0;
            players[i].active = false;
            players[i].made_choice = false;
        }

        //make player 0 active
        ActivatePlayer(0);
        
    }

    private void ActivatePlayer(int player_id)
    {
        players[player_id].active = true;
    }

}
