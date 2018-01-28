using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerTalk : MonoBehaviour
{

    [SerializeField]
    private string URL2GO;
    [SerializeField]
    private string URL2GO_AchievUpdate;
    [SerializeField]
    public string playerHash;

    [System.Serializable]
    public class PlayerAchievementsClass
    {
        public int achievID;
        public int achievStatus;
        public string playerHash;
        public string achievementName;
        public int achievProgressNum;
        public int achievProgressPer;
    }
    [System.Serializable]
    public class PlayerAchievementList
    {
        public List<PlayerAchievementsClass> playerAchievements;
    }


    public PlayerAchievementList achievementsList = new PlayerAchievementList();

    void Start ()
    {
        StartCoroutine(CheckPlayerAchievementStatus());
	}
    public IEnumerator CheckPlayerAchievementStatus()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerHash", playerHash);

        UnityWebRequest www = UnityWebRequest.Post(URL2GO, form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Something is broken...");
        }
        else
        {
            string downloadData = www.downloadHandler.text;
            achievementsList = JsonUtility.FromJson<PlayerAchievementList>(downloadData);
        }
    }

  
    public IEnumerator UpdatePlayerAchievement(string achievName, string plHash)
    {
        Debug.Log("Connecting to Server");
        WWWForm form = new WWWForm();
        form.AddField("playerHash", plHash);
        form.AddField("achievementName", achievName);

        UnityWebRequest www = UnityWebRequest.Post(URL2GO_AchievUpdate, form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Something is broken...");
        }
        else
        {
            Debug.Log("..Connected, waiting for responce:");
            string downloadData = www.downloadHandler.text;
        }
    }
}
