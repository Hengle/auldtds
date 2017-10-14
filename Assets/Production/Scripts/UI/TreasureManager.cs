using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureManager : MonoBehaviour
{
    [SerializeField]
    private int goldTreasure;
    [SerializeField]
    private Text goldTreasureText;

    [SerializeField]
    private float gameTreasure;
    [SerializeField]
    private Text gameTreasureText;

    [SerializeField]
    private float levelTimePlayed;
    [SerializeField]
    private Text levelTimePlayedText;

    [SerializeField]
    private int mithrilTreasure;
    [SerializeField]
    private Text mithrilTreasureText;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetGold();
        GetTreasure();
        GetLevelTimePlayed();
    }

    private void GetGold()
    {
        goldTreasure = GameMainManager.Instance._treasureGold;
        goldTreasureText.text = goldTreasure.ToString();
    }

    private void GetMithril()
    {
        mithrilTreasure = GameMainManager.Instance._treasureMithril;
        mithrilTreasureText.text = mithrilTreasure.ToString();
    }

    private void GetTreasure()
    {
        gameTreasure = GameMainManager.Instance._totalRoomTreasure;
        gameTreasureText.text = gameTreasure.ToString();
    }



    private void GetLevelTimePlayed()
    {
        levelTimePlayed = GameMainManager.Instance._levelTimePlayed;
        int minutes = Mathf.FloorToInt(levelTimePlayed / 60F);
        int seconds = Mathf.FloorToInt(levelTimePlayed - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        levelTimePlayedText.text = niceTime;
    }
}
