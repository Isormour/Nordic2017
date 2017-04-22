using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject main_menu;
    public GameObject credits;

    // Use this for initialization
    void Awake ()
    {
        credits.SetActive(false);
        main_menu.SetActive(true);
    }

    public void Credits()
    {
        StartCoroutine("CorrCredits");
    }
    IEnumerator CorrCredits()
    {
        GuiAudioController.Instance.button_pressed();
        yield return new WaitForSeconds(0.5f);
        credits.SetActive(true);
        main_menu.SetActive(false);
    }

    public void CloseCredits()
    {
        StartCoroutine("CorrCloseCredits");
    }
    IEnumerator CorrCloseCredits()
    {
        GuiAudioController.Instance.button_pressed();
        yield return new WaitForSeconds(0.5f);
        credits.SetActive(false);
        main_menu.SetActive(true);
    }
    
    public void PlayerSelection()
    {
        GuiController.Instance.PlayerSelection();
    }
    
    public void QuitGame()
    {
        GuiController.Instance.QuitGame();
    }
    
}
