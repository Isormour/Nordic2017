using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeManager : MonoBehaviour
{
    public GameObject AxePrefab;
    Chest[] Chests;
    // Use this for initialization
    void Start()
    {
        Axe.onAxeDestroyed += Axe_onAxeDestroyed;
        Chests = GetComponentsInChildren<Chest>();

        for (int i = 0; i < Chests.Length; i++)
        {
            Chests[i].OnAxePickedUp += AxeManager_OnAxePickedUp;
            Chests[i].Init();
            Chests[i].SetAxePrefab(AxePrefab);
        }

        RandomChestWithAxe();
    }

    private void Axe_onAxeDestroyed()
    {
        RandomChestWithAxe();
    }

    private void AxeManager_OnAxePickedUp(Axe axe)
    {
        for (int i = 0; i < Chests.Length; i++)
        {
            Chests[i].CloseChest();
        }
    }

    private void CurrentAxe_OnAxeDropped()
    {
        //CurrentAxe.ReturnToChest();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void RandomChestWithAxe()
    {
        int random = Random.Range(0, Chests.Length);
        for (int i = 0; i < Chests.Length; i++)
        {
            Chests[i].SetHaveAxe(false);
            Chests[i].CloseChest();

        }
        Chests[random].SetHaveAxe(true);

    }
}
