using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    List<PlayerCharacter> Characters;
    List<PlayerCharacter> AlivePlayers;
    public static GameplayManager Singleton;
    List<Vector3> SpawnPoints;
    // Use this for initialization
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            Characters = new List<PlayerCharacter>();
            AlivePlayers = new List<PlayerCharacter>();
            SpawnPoints = new List<Vector3>();
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
            Characters.Clear();
        }
    }

    void Start()
    {
        // searching for other objects moved to Start()
        Transform SpawnPointsContainer = transform.FindChild("SpawnPoints");
        for (int i = 0; i < SpawnPointsContainer.childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsContainer.GetChild(i).position);
        }

        StartCoroutine(StartCounting());

    }

    public void AddCharacter(PlayerCharacter CharacterInGameplay)
    {
        Characters.Add(CharacterInGameplay);
        CharacterInGameplay.OnPlayerDeath += Character_OnPlayerDeath;
    }

    private void Character_OnPlayerDeath(PlayerCharacter DeadPlayer)
    {
        AlivePlayers.Remove(DeadPlayer);
        if (AlivePlayers.Count < 2)
        {
            Endgame();
        }
    }

    void Endgame()
    {
        StartCoroutine(StartCounting());
    }
    public void StartGame()
    {
        for (int i = 0; i < AlivePlayers.Count; i++)
        {
            AlivePlayers[i].OnGameStarted();
        }
    }

    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            GuiController.Instance.MainMenu();
        }

        if (Input.GetButton("Submit"))
        {
            GuiController.Instance.VictoryScreen();
        }
    }

    public IEnumerator StartCounting()
    {
        yield return new WaitForSeconds(1.0f);
        ResetGame();
        Debug.Log("3");
        AudioClip three = SoundManager.Singleton.VoiceThree;
        SoundManager.CreateSound(three, 0.7f);

        yield return new WaitForSeconds(1.0f);

        AudioClip VoiceTwo = SoundManager.Singleton.VoiceTwo;
        SoundManager.CreateSound(VoiceTwo, 0.7f);
        Debug.Log("2");
        yield return new WaitForSeconds(1.0f);

        AudioClip One = SoundManager.Singleton.VoiceOne;
        SoundManager.CreateSound(One, 0.7f);
        Debug.Log("1");
        yield return new WaitForSeconds(1.0f);

        AudioClip VoiceGo = SoundManager.Singleton.VoiceGo;
        SoundManager.CreateSound(VoiceGo, 0.7f);
        Debug.Log("GO");
        yield return new WaitForSeconds(0.1f);
        StartGame();
    }

    private void ResetGame()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            AlivePlayers.Add(Characters[i]);
            Characters[i].transform.position = SpawnPoints[i];
            Characters[i].ResetChar();
        }
    }
}
