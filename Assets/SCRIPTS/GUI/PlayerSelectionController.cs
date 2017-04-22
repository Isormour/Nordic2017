using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionController : MonoBehaviour {

    public struct PlayerData
    {
        public int player_id;
        public int selection_id;
        public float last_input_time;
        public int controller_id;
        public bool active;
        public bool made_choice;
    }

    public float input_delay = 0.5f;

    public PlayerData[] players;
    public GameObject[] selectors;
    public GameObject[] positions;
    int max_selections;
    public int max_players;
    public int active_players;

    public int locked_players;

    public string start_input_label = "start";
    public string horizontal_input_label = "Horizontal";
    public string action_input_label = "Action";

    // Use this for initialization
    void Awake()
    {
        // initialize players
        players = new PlayerData[max_players];
        for (int i = 0; i < max_players; i++)
        {
            players[i].player_id = i;
            players[i].selection_id = -1;
            players[i].last_input_time = 0;
            players[i].active = false;
            players[i].made_choice = false;

            selectors[i].SetActive(false);

            Debug.Log(start_input_label + players[i].player_id);
            Debug.Log(players[i].selection_id);
        }

        max_selections = positions.Length;

        locked_players = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //foreach (PlayerData player_data in players)
        for (int i = 0; i < max_players; i++)
        {
            //Debug.Log(i);

            if (players[i].active == true && players[i].made_choice == false)
            {
                //read inputs and update selection
                if (Time.timeSinceLevelLoad - players[i].last_input_time > input_delay)
                {
                    if (Input.GetAxis(horizontal_input_label + players[i].player_id) < 0)
                    {
                        Debug.Log(horizontal_input_label + players[i].player_id + ", left");

                        players[i].selection_id = Mathf.Max(0, players[i].selection_id - 1);
                        players[i].last_input_time = Time.timeSinceLevelLoad;
                        UpdateSelector(i, players[i].selection_id);
                    }
                    else if (Input.GetAxis(horizontal_input_label + players[i].player_id) > 0)
                    {
                        Debug.Log(horizontal_input_label + players[i].player_id + ", right");
                        
                        // players[i].selection_id = Mathf.Min(max_players - 1, players[i].selection_id + 1);
                        players[i].selection_id = Mathf.Min(max_selections - 1, players[i].selection_id + 1);

                        players[i].last_input_time = Time.timeSinceLevelLoad;
                        UpdateSelector(i, players[i].selection_id);
                    }

                    if (Input.GetButtonDown(action_input_label + players[i].player_id))
                    {
                        Debug.Log(action_input_label + players[i].player_id);

                        players[i].made_choice = true;
                        //disable selector
                        //selectors[i].SetActive(false);

                        locked_players += 1;
                        if (locked_players == active_players)
                        {
                            GuiController.Instance.StartGame();
                        }
                    }
                }
            }
            else if (Input.GetButtonDown(start_input_label + players[i].player_id))
            {
                Debug.Log("Player activated:" + players[i].player_id);
                Debug.Log("Player made_choice:" + players[i].made_choice);
                Debug.Log("Player selection_id:" + players[i].selection_id);

                //if player pressed start make him active
                players[i].active = true;
                selectors[i].SetActive(true);
                
                active_players += 1;
            }

        }
    }

    public void UpdateSelector(int player, int position)
    {
        Debug.Log("updating position: player:" + player+ ", position:" + position);
        selectors[player].transform.position = positions[position].transform.position;
    }

}
