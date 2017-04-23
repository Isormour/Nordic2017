using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus3DManager : MonoBehaviour
{
    /*
     REQUIRES:
     GameController
     GuiController
     GuiAudioController
     MenuCameraController / CameraRig
     */

    public Menu3dController main_menu;
    public Menu3dController credits_menu;
    public Menu3dController map_select_menu;
    public Menu3dController skin_select_menu;
    
    public GameObject[] current_menu_positions;
    //public GameObject[] menu_selectors;
    private int options_count;

    public float input_delay = 0.5f;

    public string vertical_input_label = "Vertical";
    public string horizontal_input_label = "Horizontal";
    public string action_input_label = "Action";

    private int menu_selection_id;

    GameController.PlayerData[] players;

    int i = 0;

    public MenuCameraController menu_camera;

    public static Menus3DManager Instance = null;

    private void Awake()
    {
        // only first player controls menu
        i = 0;

        //menu_selection_id = 0;

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        PlayerController first_player;
        first_player = GameManager.Singleton.GetFirstPlayer();

        DSPad.DSPadBehaviour MenuBehaviour;

        //menu controls
        MenuBehaviour = new DSPad.DSPadBehaviour();
        MenuBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, MenuMove);
        MenuBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A, EButtonState.down, MenuSelect);
        first_player.PushBehaviour(MenuBehaviour);

        //initialize main menu data
        MainMenu();

        //UpdateSelector(i, 0);
        UpdateColor(0, Color.red);

        //GameController.Instance.players;
        players = GameController.Instance.players;

        //options_count = current_menu_positions.Length;

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

    void MenuMoveDown()
    {
        UpdateColor(menu_selection_id, Color.white);
        menu_selection_id = Mathf.Max(0, menu_selection_id - 1);

        players[i].last_input_time = Time.timeSinceLevelLoad;
        UpdateColor(menu_selection_id, Color.red);
    }
    void MenuMoveUp()
    {
        UpdateColor(menu_selection_id, Color.white);
        menu_selection_id = Mathf.Min(options_count - 1, menu_selection_id + 1);

        players[i].last_input_time = Time.timeSinceLevelLoad;
        UpdateColor(menu_selection_id, Color.red);
    }
    //public void UpdateSelector(int player, int position)
    //{
    //    Debug.Log("updating position: player:" + player + ", position:" + position);
    //    //menu_selectors[player].transform.position = current_menu_positions[position].transform.position;
    //    current_menu_positions[position].GetComponent<Renderer>().material.color = Color.red;
    //}

    public void UpdateColor(int position, Color new_color)
    {
        Debug.Log("updating color: Color:" + new_color + ", position:" + position);
        //menu_selectors[player].transform.position = current_menu_positions[position].transform.position;
        //current_menu_positions[position].GetComponent<Renderer>().material.color = new_color;
        current_menu_positions[position].GetComponentInChildren<Renderer>().material.color = new_color;
    }

    public void ActivateSelection(int position)
    {
        Debug.Log("ActivateSelection:" + position);
        current_menu_positions[position].GetComponent<ButtonController>().Action();
    }

    void MenuMove(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {
            if (AxisVect.y < 0)
            {
                MenuMoveDown();
            }
            else if (AxisVect.y > 0)
            {
                MenuMoveUp();
            }
        }
    }

    void MenuSelect(EButtonState buttonState)
    {
        ActivateSelection(menu_selection_id);
    }


    // BUTTONS ACTIONS

    //public void SwitchMenu(string menu_name)
    //{
    //    Menu3dController target_menu;

    //    if (menu_name == "start")

    //}

    public void MainMenu()
    {
        //Debug.Log("Not implemented.");
        //GuiController.Instance.QuitGame();
        menu_camera.set_mount(main_menu.camera_mount);
        current_menu_positions = main_menu.menu_positions;
        menu_selection_id = 0;
        options_count = current_menu_positions.Length;
    }
    public void MapSelection()
    {
        //GuiController.Instance.StartGame();
        //Transform target = Menu3dController.Instance.menu_camera.
        //Menu3dController.Instance.menu_camera.set_mount();
        //Transform camera_mount = Menus3DManager.Instance.map_select_menu.camera_mount;
        //Menus3DManager.Instance.menu_camera.set_mount(map_select_menu.camera_mount);
        menu_camera.set_mount(map_select_menu.camera_mount);
        current_menu_positions = map_select_menu.menu_positions;
        menu_selection_id = 0;
        options_count = current_menu_positions.Length;
    }
    public void Credits()
    {
        //Debug.Log("Not implemented.");
        //GuiController.Instance.QuitGame();
        menu_camera.set_mount(credits_menu.camera_mount);
        current_menu_positions = credits_menu.menu_positions;
        menu_selection_id = 0;
        options_count = current_menu_positions.Length;
    }
    public void SkinSelection()
    {
        //GuiController.Instance.StartGame();
        //Transform target = Menu3dController.Instance.menu_camera.
        //Menu3dController.Instance.menu_camera.set_mount();
        //Transform camera_mount = Menus3DManager.Instance.map_select_menu.camera_mount;
        //Menus3DManager.Instance.menu_camera.set_mount(map_select_menu.camera_mount);
        menu_camera.set_mount(skin_select_menu.camera_mount);
        current_menu_positions = skin_select_menu.menu_positions;
        menu_selection_id = 0;
        options_count = current_menu_positions.Length;
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame.");
        GuiController.Instance.QuitGame();
    }
    public void StartGame()
    {
        Debug.Log("StartGame.");
        GuiController.Instance.StartGame();
    }

}
