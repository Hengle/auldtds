using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCountSurvival : MonoBehaviour
{
    
    public float timeToSurvive;
    public bool isAwarded = false;

    private GameObject achievementManager;
    private AchievementManager amScript;

    private int countIterator;

    private void Start()
    {
        achievementManager = GameObject.Find("AchievementManager");
        amScript = achievementManager.GetComponent<AchievementManager>();
    }

    private void Update()
    {
        if (GameMainManager.Instance._levelTimePlayed >= (timeToSurvive * 60))
        {
            if (!isAwarded)
            {
                ShowAchievement();
            }
        }       
    }

    private void ShowAchievement()
    {
        GameObject newAchievementVisual = (GameObject)Instantiate(amScript.visualAchievement);
        newAchievementVisual.transform.SetParent(amScript.canvasUI.transform, false);
        newAchievementVisual.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.name;
        newAchievementVisual.transform.localScale = new Vector3(1, 1, 1);
        isAwarded = true;
        AwardAchievementToServer();
    }

    private void AwardAchievementToServer()
    {
        Debug.Log("Updating Server");
        ServerTalk ST = amScript.serverTalkComponent.GetComponent<ServerTalk>();
        StartCoroutine(ST.UpdatePlayerAchievement(this.name, ST.playerHash));
    }
}
