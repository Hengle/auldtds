using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SelectableUnitComponent : MonoBehaviour
{
    public bool unitSelected = false;
    public GameObject baseUnitObject;

    public UnitSelectManager unitSelectManager;
    

    void Start()
    {
        unitSelectManager = FindObjectOfType(typeof(UnitSelectManager)) as UnitSelectManager;
    }

    private void Update()
    {

        if (unitSelected)
        {
            //Debug.Log("Unit is Selected");
            if (!unitSelectManager.selectedUnits.Contains(this.gameObject))
            {
                unitSelectManager.selectedUnits.Add(this.gameObject);
                unitSelectManager.AddPlayerUnitCounter();
                //Debug.Log("Object Added to List");
            }
           
        }
    
        if (!unitSelected)
        {
            if (unitSelectManager.selectedUnits.Contains(this.gameObject))
            {
                unitSelectManager.selectedUnits.Remove(this.gameObject);
                unitSelectManager.RemovePlayerUnitCounter();
                Debug.Log("Object Removed from List");
            }
        }
    
    }
}