using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public ScoresList ScoresList = new ScoresList();

	void Start ()
    {
        TextAsset asset = Resources.Load("Scores") as TextAsset;
        if (asset !=null)
        {
            Debug.Log("Starting");
            ScoresList = JsonUtility.FromJson<ScoresList>(asset.text);
            Debug.Log(asset.text);
            foreach (Scores score in ScoresList.Scores)
            {
                //print(score.entryid);
                print(score.username);
                print(score.kills);
                //print(score.survivaltime);
            }
        }
        else
        {
            print("Asset is Null");
        }
	}

}
