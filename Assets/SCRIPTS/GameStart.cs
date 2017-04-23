using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("CorrStartGame");
    }
    IEnumerator CorrStartGame()
    {
        GuiAudioController.Instance.game_starting();
        yield return new WaitForSeconds(0.5f);
        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("MainMenu3d");
    }
    
}
