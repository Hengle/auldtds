using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour {

    public GameObject parentArcher;
    public float arrowSpeed;
    public float destroyTimer;
    private UnitAttributes myUnitAttributes;

    public int minDamage;
    public int maxDamage;

	// Use this for initialization
	void Start()
    {
        parentArcher = this.transform.parent.gameObject;
        this.GetComponent<Rigidbody>().AddForce(transform.forward * arrowSpeed);
        Destroy(gameObject, destroyTimer);
	}
	
	

}
