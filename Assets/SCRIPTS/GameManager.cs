using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class GameManager : MonoBehaviour
{
    // Use this for initialization
    public static GameManager Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
            PlayerControllers = transform.FindChild("PlayerControllers").GetComponentsInChildren<PlayerController>();
            for (int i = 0; i < PlayerControllers.Length; i++)
            {
                PlayerControllers[i].Initialize();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    PlayerController[] PlayerControllers;
    void Start()
    {

     
    }
    public PlayerController[] GetPlayerControllers()
    {
        return PlayerControllers;
    }
    public PlayerController GetFirstPlayer()
    {
        return PlayerControllers[0];
    }


}
