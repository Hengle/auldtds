using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRange : MonoBehaviour {

    public GameObject parentCannon;
    public GameObject cannonEffectContainer;
    private ParticleSystem cannonEffect;
    public bool lockedTarget = false;
    public GameObject cannonTarget;
    public float cannonRotateSpeed;
    public int cannonDamage;
    public GameObject nullTarget;

    
    // Use this for initialization
	void Start ()
    {
        parentCannon = this.transform.parent.gameObject;
        nullTarget = GameObject.Find("NoTarget");
        cannonEffect = cannonEffectContainer.GetComponent<ParticleSystem>();
        cannonEffect.Stop(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (lockedTarget)
        {
            CheckifTargetIsDead(cannonTarget);
            if (cannonTarget)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cannonTarget.transform.position - parentCannon.transform.position);
                parentCannon.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, (cannonRotateSpeed * Time.deltaTime));
                //parentCannon.transform.LookAt(cannonTarget.transform);
            }
        }

        
	}

    private void OnTriggerEnter(Collider other)
    {
		if(other.tag == "Minion")
		{
			if (lockedTarget == false && other.GetComponent<MinionAttributes>().minionAttributes.unitIsAlive)
	        {
	            //Debug.Log("Getting ready to fire");
	            cannonTarget = other.gameObject;
	            lockedTarget = true;
	            cannonEffect.Play();
	        }
			else if (lockedTarget == true && other.GetComponent<MinionAttributes>().minionAttributes.unitIsAlive == false)
	        {
	            cannonEffect.Stop();
	            lockedTarget = false;
	        }
		}       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Minion")
        {
            Debug.Log("Stopping");
            lockedTarget = false;
            cannonTarget = nullTarget;
            cannonEffect.Stop();
            other.gameObject.GetComponent<MinionDamages>().StopDot();
            other.gameObject.GetComponent<MinionDamages>().dotEffect = false;

        }
    }

    private void CheckifTargetIsDead(GameObject target)
    {
		if (target.GetComponent<MinionAttributes>().minionAttributes.unitIsAlive == false)
        {
            lockedTarget = false;
            cannonTarget = nullTarget;  
            cannonEffect.Stop();
        }
        /*else
        {
            cannonTarget = target.gameObject;
            cannonEffect.Play();
            lockedTarget = true;
        }*/
        
    }
}
