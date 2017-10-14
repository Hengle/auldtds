using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitSpawn : MonoBehaviour
{

    public GameObject[] heroUnits;

    public Transform spawnPoint;

    private UnitSelectManager unitSelectManager;

	// Use this for initialization
	void Start ()
    {
        unitSelectManager = this.GetComponent<UnitSelectManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SpawnHeroUnit(GameObject myPrefab)
    {
        var newUnit = GameObject.Instantiate(myPrefab,spawnPoint.transform.position, Quaternion.identity);
        newUnit.transform.parent = GameObject.Find("HeroUnits").transform;
        unitSelectManager.userControledUnits++;
    }
}
