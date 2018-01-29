using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject achievementAvailablePrefab;

    [SerializeField]
    private Sprite achievementAwardedImage;
    [SerializeField]
    private Sprite achievementAvailableImage;
    [SerializeField]
    private Sprite achievementLostImage;
    [SerializeField]
    private AchievementDatabase achievementDB;
    [SerializeField]
    private GameObject achievementLogger;

    [SerializeField]
    public GameObject visualAchievement;
    [SerializeField]
    public string gameManagerMonitor;
    [SerializeField]
    public GameObject serverTalkComponent;

    public GameObject canvasUI;

    // Use this for initialization
    void Start ()
    {
        GameObject GM;
        GM = GameObject.Find(gameManagerMonitor).gameObject;
        BuildAchievementLogger();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    public void AwardAchievement(string category, GameObject achievementEntry)
    {

    }

    public void CreateAchievementList()
    {
        ServerTalk serverTalkScript = serverTalkComponent.GetComponent<ServerTalk>();
        foreach (Achievement achiev in achievementDB.database)
        {
            
            GameObject newAchievement = (GameObject)Instantiate(achievementAvailablePrefab);
            newAchievement.transform.SetParent(GameObject.Find("Achievement List Content").transform);
            newAchievement.name = achiev.achievementTitle;
            newAchievement.transform.localScale = new Vector3(1, 1, 1);
            SetAchievementInfo(achiev, newAchievement);
            
        }
    }

    private void SetAchievementInfo(Achievement achievement, GameObject achievementObject)
    {
        achievementObject.transform.GetChild(1).GetComponent<Text>().text = achievement.achievementTitle;
        achievementObject.transform.GetChild(2).GetComponent<Text>().text = achievement.achievementDescription;
        achievementObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = achievement.achievementScore.ToString();
        /*
        if (achievement.achieveType == achievementType.MINION_KILLS)
        {
            achievementObject.gameObject.AddComponent<AchievementCountMinions>();
        }
        if (achievement.achieveType == achievementType.TIME_SURVIVE)
        {
            achievementObject.gameObject.AddComponent<AchievementCountSurvival>();
        }
        */
    }

    private void BuildAchievementLogger()
    {
        foreach (Achievement achiev in achievementDB.database)
        {
            GameObject newAchievementLogger = (GameObject)Instantiate(achievementLogger);
            newAchievementLogger.transform.SetParent(GameObject.Find("AchievementManager").transform);
            newAchievementLogger.name = achiev.achievementTitle;
            
            SetAchievementLogger(achiev, newAchievementLogger);
        }
    }

    private void SetAchievementLogger(Achievement achiev, GameObject achievementLogger)
    {
        if (achiev.achieveType == achievementType.MINION_KILLS)
        {
            achievementLogger.gameObject.AddComponent<AchievementCountMinions>();
            AchievementCountMinions achCount = achievementLogger.GetComponent<AchievementCountMinions>();
            achCount.minionString = achiev.achieveMinionToKill;
            achCount.minionCount = achiev.achieveMinionToKillCount;
            
            
        }
        if (achiev.achieveType == achievementType.TIME_SURVIVE)
        {
            achievementLogger.gameObject.AddComponent<AchievementCountSurvival>();
            AchievementCountSurvival achCountTime = achievementLogger.GetComponent<AchievementCountSurvival>();
            achCountTime.timeToSurvive = achiev.achieveTimeSurvive;
        }
    }
}
