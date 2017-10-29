using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowImpact : MonoBehaviour
{

    public Transform parentObjectTransform;
    public GameObject parentObject;
    private UnitAttributes myArrowStats;
    public int minDamage;
    public int maxDamage;
    public int arrowDamage;


    private void Start()
    {
        //parentObjectTransform = this.transform.parent;
        parentObject = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        minDamage = parentObject.GetComponent<UnitAttributes>().unitBaseAttributes.unitMinDamage;
        maxDamage = parentObject.GetComponent<UnitAttributes>().unitBaseAttributes.unitMaxDamage;
        arrowDamage = Random.Range(minDamage, maxDamage + 1);
    }
    // Use this for initialization
    
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            other.GetComponent<MinionDamages>().TakeDamage(arrowDamage, false);
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (other.tag == "LevelObjects")
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        
    }
}
