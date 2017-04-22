using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu3dController : MonoBehaviour
{
    /*
     REQUIRES:
     GameController
     GuiController
     GuiAudioController
     */

    public GameObject[] menu_positions;
    //public GameObject[] menu_selectors;

    public float input_delay = 0.5f;

    public string start_input_label = "start";
    public string vertical_input_label = "Vertical";
    public string horizontal_input_label = "Horizontal";
    public string action_input_label = "Action";

    private int options_count;

    private int menu_selection_id;

    GameController.PlayerData[] players;

    int i = 0;

    private void Awake()
    {
        options_count = menu_positions.Length;

        // only first player controls menu
        i = 0;

        menu_selection_id = 0;

        //UpdateSelector(i, 0);
        UpdateColor(0, Color.red);

    }

    void Start()
    {
        //PlayerController first_player;
        //first_player = GameManager.Singleton.GetFirstPlayer();

        //DSPad.DSPadBehaviour MenuBehaviour;

        ////menu controls
        //MenuBehaviour = new DSPad.DSPadBehaviour();
        //MenuBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, MenuMove);
        //MenuBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A, EButtonState.down, MenuSelect);
        //first_player.PushBehaviour(MenuBehaviour);

        //GameController.Instance.players;
        players = GameController.Instance.players;

    }

    // Update is called once per frame
    void Update()
    {
        //read inputs and update selection
        if (Time.timeSinceLevelLoad - players[i].last_input_time > input_delay)
        {

            // INVERTED - id goes top to down
            if (Input.GetAxis(vertical_input_label + players[i].player_id) > 0)
            {
                Debug.Log(vertical_input_label + players[i].player_id + ", positive");

                UpdateColor(menu_selection_id, Color.white);
                menu_selection_id = Mathf.Max(0, menu_selection_id - 1);

                players[i].last_input_time = Time.timeSinceLevelLoad;
                UpdateColor(menu_selection_id, Color.red);
            }
            else if (Input.GetAxis(vertical_input_label + players[i].player_id) < 0)
            {
                Debug.Log(vertical_input_label + players[i].player_id + ", negative");

                UpdateColor(menu_selection_id, Color.white);
                menu_selection_id = Mathf.Min(options_count - 1, menu_selection_id + 1);

                players[i].last_input_time = Time.timeSinceLevelLoad;
                UpdateColor(menu_selection_id, Color.red);
            }


            if (Input.GetButtonDown(action_input_label + players[i].player_id))
            {
                Debug.Log(action_input_label + players[i].player_id);
                ActivateSelection(menu_selection_id);
            }

        }

    }

    public void UpdateSelector(int player, int position)
    {
        Debug.Log("updating position: player:" + player + ", position:" + position);
        //menu_selectors[player].transform.position = menu_positions[position].transform.position;
        menu_positions[position].GetComponent<Renderer>().material.color = Color.red;
    }

    public void UpdateColor(int position, Color new_color)
    {
        Debug.Log("updating color: Color:" + new_color + ", position:" + position);
        //menu_selectors[player].transform.position = menu_positions[position].transform.position;
        menu_positions[position].GetComponent<Renderer>().material.color = new_color;
    }

    public void ActivateSelection(int position)
    {
        Debug.Log("ActivateSelection:" + position);
        menu_positions[position].GetComponent<ButtonController>().Action();
    }

    void MenuMove(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {
            Debug.Log(AxisVect);

            Vector3 SpeedVect = new Vector3(AxisVect.x, 0, AxisVect.y);
            Vector3 lookPosition = this.transform.position + SpeedVect;
            this.transform.LookAt(lookPosition);
        }
    }

    void MenuSelect(EButtonState buttonState)
    {
        Debug.Log("Dash!");
        if (this.gameObject.active)
        {
            //StartCoroutine(Dash());
        }
    }


}
