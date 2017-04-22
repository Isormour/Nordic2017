using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    List<PlayerCharacter> Characters;
    List<GameObject> Dummies;
    List<PlayerCharacter> AlivePlayers;
    public static GameplayManager Sceneton;
    List<Vector3> SpawnPoints;
    public GameObject DummyPrefab;

    // Use this for initialization
    private void Awake()
    {
        Sceneton = this;
        Characters = new List<PlayerCharacter>();
        AlivePlayers = new List<PlayerCharacter>();
        Dummies = new List<GameObject>();
        SpawnPoints = new List<Vector3>();
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

        StartCoroutine(StartCounting());

    }

    public void AddCharacter(PlayerCharacter CharacterInGameplay)
    {
        Characters.Add(CharacterInGameplay);
        CharacterInGameplay.OnPlayerDeath += Character_OnPlayerDeath;
    }

    private void Character_OnPlayerDeath(PlayerCharacter DeadPlayer, Axe Killer)
    {

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
            Vector3 ForceVect = Killer.transform.forward * 10;
            ForceVect += new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
            RBs[i].AddForce(ForceVect, ForceMode.Impulse);
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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            //GuiController.Instance.MainMenu();
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
