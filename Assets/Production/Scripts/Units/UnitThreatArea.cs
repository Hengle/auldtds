using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitThreatArea : MonoBehaviour
{

    public GameObject parentUnit;
    private UnitAttributes unitAttributes;
    private Animator unitAnimator;
    public Transform unitTargetTransform;
    public GameObject unitTarget;
    public bool unitAttacking;

	// Use this for initialization
	void Start ()
    {
        unitAttacking = false;
        parentUnit = transform.parent.gameObject;
        unitAttributes = parentUnit.GetComponent<UnitAttributes>();
        unitAnimator = parentUnit.GetComponent<Animator>();
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
                    if (!unitAttacking)
                    {
                        InvokeRepeating("AttackNormal", 0.5f, unitAttributes.unitAttributes.unitCDScore);
                        unitAttacking = true;
                    }
                    //myUnitAnim.animStateIdle = false;
                    //myUnitAnim.animStateAttack = true;
                    FaceEnemy();
                }
                else
                {
                    unitAnimator.SetBool("Idle", true);
                    unitAttacking = false;
                    CancelInvoke();
                }
            }
        }
        else
        {
            unitAnimator.SetBool("Idle", true);
            unitAttacking = false;
            CancelInvoke();
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Minion")
        {
            unitAnimator.SetBool("Idle", true);
            unitAttacking = false;
            CancelInvoke();
        }
    }

    private void FaceEnemy()
    {
        Vector3 relativePos = unitTargetTransform.position - parentUnit.transform.position;
        Quaternion unitRotation = Quaternion.LookRotation(relativePos);
        parentUnit.transform.rotation = unitRotation;
    }

    private void AttackNormal()
    {
        unitAnimator.SetTrigger("AttackNormal");
    }
}
