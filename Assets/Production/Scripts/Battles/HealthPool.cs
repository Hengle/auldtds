using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enemy;
using PlayerUnit;

public class HealthPool : MonoBehaviour
{

    public GameObject parentObject;
    public float parentHealth;
    private MinionAttributes minionAttributes;
    private UnitAttributes unitAttributes;

    // Use this for initialization
    void Start ()
    {
        InitHealthBar();        
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetHealthBar();
    }

    private void InitHealthBar()
    {
        if (parentObject.gameObject.tag == "Minion")
        {
			parentHealth = parentObject.GetComponent<Enemy.StateController>().enemyStats.currentHealth;
        }
        else if (parentObject.gameObject.tag == "RTSUnit")
        {
            //Debug.Log("Unit Detected " + this.name);
			parentHealth = parentObject.GetComponent<PlayerUnit.StateController>().playerUnitStats.currentHealth;
        }
    }

    private void SetHealthBar()
    {
        if (parentObject.gameObject.tag == "Minion")
        {
			parentHealth = parentObject.GetComponent<Enemy.StateController>().enemyStats.currentHealth;
			float healthCalculation = parentHealth / parentObject.GetComponent<Enemy.StateController>().enemyStats.totalHealth;
            this.GetComponent<Image>().fillAmount = healthCalculation;
        }
        else if (parentObject.gameObject.tag == "RTSUnit")
        {
			parentHealth = parentObject.GetComponent<PlayerUnit.StateController>().playerUnitStats.currentHealth;
			float healthCalculation = parentHealth / parentObject.GetComponent<PlayerUnit.StateController>().playerUnitStats.totalHealth;
			this.GetComponent<Image>().fillAmount = healthCalculation;
        }
    }
}
