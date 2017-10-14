using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RestartGame()
    {
        //RoomManager RM = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        //RM.CalculateTotalRoomTreasure();    
        GameObjectivesManager GOM = GameObject.Find("GameObjectivesManager").GetComponent<GameObjectivesManager>();
        GOM.gameOver = false;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
