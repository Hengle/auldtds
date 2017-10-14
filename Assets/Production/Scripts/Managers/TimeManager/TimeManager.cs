using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField]
    private float levelPlayedTime = 0.0f;
    [SerializeField]
    private float timeScale;

    // Use this for initialization
  
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetTimeScale();
        CountLevelPlayedTime();
    }

    private void CountLevelPlayedTime()
    {
        levelPlayedTime += Time.deltaTime;
        GameMainManager.Instance._levelTimePlayed = levelPlayedTime;
        //Debug.Log(niceTime);
    }

    private void SetTimeScale()
    {
        GameObjectivesManager GOM = GameObject.Find("GameObjectivesManager").GetComponent<GameObjectivesManager>();

        //if (GOM.gameOver == false)
        //{
            Time.timeScale = timeScale;
        //}
    }
}
