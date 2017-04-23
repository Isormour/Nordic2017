using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public enum EMusicToPlay
    {
        Colloseum,
        Olimp,
        Lava
    }
    public EMusicToPlay CurrentMusicToPlay;
    // Use this for initialization
    void Start()
    {
        AudioClip Intro;
        AudioClip Loop;

        switch (CurrentMusicToPlay)
        {
            case EMusicToPlay.Colloseum:
                Intro = SoundManager.Singleton.Music1Intro;
                Loop = SoundManager.Singleton.Music1Loop;
                SoundManager.PlayMusic(Intro, Loop);
                break;
            case EMusicToPlay.Olimp:
                Intro = SoundManager.Singleton.Music2Intro;
                Loop = SoundManager.Singleton.Music2Loop;
                SoundManager.PlayMusic(Intro, Loop);
                break;
            case EMusicToPlay.Lava:
                Intro = SoundManager.Singleton.Music3Intro;
                Loop = SoundManager.Singleton.Music3Loop;
                SoundManager.PlayMusic(Intro, Loop);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
