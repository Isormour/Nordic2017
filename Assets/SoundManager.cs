using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static void CreateSound(AudioClip Clip, float Volume, bool Loop = false, bool getTimeFromClip = false)
    {
        GameObject GO = new GameObject();
        AudioSource AS = GO.AddComponent<AudioSource>();
        AS.clip = Clip;
        AS.volume = Volume;
        AS.Play();
        if (!Loop)
        {
            GO.AddComponent<TimedDestroyer>();
        }
        if (getTimeFromClip)
        {
            GO.GetComponent<TimedDestroyer>().SetTime(Clip.length);
        }
        AS.loop = Loop;
    }

    public static SoundManager Singleton;

    public AudioClip AxeHit;
    public AudioClip AxeGround;
    public AudioClip AxeThrow;
    public AudioClip ChestOpen;
    public AudioClip Dash;
    public AudioClip Die;
    public AudioClip Pickup;

    public AudioClip VoiceDoubleKill;
    public AudioClip VoiceMultiKill;
    public AudioClip VoiceGo;
    public AudioClip VoiceOne;
    public AudioClip VoiceTwo;
    public AudioClip VoiceThree;
    public AudioClip voiceTrippleKill;
    public AudioClip VoiceMassacre;

    public AudioClip DwarfCollision;
    public AudioClip DwarfStunned;

    public AudioClip Music1Intro;
    public AudioClip Music1Loop;
    public AudioClip Music2Intro;
    public AudioClip Music2Loop;
    public AudioClip Music3Intro;
    public AudioClip Music3Loop;

    public static void PlayMusic(AudioClip intro, AudioClip loop)
    {
        Singleton.StartCoroutine(StartMusicLoop(intro.length,intro,loop));
    }
    public static IEnumerator StartMusicLoop(float Delay, AudioClip intro, AudioClip loop, bool getTimeFromClip = false)
    {
        CreateSound(intro, 0.5f,false,true);
        yield return new WaitForSeconds(Delay);
        CreateSound(loop, 0.5f,true);
    }

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
