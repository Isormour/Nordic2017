using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string button_type;
    
    public void Action()
    {
        if (button_type == "start")
            StartGame();
        if (button_type == "credits")
            Credits();
        if (button_type == "quit")
            QuitGame();
    }

    public void StartGame()
    {
        GuiController.Instance.StartGame();
    }
    public void Credits()
    {
        Debug.Log("Not implemented.");
        GuiController.Instance.QuitGame();
    }
    public void QuitGame()
    {
        GuiController.Instance.QuitGame();
    }
}
