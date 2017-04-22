using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static void CreateSound(AudioClip Clip, float Volume, bool Loop=false)
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
