using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameObjectivesManager : MonoBehaviour
{
    public bool gameOver;
    public GameObject gameOverMenu;
    private bool scoresSent = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeScene;
    }
    // Use this for initialization
    private void Awake()
    {
        
    }
    void Start ()
    {
        gameOver = false;
    }
	
    void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        gameOver = false;
        //Debug.Log("Scene initialized");
    }
	// Update is called once per frame
	void Update ()
    {
        CheckGameOver();
    }

    private void LateUpdate()
    {
        TreasureObjectiveLost();
    }

    private void CheckGameOver()
    {
        if (gameOver)
        {
            GameOver();
        }
    }
        

    public void GameOver()
    {
        Time.timeScale = 0;
        //Debug.Log("Game Over - You Lost");
        SendScoresToServer();
        gameOverMenu.SetActive(true);
    }

    private void TreasureObjectiveLost()
    {
        //Debug.Log("Current Treasure: " + GameMainManager.Instance._totalRoomTreasure);
        if (GameMainManager.Instance._totalRoomTreasure <=0)
        {
            gameOver= true;
        }
    }

    private void SendScoresToServer()
    {
        if (!scoresSent)
        {
            Debug.Log("Sending Scores to server");
            string playername = ServerBehaviorManager.Instance.playerData.username;
            string playerhash = ServerBehaviorManager.Instance.playerData.playerHash;
            int playerGold = GameMainManager.Instance._treasureGold;
            float survivalTime = GameMainManager.Instance._levelTimePlayed;
            int kills = GameMainManager.Instance._minionsKilled;
            ServerBehaviorManager.Instance.SendScores(playername, playerhash, playerGold, kills, survivalTime);
            scoresSent = true;
        }
        
    }
    
}
