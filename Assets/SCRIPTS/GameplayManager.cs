using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public Screenshake Shaker;
    List<PlayerCharacter> Characters;
    List<GameObject> Dummies;
    List<PlayerCharacter> AlivePlayers;
    public static GameplayManager Sceneton;
    List<Vector3> SpawnPoints;
    public GameObject DummyPrefab;
    Dictionary<PlayerCharacter, int> Score;
    PlayerCharacter Streaker;
    int killStreak = 0;
    public GameObject fireworksPrefab;
    public GameObject Leaderboard;

    public List<PlayerCharacter> GetCharacters()
    {
        return Characters;
    }
    // Use this for initialization
    private void Awake()
    {
        Sceneton = this;
        Characters = new List<PlayerCharacter>();
        AlivePlayers = new List<PlayerCharacter>();
        Dummies = new List<GameObject>();
        SpawnPoints = new List<Vector3>();
        Score = new Dictionary<PlayerCharacter, int>();
    }
    private void OnDestroy()
    {
        Sceneton = null;
    }
    void Start()
    {
        // searching for other objects moved to Start()
        Transform SpawnPointsContainer = transform.FindChild("SpawnPoints");
        for (int i = 0; i < SpawnPointsContainer.childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsContainer.GetChild(i).position);
        }
        PlayerController[] Players =  GameManager.Singleton.GetPlayerControllers();
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].InitializeChar();
        }
        StartCoroutine(StartCounting());
    }

    public void AddCharacter(PlayerCharacter CharacterInGameplay)
    {
        Characters.Add(CharacterInGameplay);
        CharacterInGameplay.OnPlayerDeath += Character_OnPlayerDeath;
        if (!Score.ContainsKey(CharacterInGameplay))
        {
            Score.Add(CharacterInGameplay, 0);
        }
    }

    public Dictionary<PlayerCharacter, int> GetScore()
    {
        return Score;
    }

    private void Character_OnPlayerDeath(PlayerCharacter DeadPlayer, Axe Killer)
    {
        Shaker.Shake(3.0f);

        if (Streaker == null)
        {
            Streaker = Killer.getThrower();
            killStreak++;
        }
        else if (Streaker == Killer.getThrower())
        {
            killStreak++;
        }
        else if (Streaker != Killer.getThrower())
        {
            killStreak = 1;
        }
        if (killStreak > 1) {
            if (killStreak < 3)
            {
                // double
                AudioClip VoiceDoubleKill = SoundManager.Singleton.VoiceDoubleKill;
                SoundManager.CreateSound(VoiceDoubleKill, 0.7f);

            }
            else if (killStreak < 4)
            {
                AudioClip voiceTrippleKill = SoundManager.Singleton.voiceTrippleKill;
                SoundManager.CreateSound(voiceTrippleKill, 0.7f);
                // triple
            }
            else if (killStreak < 5)
            {
                AudioClip VoiceMultiKill = SoundManager.Singleton.VoiceMultiKill;
                SoundManager.CreateSound(VoiceMultiKill, 0.7f);
                // fukuple
            }
        }

        if (Killer)
        {
            Score[Killer.getThrower()] += 1;
        }
        else
        {
            if (DeadPlayer.transform.position.y < -3.0f)
            {
                Score[DeadPlayer] -= 1;
            }
        }

        AlivePlayers.Remove(DeadPlayer);
        if (AlivePlayers.Count < 2)
        {
            Endgame();
        }
        GameObject Dummy = Instantiate(DummyPrefab);
        Dummy.transform.position = DeadPlayer.transform.position;
        Rigidbody[] RBs = Dummy.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < RBs.Length; i++)
        {
            if (Killer)
            {
                Vector3 ForceVect = Killer.transform.forward * 10;
                ForceVect += new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
                RBs[i].AddForce(ForceVect, ForceMode.Impulse);
            }
        }
        if (Dummy.transform.position.y < -40)
        {
            Dummy.transform.gameObject.AddComponent<TimedDestroyer>().SetTime(3);
        }
    }

    void Endgame()
    {
        int PointsToEndGame = 5;
        bool HaveSomeonePassed5Points = false;
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Score[Characters[i]] > PointsToEndGame)
            {
                HaveSomeonePassed5Points = true;
            }
        }
        if (!HaveSomeonePassed5Points)
        {
            StartCoroutine(StartCounting());
        }
        else
        {
            Debug.LogError("Show Stats not implemented");
            Leaderboard.GetComponent<LeaderboardManager>().ShowLeaderBoards();
            for (int i = 0; i < Characters.Count; i++)
            {
               // Debug.Log("char no. " + i + " score " + Score[Characters[i]]);
            }
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].OnGameEnded();
            }
        }

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
            //GuiController.Instance.MainMenu();
        }

        if (Input.GetButton("Submit"))
        {
            //GuiController.Instance.VictoryScreen();
        }
    }

    public IEnumerator StartCounting()
    {
        yield return new WaitForSeconds(1.0f);
        ResetGame();
        yield return new WaitForSeconds(2.0f);
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
        AlivePlayers.Clear();
        for (int i = 0; i < Characters.Count; i++)
        {
            AlivePlayers.Add(Characters[i]);
            Characters[i].transform.position = SpawnPoints[i];
            Characters[i].ResetChar();
        }
    }
}
