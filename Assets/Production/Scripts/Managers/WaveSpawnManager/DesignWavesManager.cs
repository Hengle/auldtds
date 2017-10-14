using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignWavesManager : MonoBehaviour {
#region Classes
	#region Weight Factor Classes
	[System.Serializable]
	public class EnemiesWeightFactor
	{
		public GameObject Enemy;
		public int EnemyWeightFactor;
	}

	[System.Serializable]
	public class SpawnCountWeightFactor
	{
		public int countWave;
		public int countWaveWeightFactor;
	}
#endregion
#endregion

	[Header("Random")]
	private int roll;
	private GameObject randomEnemy;
	private int randomEnemyWeightFactor;
	private int randomEnemyCount;
	private int randomEnemyCountWeightFactor;

	[Header("General")]
	[SerializeField]
	private Transform[] spawnPoints;
	[SerializeField]
	private SpawnMode spawnMode = SpawnMode.OnlyTime;
	[SerializeField]
	private float initialWaitingTime;
	[SerializeField]
	private float timeOfTheWave;
    [SerializeField]
    private float rateBetweenMinions = 2;
    private bool newWave;
	private bool newWaveEnemies;
	private int nextWave;

	[Header("Lists of Enemies Mob & Enemies Count Factors")]
	[SerializeField]
	private List<EnemiesWeightFactor> enemiesWeightFactorList;
	[SerializeField]
	private List<SpawnCountWeightFactor> spawnCountWeightFactorList;

	[Header("Total Weight Factor")]
	[SerializeField]
	private int initialWaveWeightFactor;
	private int waveWeightFactor;
	[SerializeField]
	private int waveWeightFactorIncrease;
	[SerializeField]
	private int waveWeightFactorLevelsToIncrease;
	private int calcTotalWeightFactor;
	private int addToTotalWeightFactor;

	[Header("Wave Setting")]
	//[SerializeField]
	private List<WaveEnemies> waveEnemiesList = new List<WaveEnemies>();
	//[SerializeField]
	private List<Wave> waveList = new List<Wave>();

	[Header("WaveSpawnManager")]
	private GameObject waveSpawnManager;
	private WaveSpawnManager waveSpawnManagerScr;

#region System Functions
	// Use this for initialization
	void Awake()
	{
		GetWaveSpawnManager();
		SetSpawnPoints();
		SetInitialWaitingTime();
		SetSpawnMode();
		SetTimeOfTheWave();
		waveWeightFactor = initialWaveWeightFactor;
	}

	void Start () 
	{
		newWaveEnemies = true;
		newWave = false;
		nextWave = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ConstructWave();
	}

	void GetWaveSpawnManager()
	{
		waveSpawnManager = GameObject.Find("WaveSpawnManager");
		waveSpawnManagerScr = waveSpawnManager.GetComponent<WaveSpawnManager>();
	}
#endregion

#region Wave Constructions Functions
	private void Roll()
	{
		if (newWaveEnemies == true)
		{
			WaveEnemies waveEnemiesClass = new WaveEnemies();

			if (calcTotalWeightFactor < waveWeightFactor)
			{
				roll = Random.Range(0, enemiesWeightFactorList.Count);
				randomEnemy = enemiesWeightFactorList[roll].Enemy;
				randomEnemyWeightFactor = enemiesWeightFactorList[roll].EnemyWeightFactor;

				roll = Random.Range(0, spawnCountWeightFactorList.Count);
				randomEnemyCount = spawnCountWeightFactorList[roll].countWave;
				randomEnemyCountWeightFactor = spawnCountWeightFactorList[roll].countWaveWeightFactor;

				addToTotalWeightFactor = randomEnemyWeightFactor * randomEnemyCountWeightFactor;

				if ((calcTotalWeightFactor + addToTotalWeightFactor) <= waveWeightFactor)
				{
					waveEnemiesClass.Enemy = randomEnemy;
					waveEnemiesClass.countOfEnemies = randomEnemyCount;

					waveEnemiesList.Add(waveEnemiesClass);

					calcTotalWeightFactor += addToTotalWeightFactor;
				}
			}
			else if (calcTotalWeightFactor == waveWeightFactor)
			{
				newWaveEnemies = false;
				newWave = true;
			}
		}
	}

	private void SettingWaveList()
	{
		if(newWave == true)
		{
			//Debug.Log("newWave = true");
			Wave waveClass = new Wave();
			waveClass.name = "Wave:" + nextWave;
			waveClass.rate = rateBetweenMinions;
			waveClass.waveEnemy = waveEnemiesList;
			waveList.Add(waveClass);
			newWave = false;
		}
		else
		{
			if (nextWave == waveSpawnManagerScr.waveCount)
			{
				waveEnemiesList.Clear();
				calcTotalWeightFactor = 0;
				newWaveEnemies = true;
				WaveWeightFactorIncrease();
				//waveWeightFactor = waveWeightFactor + waveWeightFactorIncrease;
				nextWave++;
			}
		}
	}

	private void SendWaveDataToWaveSpawnManager()
	{
		waveSpawnManagerScr.waveList = waveList;
	}

	private void WaveWeightFactorIncrease()
	{
		if((nextWave+1) % waveWeightFactorLevelsToIncrease == 0)
		{
			waveWeightFactor += waveWeightFactorIncrease;
		}
	}

	private void ConstructWave()
	{
		Roll();
		SettingWaveList();
		SendWaveDataToWaveSpawnManager();
	}
		
#endregion
#region SpawnPoints Functions
	private void SetSpawnPoints()
	{
		waveSpawnManagerScr.spawnPoints = spawnPoints;	
	}
#endregion
#region Set General Functions
	private void SetSpawnMode()
	{
		waveSpawnManagerScr.mode = spawnMode;
	}
	private void SetTimeOfTheWave()
	{
		waveSpawnManagerScr.timeOfTheWave = timeOfTheWave;
	}
	private void SetInitialWaitingTime()
	{
		waveSpawnManagerScr.initialWaitingTime = initialWaitingTime;
	}
#endregion
}
