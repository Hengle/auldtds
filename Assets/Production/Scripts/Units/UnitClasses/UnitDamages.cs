using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDamages : MonoBehaviour
{
	public float textYOffset = 0;
	private UnitAttributes unitAttributes;

	// Use this for initialization
	void Start ()
	{
		unitAttributes = this.GetComponent<UnitAttributes>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void TakeDamage(int damage, bool critical)
	{
		CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
		unitAttributes.unitAttributes.unitHealthPoints -= damage;
	}

	public void MissDamage(string miss, bool critical)
	{
		CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
	}
}
