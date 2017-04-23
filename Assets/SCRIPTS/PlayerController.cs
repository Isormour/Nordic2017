using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    DSPad Pad;
    public GamepadInput.GamePad.Index PlayerIndex;
    public GameObject CharPrefab;
    bool Initialized = false;
    Color PlayerColor;
    // Use this for initialization
    void Start()
    {

    }
    public void Initialize()
    {
        Pad = new DSPad(PlayerIndex);
        switch (PlayerIndex)
        {
            case GamepadInput.GamePad.Index.Any:
                {
                    PlayerColor = new Color(1.0f, 0, 0);
                }
                break;
            case GamepadInput.GamePad.Index.One:
                PlayerColor = new Color(0, 1.0f, 0);
                break;
            case GamepadInput.GamePad.Index.Two:
                PlayerColor = new Color(0, 0, 1.0f);
                break;
            case GamepadInput.GamePad.Index.Three:
                PlayerColor = new Color(1.0f, 1.0f, 1.0f);
                break;
            case GamepadInput.GamePad.Index.Four:
                break;
            default:
                break;
        }
        //GameObject GO = Instantiate(CharPrefab);
        // GO.transform.SetParent(this.transform);
        // GO.GetComponent<PlayerCharacter>().Initialize(this);
        //if (PlayerIndex == GamepadInput.GamePad.Index.Any)
        // {
        //     GO.GetComponent<PlayerCharacter>().debugSteer = true;
        // }
        Initialized = true;
    }
    // Update is called once per frame
    public void InitializeChar()
    {
        GameObject GO = Instantiate(CharPrefab);
        GO.transform.SetParent(this.transform);
        GO.GetComponent<PlayerCharacter>().Initialize(this);
        GO.GetComponent<PlayerCharacter>().BodyColor.material.color = PlayerColor;

    }
    void Update()
    {
        if (Initialized) Pad.CheckPadInput();
    }

    public Color GetColor()
    {
        return PlayerColor;
    }

    public void PushBehaviour(DSPad.DSPadBehaviour NewBehaviour)
    {
        Pad.SetBehaviour(NewBehaviour);
    }

    internal void BehaviourGoBack()
    {
        Pad.BehaviourGoBack();
    }
}

