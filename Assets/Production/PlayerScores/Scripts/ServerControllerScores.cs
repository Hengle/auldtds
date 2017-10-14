using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerControllerScores : MonoBehaviour
{

    public string getScoresURL;
    public ScoresList ScoresList = new ScoresList();

    [SerializeField]
    private GameObject scoresCanvas;
    [SerializeField]
    private GameObject rankTextPrefab;
    [SerializeField]
    private GameObject playerTextPrefab;
    [SerializeField]
    private GameObject playerScorePrefab;

    private void Start()
    {
        //GetPlayerScores();
    }

    public void GetPlayerScores()
    {
        //Debug.Log("Getting Scores from Server");
        scoresCanvas.SetActive(true);
        StartCoroutine(_GetPlayerScores());
    }
    string fixJson(string value)
    {
        value = "{\"Scores\":" + value + "}";
        return value;
    }

    IEnumerator _GetPlayerScores()
    {
        WWWForm scoresForm = new WWWForm();
        scoresForm.AddField("action", "getallscores");
        UnityWebRequest www = UnityWebRequest.Post(getScoresURL, scoresForm);

        yield return www.Send();
        Debug.Log("Server Responded");
        if (www.error !=null)
        {
            Debug.Log("ERROR");
            string errorReportingMessage = "Oops. Something went wrong. (error 0x000-Connection Error)";
            Debug.Log(errorReportingMessage);
        }
        else
        {
            string jsnData = www.downloadHandler.text;
            //Debug.Log(jsnData);
            if (jsnData == "0x000")
            {
                string errorReportingMessage = "Error Getting Data from Server - 0x000";
                Debug.Log(errorReportingMessage);
            }
            else
            {
                string fixedJsn = fixJson(jsnData);
                Debug.Log(fixedJsn);
                //PlayerScoresClass[] playerScores = JsonHelper.FromJson<PlayerScoresClass>(jsnData);
                ScoresList = JsonUtility.FromJson<ScoresList>(fixedJsn);
                int rankCounter = 1;
                foreach (Scores score in ScoresList.Scores)
                {
                    //print(score.username);
                    InstantiateScore(score.username,rankCounter, score.survivaltime);
                    rankCounter++;
                }
            }

        }
    }

    private string SurvivalScore (float _survivalTime)
    {
        int minutes = Mathf.FloorToInt(_survivalTime / 60F);
        int seconds = Mathf.FloorToInt(_survivalTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;
    }

    private void InstantiateScore(string _playername, int _rank, float _score)
    {
        GameObject scoreRank = Instantiate(rankTextPrefab) as GameObject;
        GameObject playerName = Instantiate(playerTextPrefab) as GameObject;
        GameObject playerScore = Instantiate(playerScorePrefab) as GameObject;
        scoreRank.transform.SetParent(GameObject.Find("ScoresList").transform, false);
        playerName.transform.SetParent(GameObject.Find("ScoresList").transform, false);
        playerScore.transform.SetParent(GameObject.Find("ScoresList").transform, false);
        scoreRank.GetComponent<Text>().text = _rank.ToString();
        scoreRank.transform.position = new Vector2(scoreRank.transform.position.x, scoreRank.transform.position.y-(40 *_rank));
        playerName.GetComponent<Text>().text = _playername;
        playerName.transform.position = new Vector2(playerName.transform.position.x, playerName.transform.position.y - (40 * _rank));
        playerScore.GetComponent<Text>().text = SurvivalScore(_score);
        playerScore.transform.position = new Vector2(playerScore.transform.position.x, playerScore.transform.position.y - (40 * _rank));
    }
}
