using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitThreatArea : MonoBehaviour
{

    public GameObject parentUnit;
    public UnitAnimations myUnitAnim;
    public Transform unitTargetTransform;
    public GameObject unitTarget;

	// Use this for initialization
	void Start ()
    {
        parentUnit = transform.parent.gameObject;
        myUnitAnim = transform.parent.GetComponent<UnitAnimations>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (parentUnit.GetComponent<UnitAttributes>().unitAttributes.unitIsAlive)
        {
            if ((other.tag == "Minion"))
            {
                //Debug.Log("Attaaaack "+other.name);
                if (other.GetComponent<MinionAttributes>().minionAttributes.unitIsAlive == true)
                {
                    unitTargetTransform = other.gameObject.transform;
                    unitTarget = other.gameObject;
                    myUnitAnim.animStateIdle = false;
                    myUnitAnim.animStateAttack = true;
                    FaceEnemy();
                }
                else
                {
                    myUnitAnim.animStateAttack = false;
                    myUnitAnim.animStateIdle = true;
                }
            }
        }
        else
        {
            myUnitAnim.animStateAttack = false;
            myUnitAnim.animStateIdle = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Minion")
        {
            myUnitAnim.animStateAttack = false;
            myUnitAnim.animStateIdle = true;
        }
    }

    private void FaceEnemy()
    {
        Vector3 relativePos = unitTargetTransform.position - parentUnit.transform.position;
        Quaternion unitRotation = Quaternion.LookRotation(relativePos);
        parentUnit.transform.rotation = unitRotation;
    }
}
