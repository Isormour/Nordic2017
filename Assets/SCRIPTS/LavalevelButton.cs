using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavalevelButton : MonoBehaviour
{
    public GameObject Trap;
    List<PlayerCharacter> CharsOnTrap;
    bool IsPressed = false;
    Vector3 StartVect;
    // Use this for initialization
    void Start()
    {
        IsPressed = false;
        CharsOnTrap = new List<PlayerCharacter>();
        StartVect = Trap.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPressed)
        {
            Trap.transform.position = Vector3.Lerp(Trap.transform.position, StartVect + new Vector3(0, -4, 0), 0.2f);
        }
        else
        {
            Trap.transform.position = Vector3.Lerp(Trap.transform.position, StartVect, 0.2f);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter Char = other.GetComponent<PlayerCharacter>();
        if (Char)
        {
            CharsOnTrap.Add(Char);
            if (!IsPressed)
            {
                IsPressed = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            PlayerCharacter Char = other.GetComponent<PlayerCharacter>();
            if (Char)
            {
                if (CharsOnTrap.Contains(Char))
                {
                    CharsOnTrap.Remove(Char);
                    if (CharsOnTrap.Count < 1)
                    {
                        IsPressed = false;
                    }
                }
            }
        }
    }
}
