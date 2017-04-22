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

            Transform SpawnPointsContainer = transform.FindChild("SpawnPoints");
            for (int i = 0; i < SpawnPointsContainer.childCount; i++)
            {
                SpawnPoints.Add(SpawnPointsContainer.GetChild(i).position);
            }
        }
        else
        {
            Destroy(this.gameObject);
            Characters.Clear();
        }
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

    void Start()
    {
        StartCoroutine(StartCounting());

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
    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator StartCounting()
    {
        yield return new WaitForSeconds(1.0f);
        ResetGame();
        Debug.Log("3");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("2");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("1");
        yield return new WaitForSeconds(1.0f);
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
