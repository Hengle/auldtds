using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TrapNeedle : MonoBehaviour
{
	[Header("Animation")]
	private Animation myAnimation;
	private float timeLengthAnimation;

	[Header("Trigger")]
	[SerializeField]
	private bool trapTriggered = false;
	[SerializeField]
	private float trapCD;
	[SerializeField]
	private float trapTriggerTime;

	[Header("TrapDamage")]
	[SerializeField]
	private int trapDamage;
	[SerializeField]
	private List<GameObject> minionsList;

	// Use this for initialization
	void Start ()
	{
		myAnimation = GetComponent<Animation>();
		minionsList = new List<GameObject>();
		timeLengthAnimation = this.GetComponent<Animation>().GetClip("Anim_TrapNeedle_Show").length;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Minion" && !trapTriggered)
		{
			StartCoroutine(ActivateTrap(trapTriggerTime));
		}

		if(other.tag == "Minion")
		{
			if(!minionsList.Contains(other.gameObject))
			{
				minionsList.Add(other.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Minion")
		{
			minionsList.Remove(other.gameObject);
		}
	}

	private IEnumerator ActivateTrap(float time)
	{
		if (time < timeLengthAnimation)
		{
			time = timeLengthAnimation;
		}

		trapTriggered = true;
		yield return new WaitForSeconds(time);
		myAnimation.CrossFade("Anim_TrapNeedle_Show");
		TrapDamage();
		this.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
		if (trapCD >0)
		{
			Invoke("ResetTrap", trapCD);
		}
	}

	private void ResetTrap()
	{
		myAnimation.CrossFade("Anim_TrapNeedle_Hide");
		trapTriggered = false;
		this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
		minionsList.Clear();
	}

	private void TrapDamage()
	{
		foreach (GameObject minion in minionsList)
		{
			minion.GetComponent<MinionDamages>().TakeDamage(trapDamage, true);
		}
	}
}