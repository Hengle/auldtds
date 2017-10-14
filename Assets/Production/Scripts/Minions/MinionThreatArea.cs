using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionThreatArea : MonoBehaviour
{
	public UnitAnimations myUnitAnim;
	public Transform unitTargetTransform;
	public GameObject unitTarget;

	// Use this for initialization
	void Start ()
	{
		myUnitAnim = transform.parent.GetComponent<UnitAnimations>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerStay(Collider other)
	{
		Debug.Log("Collision detected");
		if (other.tag == "BlockItems")
		{
			Debug.Log("Attaaaack");
			if (other.GetComponent<UnitAttributes>().unitAttributes.unitIsAlive == true)
			{
				unitTargetTransform = other.gameObject.transform;
				unitTarget = other.gameObject;
				//myUnitAnim.animStateIdle = false;
				//myUnitAnim.animStateAttack = true;
			}
			else
			{
				//myUnitAnim.animStateAttack = false;
				//myUnitAnim.animStateIdle = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "BlockItems")
		{
			//myUnitAnim.animStateAttack = false;
			//myUnitAnim.animStateIdle = true;
		}
	}
}
