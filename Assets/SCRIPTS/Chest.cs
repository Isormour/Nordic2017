using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    GameObject AxePrefab;
    public delegate void AxeFound();
    public event AxeFound OnAxeFound;
    public delegate void AxePickedUp(Axe axe);
    public event AxePickedUp OnAxePickedUp;
    bool HaveAxe;
    List<PlayerCharacter> PlayersInRange;
    public bool DebugMaterialChanger;
    bool IsOpen;
    Animator Anim;
    ParticleSystem Particle;
    internal void Init()
    {
        PlayersInRange = new List<PlayerCharacter>();
        IsOpen = false;
        Marker = transform.FindChild("Marker");
        if (Marker)
        {
            Marker.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Marker Should Exist");
        }
        Anim = GetComponentInChildren<Animator>();
        Particle = GetComponentInChildren<ParticleSystem>();
    }

    Transform Marker;
    // Use this for initialization
    void Start()
    {

    }
    public bool GetIsOpen()
    { return IsOpen; }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetAxePrefab(GameObject AxePrefab)
    {
        this.AxePrefab = AxePrefab;
    }
    public void SetHaveAxe(bool isInChest)
    {
        HaveAxe = isInChest;
        if (DebugMaterialChanger)
        {
            if (HaveAxe)
            {
                GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.6f, 0.2f);
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.9f);
            }
        }

    }
    public bool GetHaveAxe()
    {
        return HaveAxe;
    }

    public bool OpenChest(PlayerCharacter Player)
    {
        AudioClip ChestOpen = SoundManager.Singleton.ChestOpen;
        SoundManager.CreateSound(ChestOpen, 0.5f);

        IsOpen = true;
        //Marker.gameObject.SetActive(true);

        if (HaveAxe)
        {
            Marker.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.8f, 0.2f);

            GameObject AxeOB = Instantiate(AxePrefab) as GameObject;
            AxeOB.transform.position = this.transform.position + (this.transform.forward * 2);
            Particle.Play();
        }
        else
        {
            Marker.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.2f, 0.2f);
        }
        Anim.SetInteger("AnimationState", 1);
        return HaveAxe;
    }
    public void CloseChest()
    {
        IsOpen = false;
        //Marker.gameObject.SetActive(false);
        Anim.SetInteger("AnimationState", 0);
    }
    public Axe PickupAxe(PlayerCharacter PlayerChar)
    {
        Axe axe = null;

        if (OnAxePickedUp != null)
        {
            if (AxePrefab)
            {
                GameObject AxeOB = Instantiate(AxePrefab) as GameObject;
                axe = AxeOB.GetComponent<Axe>();
                axe.Pickup(PlayerChar.transform);
                OnAxePickedUp(axe);
            }
        }
        SetHaveAxe(false);

        return axe;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter Character = other.GetComponent<PlayerCharacter>();
        if (Character)
        {
            PlayersInRange.Add(Character);
            Character.OnEnterChestRange(this);
        }

    }
    void OnTriggerExit(Collider other)
    {
        PlayerCharacter Character = other.GetComponent<PlayerCharacter>();
        if (Character)
        {
            if (PlayersInRange.Contains(Character))
            {
                PlayersInRange.Remove(Character);
                Character.OnExitChestRange(this);
                if (PlayersInRange.Count == 0)
                {
                    CloseChest();
                }
            }
        }
    }


}
