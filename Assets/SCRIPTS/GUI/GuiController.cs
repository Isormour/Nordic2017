using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuiController : MonoBehaviour
{
        
    public static GuiController Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void MainMenu()
    {
        StartCoroutine("CorrMainMenu");
    }
    IEnumerator CorrMainMenu()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerSelection()
    {
        StartCoroutine("CorrPlayerSelection");
    }
    IEnumerator CorrPlayerSelection()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PlayerSelection");
    }

    public void StartGame()
    {
        StartCoroutine("CorrStartGame");
    }
    IEnumerator CorrStartGame()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameplayMain");
    }

    public void QuitGame()
    {
        StartCoroutine("CorrQuitGame");
    }
    IEnumerator CorrQuitGame()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void VictoryScreen()
    {
        StartCoroutine("CorrVictoryScreen");
    }
    IEnumerator CorrVictoryScreen()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("VictoryScreen");
    }

}
