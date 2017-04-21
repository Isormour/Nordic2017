using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class DSLocalMultiplayerManager : MonoBehaviour
{
    public static DSLocalMultiplayerManager Singleton;

    DSPad[] Pads;
    // Use this for initialization
    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            Pads = new DSPad[4];
            for (int i = 0; i < Pads.Length; i++)
            {
                int PadIndex = i;// cuse any state is first one
                Pads[i] = new DSPad((GamePad.Index)(PadIndex));
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Pads.Length; i++)
        {
            Pads[i].CheckPadInput();
        }
    }
    public DSPad GetPadInput(int PadIndex)
    {
        return Pads[PadIndex];
    }
}

public enum EButtonState
{
    down,
    up
}



public class DSPad
{
    public delegate void ButtonDown(EButtonState buttonState);
    public delegate void TriggerPressure(float pressureValue);
    public delegate void StickAxis(Vector2 deviation);


    const int padEnumNum = 10;
    const int stateEnumNum = 2;

    // pad events
    public event StickAxis OnLeftStickMove;
    public event StickAxis OnRightStickMove;

    public event TriggerPressure OnLeftTriggerPressed;
    public event TriggerPressure OnRightTriggerPressed;

    GamePad.Index PadIndex;
    bool HasJoined = false;
    public delegate void PlayerJoin(GamePad.Index PadIndex);
    public event PlayerJoin OnPlayerJoined;

    Dictionary<Tuple<GamePad.Button, EButtonState>, ButtonDown> ButtonMap;

    DSPadBehaviour CurrentBehaviour;
    Stack<DSPadBehaviour> BehaviourStack;

    public DSPad(GamePad.Index index)
    {
        BehaviourStack = new Stack<DSPadBehaviour>();
        PadIndex = index + 1;
        ButtonMap = new Dictionary<Tuple<GamePad.Button, EButtonState>, ButtonDown>();


        for (int i = 0; i < padEnumNum; i++)
        {
            for (int j = 0; j < stateEnumNum; j++)
            {
                Tuple<GamePad.Button, EButtonState> TempTuple = Tuple.Create<GamePad.Button, EButtonState>((GamePad.Button)i, (EButtonState)j);
                ButtonMap.Add(TempTuple, null);
            }
        }

    }
    public void SetBehaviour(DSPadBehaviour Behaviour)
    {

        BehaviourStack.Push(CurrentBehaviour);
        CurrentBehaviour = Behaviour;
        ApplyBehaviour();

    }
    public void BehaviourGoBack()
    {
        CurrentBehaviour = BehaviourStack.Pop();
        ApplyBehaviour();
    }
    void ApplyBehaviour()
    {
        //clearDelegates
        if (OnLeftStickMove != null)
        {
            foreach (Delegate d in OnLeftStickMove.GetInvocationList())
            {
                OnLeftStickMove -= (StickAxis)d;
            }
        }
        if (OnRightStickMove != null)
        {
            foreach (Delegate d in OnRightStickMove.GetInvocationList())
            {
                OnRightStickMove -= (StickAxis)d;
            }
        }
        if (OnLeftTriggerPressed != null)
        {
            foreach (Delegate d in OnLeftTriggerPressed.GetInvocationList())
            {
                OnLeftTriggerPressed -= (TriggerPressure)d;
            }
        }
        if (OnRightTriggerPressed != null)
        {
            foreach (Delegate d in OnRightTriggerPressed.GetInvocationList())
            {
                OnRightTriggerPressed -= (TriggerPressure)d;
            }
        }

        //apply current ones
        OnLeftStickMove += CurrentBehaviour.GetStickBehaviour(GamePad.Axis.LeftStick);
        OnRightStickMove += CurrentBehaviour.GetStickBehaviour(GamePad.Axis.RightStick);

        OnLeftTriggerPressed += CurrentBehaviour.GetTriggerPressure(GamePad.Trigger.LeftTrigger);
        OnRightTriggerPressed += CurrentBehaviour.GetTriggerPressure(GamePad.Trigger.RightTrigger);

        for (int i = 0; i < padEnumNum; i++)
        {
            for (int j = 0; j < stateEnumNum; j++)
            {
                Tuple<GamePad.Button, EButtonState> TempTuple = Tuple.Create<GamePad.Button, EButtonState>((GamePad.Button)i, (EButtonState)j);
                ButtonMap[TempTuple] = CurrentBehaviour.GetButtonBehaviour((GamePad.Button)i, (EButtonState)j);
            }
        }
    }

    public void CheckPadInput()
    {
        CheckSticks();
        CheckTriggers();
        CheckButtons();
    }

    void CheckSticks()
    {
        if (OnLeftStickMove != null)
        {
            OnLeftStickMove(GamePad.GetAxis(GamePad.Axis.LeftStick, PadIndex));
        }
        if (OnRightStickMove != null)
        {
            OnRightStickMove(GamePad.GetAxis(GamePad.Axis.RightStick, PadIndex));
        }
    }

