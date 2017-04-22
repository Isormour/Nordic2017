using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
        
    public GameObject main_menu;
    public GameObject credits;

    public static MainMenuController Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        StartCoroutine("CStartGame");
    }
    IEnumerator CStartGame()
    {
        //if (clickAudio != null)
        //clickAudio.Play();
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        StartCoroutine("CQuitGame");
    }
    IEnumerator CQuitGame()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void Credits()
    {
        StartCoroutine("CCredits");
    }
    IEnumerator CCredits()
    {
        GuiAudioController.Instance.button_pressed();
        yield return new WaitForSeconds(0.5f);
        // SceneManager.LoadScene("Credits");
        credits.SetActive(true);
        main_menu.SetActive(false);
    }

    public void CloseCredits()
    {
        StartCoroutine("CCloseCredits");
    }
    IEnumerator CCloseCredits()
    {
        GuiAudioController.Instance.button_pressed();
        yield return new WaitForSeconds(0.5f);
        //SceneManager.LoadScene("MainMenu");
        credits.SetActive(false);
        main_menu.SetActive(true);
    }
}
