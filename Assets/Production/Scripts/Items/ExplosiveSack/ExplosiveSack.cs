using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveSack : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private float chanceToTrigger;
    [SerializeField]
    private bool isTriggered = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isTriggered)
        {
            GameObject.Destroy(this);
        }
	}
}
