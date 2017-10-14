using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
	public class Wave
	{
		public string name;
		public List<WaveEnemies> waveEnemy;
		public float rate;
	}

	[System.Serializable]
	public class WaveEnemies
	{
		public GameObject Enemy;
		public int countOfEnemies;
	}

public enum SpawnMode {KillThemAll, OnlyTime, Both, Nothing}

public class WaveSpawnManager : MonoBehaviour {

	public enum SpawnState {Spawning, Waiting, Counting}

	[Header("Wave General Variables")]
	[HideInInspector]
	public float initialWaitingTime;
	[HideInInspector]
	public Transform[] spawnPoints;
	[HideInInspector]
	public SpawnState state = SpawnState.Counting;
	[HideInInspector]
	public SpawnMode mode;

	[Header("Spawning Next Wave By Killing All Enemies")]
	private Wave[] waves;
	private float timeBetweenWaves = 5.0f;
	private float waveCountdown;
	private float searchCountdown = 1.0f;

	[Header("Spawning Next Wave By Time")]
	private float timeOfTheWaveCountdown;
	[HideInInspector]
	public float timeOfTheWave;
	[HideInInspector]
	public List<Wave> waveList;
	[HideInInspector]
	public int waveCount = 0;


#region System Functions
	// Use this for initialization
	void Start () 
	{
		waveCountdown = initialWaitingTime;
		timeOfTheWaveCountdown = initialWaitingTime;
		CheckSpawnPoints();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ChooseSpawningMode();
	}
#endregion

#region Gereral Spawn Waves Functions
	IEnumerator SpawnWave(Wave _wave)
	{
		state = SpawnState.Spawning;
		for (int i =0; i < _wave.waveEnemy.Count; i++)
		{
			for(int j =0; j < _wave.waveEnemy[i].countOfEnemies; j++)
			{
				SpawnEnemy(_wave.waveEnemy[i].Enemy);
				yield return new WaitForSeconds (1.0f/_wave.rate);  //_wave.delay or something else
			}
		}
		//isWaveSet = false;	//testing
		state = SpawnState.Waiting;
		yield break;
	}

	void SpawnEnemy(GameObject _enemy)
	{
		//Debug.Log("Spawning Enemy:" + _enemy.name);
		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		GameObject myItem = Instantiate(_enemy, _sp.position, _sp.rotation);
		myItem.transform.SetParent(GameObject.Find("WaveMinions").transform);
	}
		
	private void WaveCompleted()
	{
		if (mode == SpawnMode.KillThemAll)
		{
			//Debug.Log("Wave Completeted!");
			state = SpawnState.Counting;
			waveCountdown = timeBetweenWaves;	//For SpawningNextWaveByKillingAllEnemies

			if(waveCount + 1 > waveList.Count - 1)
			{
				waveCount = 0;
				//Debug.Log("All Waves Complete!!! Looping...");
			}
			else
			{
				waveCount++;
			}
		}
		else if (mode == SpawnMode.OnlyTime)
		{
			//Debug.Log("Wave Completeted!");
			state = SpawnState.Counting;
			timeOfTheWaveCountdown = timeOfTheWave;

			waveCount++;
		}
	}

	private void ChooseSpawningMode()
	{
		if (mode == SpawnMode.KillThemAll)
		{
//			SpawningNextWaveByKillingAllEnemies();
		}
		else if (mode == SpawnMode.OnlyTime)
		{
			SpawningNextWaveByTiming();
		}
	}


#endregion

#region Spawning Next Wave By Killing All Enemies Functions
	/*private void SpawningNextWaveByKillingAllEnemies()
	{
		if(state == SpawnState.Waiting)
		{
			if(!EnemyisAlive())
			{
				WaveCompleted();
				return;
			}
			else   //Check if is to change waves with time
			{
				return;
			}
		}

		if(waveCountdown <= 0)
		{
			if(state != SpawnState.Spawning)
			{
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;	
		}
	}*/

	bool EnemyisAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0)
		{
			searchCountdown = 1f;
			if(GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}
#endregion

#region Spawning Next Wave By Timing Functions
	private void SpawningNextWaveByTiming()
	{
		if(state == SpawnState.Waiting)
		{
			if(timeOfTheWaveCountdown <= 0)
			{
				WaveCompleted();
				return;
			}
		}

		if(timeOfTheWaveCountdown <= 0)
		{
			if(state != SpawnState.Spawning)
			{
				StartCoroutine(SpawnWave(waveList[waveCount]));
			}
		}
		else
		{
			timeOfTheWaveCountdown -= Time.deltaTime;	
		}
	}
#endregion

#region SpawnPoints Functions
	private void CheckSpawnPoints()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No SpawnPoints assigned!!!");
		}
	}
#endregion
}
