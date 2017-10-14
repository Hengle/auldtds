using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherThreatArea : MonoBehaviour
{

    public GameObject parentUnit;
    public Transform unitTargetTransform;
    public GameObject unitTarget;
    public LayerMask rayCastBlockLayer;

    [SerializeField]
    private List<GameObject> enemiesList = new List<GameObject>();
    private Animator unitAnimator;

    // Use this for initialization
    void Start ()
    {
        parentUnit = transform.parent.gameObject;
        unitAnimator = parentUnit.GetComponent<Animator>();
        unitAnimator.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Minion")
        {
            //Debug.Log("Target Aquired");
            RaycastHit hit;
            if (!Physics.Linecast(parentUnit.transform.position, other.gameObject.transform.position, out hit, rayCastBlockLayer))
            {
                unitTarget = other.gameObject;
                unitTargetTransform = other.gameObject.transform;
                FaceEnemy();
                unitAnimator.enabled = true;
            }
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Minion")
        {
            //Debug.Log("Enemy is in Range");
            unitTarget = other.gameObject;
            unitTargetTransform = other.gameObject.transform;
            FaceEnemy();
            unitAnimator.enabled = true;
        }
       
    }
    */
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Minion")
        {
            //StopShooting.
            unitAnimator.enabled = false;
        }
    }

    private void FaceEnemy()
    {
        Vector3 relativePos = unitTargetTransform.position - parentUnit.transform.position;
        Quaternion unitRotation = Quaternion.LookRotation(relativePos);
        parentUnit.transform.rotation = unitRotation;
    }
}
