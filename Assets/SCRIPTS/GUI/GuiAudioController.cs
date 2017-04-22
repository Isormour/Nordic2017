using UnityEngine;
using System.Collections;

public class GuiAudioController : MonoBehaviour {

    public AudioClip start;
    public AudioClip button;
    // public AudioClip[] button_clicks;
    // static Random randomizer = new Random();	//one instance - i will reuse it all the time
    // GetComponent<AudioSource>().clip = button_clicks[Random.Range(0, swoops.Length)];

    public static GuiAudioController Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
	
	public void game_starting()	{
		GetComponent<AudioSource>().clip = start;
		GetComponent<AudioSource>().Play();
	}
	public void button_pressed()	{
		GetComponent<AudioSource>().clip = button;
        // GetComponents<AudioSource>()[1].loop = true;
        GetComponent<AudioSource>().Play();
	}
    
}
