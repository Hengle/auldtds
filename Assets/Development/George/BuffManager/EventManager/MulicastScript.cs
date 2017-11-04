using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulicastScript : MonoBehaviour
{
    delegate void MultiDelegate();
    MultiDelegate myMultiDelegate;

	// Use this for initialization
	void Start ()
    {
        myMultiDelegate += PowerUp;
        myMultiDelegate += TurnRed;

        if (myMultiDelegate != null)
        {
            myMultiDelegate();
        }
        
        
	}
	
    void PowerUp()
    {
        print("Cube is powered Up");
    }
	
    void TurnRed()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }
}
