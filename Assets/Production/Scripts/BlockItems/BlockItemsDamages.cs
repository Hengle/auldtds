using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItemsDamages : MonoBehaviour
{
	public float textYOffset = 0;
	private BlockItemsAttributes blockItemsAttributes;

	// Use this for initialization
	void Start ()
	{
		blockItemsAttributes = this.GetComponent<BlockItemsAttributes>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void TakeDamage(int damage, bool critical)
	{
		CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
		blockItemsAttributes.blockItemsAttributes.unitHealthPoints -= damage;
	}

	public void MissDamage(string miss, bool critical)
	{
		CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
	}
}
