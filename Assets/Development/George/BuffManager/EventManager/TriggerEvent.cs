using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("q"))
        {
            EventManager.TriggerEvent("Test");
        }
	}
}
