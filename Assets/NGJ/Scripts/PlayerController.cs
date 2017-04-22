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
    // Use this for initialization
    void Start()
    {
      
    }
    public void Initialize()
    {
        Pad = new DSPad(PlayerIndex);
        GameObject GO = Instantiate(CharPrefab);
        GO.transform.SetParent(this.transform);
        GO.GetComponent<PlayerCharacter>().Initialize(this);
        Initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
       if(Initialized)Pad.CheckPadInput();
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

