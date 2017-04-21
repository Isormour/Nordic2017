using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    PlayerController Player;
    DSPad.DSPadBehaviour CharBehaviour;
    float Speed = 5.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(PlayerController playerController)
    {
        Player = playerController;
        CharBehaviour = new DSPad.DSPadBehaviour();
        CharBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, LeftStick);
        CharBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A,EButtonState.down, ButtonADown);


        Player.PushBehaviour(CharBehaviour);
        Debug.Log("Initialized");

    }

   

    #region BehaviourDelegates
    void LeftStick(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {
            this.transform.position += (new Vector3(AxisVect.x, 0, AxisVect.y) * Speed);
        }
    }
    private void ButtonADown(EButtonState buttonState)
    {
        Debug.Log("Button A");
    }
    #endregion
}
