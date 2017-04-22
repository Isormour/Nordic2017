using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    bool Throwed = false;
    public delegate void Axedropped();
    public event Axedropped OnAxeDropped;

    public delegate void AxeDestroyed();
    public static event AxeDestroyed onAxeDestroyed;
    float Speed = 0.32f;
    BoxCollider Collider;
    MeshRenderer Renderer;
    Coroutine LifeTimeCorr;
    Transform Model;
    Transform owner;
    // Use this for initialization
    void Start()
    {

    }
    public void Init()
    {
        Collider = GetComponent<BoxCollider>();
        Renderer = transform.Find("Model").GetComponent<MeshRenderer>();
        Collider.enabled = false;
        Model = transform.FindChild("Model");
    }
    // Update is called once per frame
    void Update()
    {
        if (Throwed)
        {
            Model.transform.Rotate(new Vector3(0, 0, 15));
            this.transform.position += this.transform.forward * Speed;
        }
    }

    internal void ReturnToChest()
    {
        Collider.enabled = false;
        Renderer.enabled = false;
    }
    public void Pickup(Transform DwarfTransform)
    {

        if (!owner)
        {
            Init();
            transform.SetParent(DwarfTransform);
            transform.localPosition = new Vector3(0, 0, 1.5f);
            Renderer.enabled = true;
            owner = DwarfTransform;
            Model.transform.localRotation =Quaternion.Euler (new Vector3(-90, 0, 130));
        }
    }
    public void Throw()
    {
        Collider.enabled = true;
        Throwed = true;
        owner = null;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        //Debug.LogError("Implement Throw physics");
        transform.SetParent(null);
        LifeTimeCorr = StartCoroutine(TimetoDestroyCorr());
    }
    IEnumerator TimetoDestroyCorr()
    {
        yield return new WaitForSeconds(0.5f);
        Throwed = false;

        AudioClip AxeGround = SoundManager.Singleton.AxeGround;
        SoundManager.CreateSound(AxeGround, 0.5f);

        GetComponent<BoxCollider>().isTrigger = true;
        yield return new WaitForSeconds(3.0f);
        if (!owner)
        {
            DestroyAxe();
        }
        else
        {
            StopCoroutine(LifeTimeCorr);
        }

    }
    private void DestroyAxe()
    {
        StopCoroutine(LifeTimeCorr);
        if (onAxeDestroyed != null)
        {
            onAxeDestroyed();
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter dwarf = other.GetComponent<PlayerCharacter>();
        if (dwarf)
        {
            dwarf.OnEnterPickableAxe(this);
        }
        Obstacle OBS = other.gameObject.GetComponent<Obstacle>();
        if (OBS)
        {
            Throwed = false;
            AudioClip AxeHit = SoundManager.Singleton.AxeHit;
            SoundManager.CreateSound(AxeHit, 0.5f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerCharacter dwarf = other.GetComponent<PlayerCharacter>();
        if (dwarf)
        {
            dwarf.OnExitPickableAxe(this);
        }
    }
}
