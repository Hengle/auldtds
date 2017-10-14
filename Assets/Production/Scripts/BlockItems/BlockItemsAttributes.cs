using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItemsAttributes : MonoBehaviour
{
	#region Properties
	[Header("Block Items Attributes")]
	public UnitBaseClass blockItemsAttributes = new UnitBaseClass();

	[Header("Item Palcement")]
	[SerializeField]
	public GameObject savedPlacement;
	#endregion

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		BlockItemDeath();
	}

	private void BlockItemDeath()
	{
		if (blockItemsAttributes.unitHealthPoints <=0 && blockItemsAttributes.unitIsAlive == true)
		{
			blockItemsAttributes.unitIsAlive = false;

			Reactivate();
			//animation for dying
			//StartCoroutine(DestroyOnDeath());
			Destroy(gameObject);
		}
	}

	private void Reactivate()
	{
		if(savedPlacement)
		{
			savedPlacement.SetActive(true);
			savedPlacement.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	/*
	private IEnumerator DestroyOnDeath()
	{
		yield return new WaitForSeconds(3.5f);
		Destroy(gameObject);
	}
*/
}
