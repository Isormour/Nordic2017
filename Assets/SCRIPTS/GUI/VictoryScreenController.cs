using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
