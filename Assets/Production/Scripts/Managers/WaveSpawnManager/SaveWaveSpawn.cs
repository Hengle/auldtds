using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]

public class Wave
{
	public string name;
	public GameObject[] enemy;
	public int count;
	public float rate;
}*/
[System.Serializable]	


public class SaveWaveSpawn : MonoBehaviour {
	//Get it out
	public class Wave
	{
		public string name;
		public GameObject[] enemy;
		public int count;
		public float rate;
	}

	public enum SpawnState {Spawning, Waiting, Counting}
	public enum SpawnMode {KillThemAll, OnlyTime, Both}

	[Header("Wave General Variables")]
	public Transform[] spawnPoints;
	private SpawnState state = SpawnState.Counting;
	private float searchCountdown = 1.0f;
	public SpawnMode mode = SpawnMode.OnlyTime;
	public float initialWaitingTime = 5.0f;

	[Header("Spawning Next Wave By Killing All Enemies")]
	[SerializeField]
	private Wave[] waves;
	private int nextWave = 0;
	public float timeBetweenWaves = 5.0f;
	private float waveCountdown;

	[Header("Spawning Next Wave By Time")]
	public float timeOfTheWave = 10.0f;
	private float timeOfTheWaveCountdown;
	[SerializeField]
	public int byTimeWaveCounting;
	public Wave wavebyTime;


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
		//Debug.Log("Spawning Wave:" + _wave.name);
		state = SpawnState.Spawning;

		for (int i =0; i < _wave.count; i++)
		{
			for(int j =0; j < _wave.enemy.Length; j++)
			{
				SpawnEnemy(_wave.enemy[j]);
				yield return new WaitForSeconds (1.0f/_wave.rate);  //_wave.delay or something else
			}
		}

		state = SpawnState.Waiting;
		yield break;
	}

	void SpawnEnemy(GameObject _enemy)
	{
		//Debug.Log("Spawning Enemy:" + _enemy.name);
		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}



	private void WaveCompleted()
	{
		if (mode == SpawnMode.KillThemAll)
		{
			//Debug.Log("Wave Completeted!");
			state = SpawnState.Counting;
			waveCountdown = timeBetweenWaves;	//For SpawningNextWaveByKillingAllEnemies

			if(nextWave + 1 > waves.Length - 1)
			{
				nextWave = 0;
				//Debug.Log("All Waves Complete!!! Looping...");
			}
			else
			{
				nextWave++;
			}
		}
		else if (mode == SpawnMode.OnlyTime)
		{
			//Debug.Log("Wave Completeted!");
			state = SpawnState.Counting;
			timeOfTheWaveCountdown = timeOfTheWave;
		}
	}

	private void ChooseSpawningMode()
	{
		if (mode == SpawnMode.KillThemAll)
		{
			SpawningNextWaveByKillingAllEnemies();
		}
		else if (mode == SpawnMode.OnlyTime)
		{
			SpawningNextWaveByTiming();
		}
	}


	#endregion

	#region Spawning Next Wave By Killing All Enemies Functions
	private void SpawningNextWaveByKillingAllEnemies()
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
	}

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
				byTimeWaveCounting ++;
				StartCoroutine(SpawnWave(wavebyTime));
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
