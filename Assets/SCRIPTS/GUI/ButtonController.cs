using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string button_type;
    
    public void Action()
    {
        Debug.Log("ButtonController Action: " + button_type);

        if (button_type == "map")
            Menus3DManager.Instance.MapSelection();
        if (button_type == "credits")
            Menus3DManager.Instance.Credits();
        if (button_type == "quit")
            Menus3DManager.Instance.QuitGame();
        if (button_type == "main")
            Menus3DManager.Instance.MainMenu();

        if (button_type == "skin1")
        {
            Menus3DManager.Instance.map_id = 1;
            Menus3DManager.Instance.SkinSelection();
        }
        if (button_type == "skin2")
        {
            Menus3DManager.Instance.map_id = 2;
            Menus3DManager.Instance.SkinSelection();
        }
        if (button_type == "skin3")
        {
            Menus3DManager.Instance.map_id = 3;
            Menus3DManager.Instance.SkinSelection();
        }

        if (button_type == "start")
            Menus3DManager.Instance.StartGame();
    }

    //public void StartGame()
    //{
    //    //GuiController.Instance.StartGame();
    //    //Transform target = Menu3dController.Instance.menu_camera.
    //    //Menu3dController.Instance.menu_camera.set_mount();
    //    Transform camera_mount = Menus3DManager.Instance.map_select_menu.camera_mount;
    //    Menus3DManager.Instance.menu_camera.set_mount(camera_mount);
    //}
    //public void Credits()
    //{
    //    Debug.Log("Not implemented.");
    //    GuiController.Instance.QuitGame();
    //}
    //public void QuitGame()
    //{
    //    GuiController.Instance.QuitGame();
    //}
    //public void MainMenu()
    //{
    //    Debug.Log("Not implemented.");
    //    GuiController.Instance.QuitGame();
    //}

}
