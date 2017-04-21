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
    // Use this for initialization
    void Start()
    {

    }
    public void Init()
    {
        Collider = GetComponent<BoxCollider>();
        Renderer = GetComponent<MeshRenderer>();
        Collider.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Throwed)
        {
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
        Init();
        transform.SetParent(DwarfTransform);
        transform.localPosition = new Vector3(0, 0, 0.5f);
        Renderer.enabled = true;
    }
    public void Throw()
    {
        Collider.enabled = true;
        Throwed = true;
       // Debug.LogError("Implement Throw physics");
        transform.SetParent(null);
        StartCoroutine(TimetoDestroyCorr());
    }
    IEnumerator TimetoDestroyCorr()
    {
        yield return new WaitForSeconds(5.0f);
        if (onAxeDestroyed != null)
        {
            onAxeDestroyed();
        }
    }
}