    void CheckTriggers()
    {
        //right trigger
        if (OnRightTriggerPressed != null)
        {
            OnRightTriggerPressed(GamePad.GetTrigger(GamePad.Trigger.RightTrigger, PadIndex));
        }
        //left trigger
        if (OnLeftTriggerPressed != null)
        {
            OnLeftTriggerPressed(GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, PadIndex));
        }
    }

    void CheckButtons()
    {
        int padEnumNum = 10;
        //Get button down
        for (int i = 0; i < padEnumNum; i++)
        {
            if (GamePad.GetButtonDown((GamePad.Button)i, PadIndex))
            {
                Tuple<GamePad.Button, EButtonState> TempTuple = Tuple.Create<GamePad.Button, EButtonState>((GamePad.Button)i, EButtonState.down);
                if (ButtonMap[TempTuple] != null)
                {
                    ButtonMap[TempTuple](EButtonState.down);
                }
                if (!HasJoined)
                {
                    HasJoined = true;
                    if (OnPlayerJoined != null)
                    {
                        OnPlayerJoined(PadIndex);
                    }
                }
            }
        }
        //Get button up
        for (int i = 0; i < padEnumNum; i++)
        {
            if (GamePad.GetButtonUp((GamePad.Button)i, PadIndex))
            {
                Tuple<GamePad.Button, EButtonState> TempTuple = Tuple.Create<GamePad.Button, EButtonState>((GamePad.Button)i, EButtonState.up);
                if (ButtonMap[TempTuple] != null)
                {
                    ButtonMap[TempTuple](EButtonState.up);
                }
            }
        }
    }
    #region Behaviour
    public class DSPadBehaviour
    {
        Dictionary<GamePad.Axis, StickAxis> StickMap;
        Dictionary<GamePad.Trigger, TriggerPressure> TriggerMap;

        Dictionary<Tuple<GamePad.Button, EButtonState>, ButtonDown> ButtonMap;

        public DSPadBehaviour()
        {//create and fill maps
            StickMap = new Dictionary<GamePad.Axis, StickAxis>();
            StickMap.Add(GamePad.Axis.LeftStick, null);
            StickMap.Add(GamePad.Axis.RightStick, null);

            TriggerMap = new Dictionary<GamePad.Trigger, TriggerPressure>();
            TriggerMap.Add(GamePad.Trigger.LeftTrigger, null);
            TriggerMap.Add(GamePad.Trigger.RightTrigger, null);
            ButtonMap = new Dictionary<Tuple<GamePad.Button, EButtonState>, ButtonDown>();
            for (int i = 0; i < padEnumNum; i++)
            {
                for (int j = 0; j < stateEnumNum; j++)
                {
                    Tuple<GamePad.Button, EButtonState> TempTuple = Tuple.Create<GamePad.Button, EButtonState>((GamePad.Button)i, (EButtonState)j);
                    ButtonMap.Add(TempTuple, null);
                }
            }
        }
        public void AddStickBehaviour(GamePad.Axis Axis, StickAxis Delegate)
        {
            if (StickMap[Axis] == null)
            {
                StickMap[Axis] = Delegate;
            }
            else
            {
              //s  Debug.LogError("Behaviour Exists " + Axis + " " + Delegate.ToString());
            }
        }
        public void AddTriggerBehaviour(GamePad.Trigger Trigger, TriggerPressure Delegate)
        {
            if (TriggerMap[Trigger] == null)
            {
                TriggerMap[Trigger] = Delegate;
            }
            else
            {
               // Debug.LogError("Behaviour Exists " + Trigger + " " + Delegate.ToString());
            }
        }
        public void AddButtonBehaviour(GamePad.Button Button, EButtonState State, ButtonDown Delegate)
        {

            Tuple<GamePad.Button, EButtonState> TempTupl = new Tuple<GamePad.Button, EButtonState>(Button, State);
            if (ButtonMap[TempTupl] == null)
            {
                ButtonMap[TempTupl] = Delegate;
            }
            else
            {
                //Debug.LogError(" Behaviour Error " + Button + " " + Delegate.ToString());
            }
        }
        public StickAxis GetStickBehaviour(GamePad.Axis Axis)
        {
            if (StickMap[Axis] != null)
            {
                return StickMap[Axis];
            }
            else
            {
               // Debug.LogError(" Missing Behaviour " + Axis.ToString() + " " + StickMap.ToString());
                return null;
            }
        }
        public TriggerPressure GetTriggerPressure(GamePad.Trigger Trigger)
        {
            if (TriggerMap[Trigger] != null)
            {
                return TriggerMap[Trigger];
            }
            else
            {
                //Debug.LogError(" Missing Behaviour " + Trigger.ToString() + " " + TriggerMap.ToString());
                return null;
            }
        }
        public ButtonDown GetButtonBehaviour(GamePad.Button Button, EButtonState State)
        {
            Tuple<GamePad.Button, EButtonState> TempTupl = new Tuple<GamePad.Button, EButtonState>(Button, State);
            if (ButtonMap[TempTupl] != null)
            {
                return ButtonMap[TempTupl];
            }
            else
            {
               // Debug.LogError(" Behaviour Doesnt exists " + Button);
                return null;
            }
        }

    }
    #endregion
}
