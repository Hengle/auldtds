using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            //Debug.Log("Minion Detected " + this.name);
            minionAttributes = parentObject.GetComponent<MinionAttributes>();
            parentHealth = minionAttributes.minionAttributes.unitHealthPoints;
        }
        else if (parentObject.gameObject.tag == "RTSUnit")
        {
            //Debug.Log("Unit Detected " + this.name);
            unitAttributes = parentObject.GetComponent<UnitAttributes>();
            parentHealth = unitAttributes.unitAttributes.unitHealthPoints;
        }
    }

    private void SetHealthBar()
    {
        if (parentObject.gameObject.tag == "Minion")
        {
            parentHealth = minionAttributes.minionAttributes.unitHealthPoints;
            float healthCalculation = parentHealth / minionAttributes.minionAttributes.unitTotalHealthPoints;
            this.GetComponent<Image>().fillAmount = healthCalculation;
        }
        else if (parentObject.gameObject.tag == "RTSUnit")
        {
            parentHealth = unitAttributes.unitAttributes.unitHealthPoints;
            float healthCalculation = parentHealth / unitAttributes.unitAttributes.unitTotalHealthPoints;
            this.GetComponent<Image>().fillAmount = healthCalculation;
        }

    }
}
