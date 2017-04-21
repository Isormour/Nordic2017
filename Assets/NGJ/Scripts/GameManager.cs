using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class GameManager : MonoBehaviour
{

    // Use this for initialization
    PlayerController[] PlayerControllers;
    void Start()
    {
        PlayerControllers = transform.FindChild("PlayerControllers").GetComponentsInChildren<PlayerController>();
        for (int i = 0; i < PlayerControllers.Length; i++)
        {
            PlayerControllers.Initialize();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
