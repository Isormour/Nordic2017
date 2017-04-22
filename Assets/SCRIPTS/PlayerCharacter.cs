using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public delegate void PlayerDeath(PlayerCharacter DeadPlayer, Axe KillerAxe);
    public event PlayerDeath OnPlayerDeath;

    List<Obstacle> Obstacles;

    PlayerController Player;
    DSPad.DSPadBehaviour InGameBehaviour;
    DSPad.DSPadBehaviour LockedBehaviour;
    DSPad.DSPadBehaviour TotalLock;
    DSPad.DSPadBehaviour EndGameBehaviour;

    float Speed = 130.1f;
    float DashForce = 40.0f;
    Rigidbody CharacterRigid;

    Axe CurrentAxe;
    bool IsDashLocked;
    bool IsDashing;
    bool IsPushed;
    Chest CurrentChest;

    MeshRenderer MeshColor;
    ParticleSystem CollisionParticle;
    GameObject StunIndicator;
    Animator Anim;

    Coroutine StunCorr;
    float InitialY = 10;
    // Use this for initialization
    void Start()
    {
        Obstacles = new List<Obstacle>();
        CurrentAxe = null;
        CharacterRigid = GetComponent<Rigidbody>();
        IsDashLocked = false;
        IsDashing = false;
        IsPushed = false;
        GameplayManager.Sceneton.AddCharacter(this);
        transform.gameObject.SetActive(false);
        Anim = GetComponentInChildren<Animator>();
        MeshColor = GetComponent<MeshRenderer>();

        StunIndicator = transform.FindChild("StunIndicator").gameObject;
        StunIndicator.SetActive(false);
        CollisionParticle = transform.FindChild("iskra_particle").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        chceckY();
    }
    void chceckY()
    {
        if (InitialY < 9) {
            if (InitialY - this.transform.position.y > 0.3f)
            {
                Player.PushBehaviour(LockedBehaviour);
                GetComponent<Rigidbody>().drag = 1;
                this.transform.position += new Vector3(0, -0.1f, 0.0f);
                if (this.transform.position.y < -3)
                {
                    transform.GetComponent<BoxCollider>().enabled = false;
                    //StartCoroutine(RespCorr());
                    if (OnPlayerDeath != null)
                    {
                        OnPlayerDeath(this, null);
                    }
                    AudioClip Die = SoundManager.Singleton.Die;
                    SoundManager.CreateSound(Die, 0.7f);
                    this.gameObject.SetActive(false);
                }
            }
        }

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

        EndGameBehaviour = new DSPad.DSPadBehaviour();
        EndGameBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.A, EButtonState.down, EndGameButton);
        EndGameBehaviour.AddButtonBehaviour(GamepadInput.GamePad.Button.B, EButtonState.down, EndGameButton);

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
        InitialY = this.transform.position.y;
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
                OtherPlayer.Push(this.transform.forward * DashForce * 20.0f);

                AudioClip DwarfCollision = SoundManager.Singleton.DwarfCollision;
                SoundManager.CreateSound(DwarfCollision, 0.7f);
                CollisionParticle.Play();
                if (Obstacles.Count > 0)
                {
                    TryStun();
                }
            }
        }
        Obstacle OBS = collision.gameObject.GetComponent<Obstacle>();
        if (OBS)
        {
            Obstacles.Add(OBS);
            if (IsPushed)
            {
                TryStun();
            }
            if (IsDashing)
            {
                //StartCoroutine(Stun());
            }
        }
    }
    void TryStun()
    {
        if (StunCorr != null)
        {

        }
        else
        {
            StunCorr = StartCoroutine(Stun());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Obstacle OBS = collision.gameObject.GetComponent<Obstacle>();
        if (OBS)
        {
            Obstacles.Remove(OBS);
        }
    }
    IEnumerator Stun()
    {
        AudioClip DwarfStunned = SoundManager.Singleton.DwarfStunned;
        SoundManager.CreateSound(DwarfStunned, 0.7f);

        Anim.SetInteger("AnimationState", 0);
        Player.PushBehaviour(TotalLock);
        MeshColor.material.color = new Color(1.0f, 1.0f, 0.2f);
        StunIndicator.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        MeshColor.material.color = new Color(1.0f, 1.0f, 1.0f);
        Player.PushBehaviour(InGameBehaviour);
        StunIndicator.SetActive(false);
        IsPushed = false;
        IsDashing = false;
        StunCorr = null;

    }
    public void OnGameEnded()
    {
        Player.PushBehaviour(TotalLock);
        transform.gameObject.SetActive(true);
        transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), -100, 0);
        StartCoroutine(WaitforScores());
    }
    IEnumerator WaitforScores()
    {
        yield return new WaitForSeconds(3);

        Player.PushBehaviour(EndGameBehaviour);
    }
    void Push(Vector3 PushForce)
    {
        IsPushed = true;
        CharacterRigid.AddForce(PushForce);
    }
    IEnumerator PushCD()
    {
        yield return new WaitForSeconds(0.4f);
        IsPushed = false;
    }
    internal void ResetChar()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Respawn();
        Player.PushBehaviour(LockedBehaviour);
        GetComponent<Rigidbody>().drag = 10.39f;
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
    private void EndGameButton(EButtonState buttonState)
    {
        Debug.LogError("Go To Menu");
    }
    private void ButtonBDown(EButtonState buttonState)
    {
        //Dash
        if (!IsDashLocked)
        {
            Debug.Log("Dash!");
            // if (this.gameObject.active)
            {
                StartCoroutine(Dash());
            }
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
                CurrentAxe.Throw(this);

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
