using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    PlayerController Player;
    DSPad.DSPadBehaviour CharBehaviour;
    float Speed = 130.1f;
    float DashForce = 40.0f;
    Rigidbody CharacterRigid;

    Axe CurrentAxe;
    bool IsDashLocked;
    bool IsDashing;
    Chest CurrentChest;
    // Use this for initialization
    void Start()
    {
        CurrentAxe = null;
        CharacterRigid = GetComponent<Rigidbody>();
        IsDashLocked = false;
        IsDashing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(PlayerController playerController)
    {
        Player = playerController;
        CharBehaviour = new DSPad.DSPadBehaviour();
        CharBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, LeftStick);
        CharBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A, EButtonState.down, ButtonADown);
        CharBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.B, EButtonState.down, ButtonBDown);
        Player.PushBehaviour(CharBehaviour);
        Debug.Log("Initialized");

    }

    internal void OnEnterChestRange(Chest chest)
    {
        CurrentChest = chest;
    }

    internal void OnExitChestRange(Chest chest)
    {
        CurrentChest = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Axe axe = collision.gameObject.GetComponent<Axe>();
        if (axe)
        {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<BoxCollider>().enabled = false;
            DSPad.DSPadBehaviour beh = new DSPad.DSPadBehaviour();
            Player.PushBehaviour(beh);
            StartCoroutine(RespCorr());
        }
        PlayerCharacter OtherPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (OtherPlayer)
        {
            if (IsDashing)
            {
                IsDashing = false;
                OtherPlayer.GetComponent<Rigidbody>().AddForce(this.transform.forward * DashForce*1.1f,ForceMode.Impulse);
            }
        }
    }


    #region BehaviourDelegates
    void LeftStick(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {

            Vector3 SpeedVect = new Vector3(AxisVect.x, 0, AxisVect.y);
            CharacterRigid.AddForce(SpeedVect * Speed);

            Vector3 lookPosition = this.transform.position + SpeedVect;
            this.transform.LookAt(lookPosition);

        }

    }
    public void OnEnterPickableAxe(Axe axeToPickup)
    {
        CurrentAxe = axeToPickup;
    }
    public void OnExitPickableAxe(Axe axeToPickup)
    {
        if (axeToPickup == CurrentAxe)
        {
            CurrentAxe = null;
        }
    }

    public IEnumerator RespCorr()
    {
        yield return new WaitForSeconds(5);
        transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        Player.BehaviourGoBack();
        transform.gameObject.SetActive(true);
    }
    private void ButtonBDown(EButtonState buttonState)
    {
        //Dash
        if (!IsDashLocked)
        {
            Debug.Log("Dash!");
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        IsDashing = true;
        IsDashLocked = true;
        GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.2f, 0.2f);
        CharacterRigid.AddForce(this.transform.forward * DashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);

        GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.8f);
        IsDashing = false;

        yield return new WaitForSeconds(3.0f);
        GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.8f, 0.2f);
        IsDashLocked = false;
       
    }

    private void ButtonADown(EButtonState buttonState)
    {
        if (!CurrentAxe)
        {
            if (CurrentChest)
            {
                if (!CurrentChest.GetIsOpen())
                {
                    CurrentChest.OpenChest(this);
                }
                else
                {
                    if (CurrentChest.GetHaveAxe())
                    {
                        CurrentAxe = CurrentChest.PickupAxe(this);
                    }
                }
            }
        }
        else
        {
            if (CurrentAxe.transform.parent == this.transform)
            {
                CurrentAxe.Throw();
                CurrentAxe.transform.localPosition += this.transform.forward;
                CurrentAxe.transform.rotation = this.transform.rotation;

                CurrentAxe = null;
            }
            else
            {
                CurrentAxe.Pickup(this.transform);
            }

        }
    }
    #endregion
}
