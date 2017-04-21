using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    PlayerController Player;
    DSPad.DSPadBehaviour CharBehaviour;
    float Speed = 100.1f;
    Rigidbody CharacterRigid;

    Axe CurrentAxe;

    Chest CurrentChest;
    // Use this for initialization
    void Start()
    {
        CurrentAxe = null;
        CharacterRigid = GetComponent<Rigidbody>();

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
            StartCoroutine (RespCorr());
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
    public IEnumerator RespCorr()
    {
        yield return new WaitForSeconds(5);
        transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        Player.BehaviourGoBack();
        transform.gameObject.SetActive(true);
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
            CurrentAxe.Throw();
            CurrentAxe.transform.localPosition += this.transform.forward;
            CurrentAxe.transform.rotation = this.transform.rotation;
     

            CurrentAxe = null;
        }
    }
    #endregion
}
