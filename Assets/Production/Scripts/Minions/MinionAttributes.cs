using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttributes : MonoBehaviour
{
	public UnitBaseClass minionAttributes = new UnitBaseClass();

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
        //MinionDeath();
        CheckUnitHealth();

    }


    private void CheckUnitHealth()
    {
        if (minionAttributes.unitHealthPoints <=0)
        {
            minionAttributes.unitIsAlive = false;
        }
    }
	
}
