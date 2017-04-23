using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardManager : MonoBehaviour {
    public Vector3 InitialPosition;
    public Vector3 TargetPosition;
    public MeshRenderer[] Kafelki;
    public Text[] Texts;
    // Use this for initializatio
    bool move;
    void Start () {
        this.transform.position = InitialPosition;
        move = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, TargetPosition, 0.1f);
        }
    }
    public void ShowLeaderBoards()
    {
        move = true;
        List<PlayerCharacter> Chars =   GameplayManager.Sceneton.GetCharacters();
        Dictionary<PlayerCharacter, int> Score = GameplayManager.Sceneton.GetScore();
        

        for (int i = 0; i < Chars.Count; i++)
        {
            Texts[i+1].text = Score[Chars[i]].ToString();
            transform.GetChild(i+1).GetComponent<MeshRenderer>().material.color = Chars[i].BodyColor.material.color;
        }
        Texts[0].text = "Points";
    }
}
