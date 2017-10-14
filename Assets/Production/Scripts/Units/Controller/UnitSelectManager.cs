using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectManager : MonoBehaviour
{
    public int userControledUnits = 0;
    
   public List<GameObject> selectedUnits = new List<GameObject>();
   //public Dictionary<int, GameObject> selectedUnits = new Dictionary<int, GameObject>();
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddPlayerUnitCounter()
    {
        userControledUnits++;
    }

    public void RemovePlayerUnitCounter()
    {
        userControledUnits--;
    }


}
