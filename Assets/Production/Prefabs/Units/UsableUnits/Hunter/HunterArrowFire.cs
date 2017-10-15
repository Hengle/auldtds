using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterArrowFire : MonoBehaviour {

    private Animator unitAnimator;
    [SerializeField]
    private Transform arrowPrefab;
    [SerializeField]
    private Transform arrowSpawnPoint;


    // Use this for initialization
    void Start ()
    {
        unitAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (unitAnimator.GetBool("arrowFire")==true)
        {
            //Debug.Log("Fire Arrow");
            Transform arrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation);
            arrow.transform.parent = gameObject.transform;
            //Debug.Log("My daddy is "+arrow.gameObject.name);
            unitAnimator.SetBool("arrowFire", false);
        }
        
    }
}
