using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCountMinions : MonoBehaviour
{
    public string minionString;
    public int minionCount;
    public bool isAwarded = false;

    private GameObject achievementManager;
    //private GameObject gameManager;
    private GM_MobCounter mobCounter;
    private AchievementManager amScript;

    private int countIterator;

    private void Start()
    {
        achievementManager = GameObject.Find("AchievementManager");
        amScript = achievementManager.GetComponent<AchievementManager>();
        countIterator = 0;
        if (GameMainManager.Instance.minionKillTags.Count >=1)
        {
            foreach (string minionTag in GameMainManager.Instance.minionKillTags)
            {
                if (minionString == minionTag)
                {
                    break;
                }
                else
                {
                    countIterator++;
                }
            }
        }
    }

    private void Update()
    {
        //Debug.Log(countIterator);
        if (GameMainManager.Instance.minionKillTags.Count >= 1)
        {
            if (GameMainManager.Instance.minionKillCounter[countIterator] >= minionCount)
            {
                if (!isAwarded)
                {
                    Debug.Log("award Achievement " + gameObject.name);
                    ShowAchievement();
                }
            }
        }
    }

    private void OnDestroy()
    {

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
