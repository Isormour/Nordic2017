using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
      PlayerController[] Players =  GameManager.Singleton.GetPlayerControllers();
        for (int i = 0; i < Players.Length; i++)
        {
            DSPad.DSPadBehaviour ColorBehav = new DSPad.DSPadBehaviour();
            
            Players[i].PushBehaviour(ColorBehav);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
