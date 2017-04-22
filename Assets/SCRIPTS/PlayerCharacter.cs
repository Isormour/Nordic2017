using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public delegate void PlayerDeath(PlayerCharacter DeadPlayer,Axe KillerAxe);
    public event PlayerDeath OnPlayerDeath;

    PlayerController Player;
    DSPad.DSPadBehaviour InGameBehaviour;
    DSPad.DSPadBehaviour LockedBehaviour;
    DSPad.DSPadBehaviour TotalLock;
    float Speed = 130.1f;
    float DashForce = 40.0f;
    Rigidbody CharacterRigid;

    Axe CurrentAxe;
    bool IsDashLocked;
    bool IsDashing;
    bool IsPushed;
    Chest CurrentChest;

    MeshRenderer MeshColor;

    Animator Anim;
    // Use this for initialization
    void Start()
    {
        CurrentAxe = null;
        CharacterRigid = GetComponent<Rigidbody>();
        IsDashLocked = false;
        IsDashing = false;
        IsPushed = false;
        GameplayManager.Sceneton.AddCharacter(this);
        transform.gameObject.SetActive(false);
        Anim = GetComponentInChildren<Animator>();
        MeshColor = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(PlayerController playerController)
    {
        Player = playerController;
        InGameBehaviour = new DSPad.DSPadBehaviour();
        InGameBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, MoveByStick);
        InGameBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A, EButtonState.down, ButtonADown);
        InGameBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.B, EButtonState.down, ButtonBDown);
        Player.PushBehaviour(InGameBehaviour);


        LockedBehaviour = new DSPad.DSPadBehaviour();
        LockedBehaviour.AddStickBehaviour(GamepadInput.GamePad.Axis.LeftStick, LookByStick);
        Player.PushBehaviour(LockedBehaviour);

        TotalLock = new DSPad.DSPadBehaviour();

        Debug.Log("Initialized");


    }

    internal void OnEnterChestRange(Chest chest)
    {
        CurrentChest = chest;
    }

    internal void OnGameStarted()
    {
        Player.PushBehaviour(InGameBehaviour);
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
            //MeshColor.enabled = false;
            transform.GetComponent<BoxCollider>().enabled = false;
            //StartCoroutine(RespCorr());
            if (OnPlayerDeath != null)
            {
                OnPlayerDeath(this, axe);
            }
            AudioClip Die = SoundManager.Singleton.Die;
            SoundManager.CreateSound(Die, 0.7f);
            this.gameObject.SetActive(false);
        }
        PlayerCharacter OtherPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (OtherPlayer)
        {
            if (IsDashing)
            {
                IsDashing = false;
                OtherPlayer.Push(this.transform.forward * DashForce * 10.0f);

                AudioClip DwarfCollision = SoundManager.Singleton.DwarfCollision;
                SoundManager.CreateSound(DwarfCollision, 0.7f);
            }
        }
        Obstacle OBS = collision.gameObject.GetComponent<Obstacle>();
        if (OBS)
        {
            if (IsPushed)
            {
                StartCoroutine(Stun());
            }
            if (IsDashing)
            {
                StartCoroutine(Stun());
            }
        }
    }
    IEnumerator Stun()
    {
        AudioClip DwarfStunned = SoundManager.Singleton.DwarfStunned;
        SoundManager.CreateSound(DwarfStunned, 0.7f);

        Anim.SetInteger("AnimationState", 0);
        Player.PushBehaviour(TotalLock);
        MeshColor.material.color = new Color(1.0f, 1.0f, 0.2f);
        yield return new WaitForSeconds(2.0f);
        MeshColor.material.color = new Color(1.0f, 1.0f, 1.0f);
        Player.PushBehaviour(InGameBehaviour);
        IsPushed = false;
        IsDashing = false;


    }
    void Push(Vector3 PushForce)
    {
        IsPushed = true;
        CharacterRigid.AddForce(PushForce);
    }
    IEnumerator PushCD()
    {
        yield return new WaitForSeconds(0.2f);
        IsPushed = false;
    }
    internal void ResetChar()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Respawn();
        Player.PushBehaviour(LockedBehaviour);
    }


    #region BehaviourDelegates
    void MoveByStick(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {
            Vector3 SpeedVect = new Vector3(AxisVect.x, 0, AxisVect.y);
            CharacterRigid.AddForce(SpeedVect * Speed);

            Vector3 lookPosition = this.transform.position + SpeedVect;
            this.transform.LookAt(lookPosition);
            Anim.SetInteger("AnimationState", 1);

        }
        else
        {
            Anim.SetInteger("AnimationState", 0);
        }
    }
    void LookByStick(Vector2 AxisVect)
    {
        if (AxisVect.magnitude > 0)
        {
            Vector3 SpeedVect = new Vector3(AxisVect.x, 0, AxisVect.y);
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
        Respawn();
    }
    void Respawn()
    {
        transform.gameObject.SetActive(true);
        //transform.gameObject.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<BoxCollider>().enabled = true;

    }
    private void ButtonBDown(EButtonState buttonState)
    {
        //Dash
        if (!IsDashLocked)
        {
            Debug.Log("Dash!");
           if(this.gameObject.active) StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        IsDashing = true;
        IsDashLocked = true;
        MeshColor.material.color = new Color(0.8f, 0.2f, 0.2f);
        CharacterRigid.AddForce(this.transform.forward * DashForce, ForceMode.Impulse);
        //play sound
        AudioClip DashClip = SoundManager.Singleton.Dash;
        SoundManager.CreateSound(DashClip, 0.5f);

        yield return new WaitForSeconds(0.3f);

        MeshColor.material.color = new Color(0.2f, 0.2f, 0.8f);
        IsDashing = false;

        yield return new WaitForSeconds(3.0f);
        MeshColor.material.color = new Color(0.2f, 0.8f, 0.2f);
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
                      //CurrentAxe = CurrentChest.PickupAxe(this);
                    }
                }
            }
        }
        else
        {
            if (CurrentAxe.transform.parent == this.transform)
            {
                CurrentAxe.Throw();

                AudioClip AxeThrow = SoundManager.Singleton.AxeThrow;
                SoundManager.CreateSound(AxeThrow, 0.7f);

                CurrentAxe.transform.localPosition += this.transform.forward;
                CurrentAxe.transform.rotation = this.transform.rotation;
                CurrentAxe = null;

                CharacterRigid.AddForce(-this.transform.forward * DashForce * 1.5f, ForceMode.Impulse);
            }
            else
            {
                CurrentAxe.Pickup(this.transform);
                AudioClip Pickup = SoundManager.Singleton.Pickup;
                SoundManager.CreateSound(Pickup, 0.5f);
            }
        }
    }
    #endregion
}
