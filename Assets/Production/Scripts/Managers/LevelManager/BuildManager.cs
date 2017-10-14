﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

	[Header("Placement Variables")]
	private GameObject itemPlacement;
	private LayerMask placementLayer;
	private GameObject[] placementsAvailable;
	//[HideInInspector]
	public GameObject lastPlacedItem;
	public RaycastHit Hit;

	[Header("Check If Other Item in Position")]
	public bool canBePlaced;
	[SerializeField]
	private float radius;
	[SerializeField]
	private int excludeLayer;
	[SerializeField]
	private int excludeLayerMask;
	[SerializeField]
	private Vector3 boxHalfExtents;
	[SerializeField]
	private Quaternion boxQuaternion;
	[SerializeField]
	private Vector3 boxCenter;
	[SerializeField]
	private Vector3 boxDirection;
	[SerializeField]
	private int avoidLayer;	//LevelObjects Layer
	[SerializeField]
	private int avoidLayerMask;

	[Header("UIManager")]
	private GameObject uiManager;
	private ButtonManager buttonManagerScr;
	public bool isForSpecificPlacement;

#region System Functions
	void Awake()
	{
		ChooseLayer();
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		ItemPlacement();
		GetLayerFromButton();
		ShowItemsToBuild();
	}

	private void GetUIManager()
	{
		uiManager = GameObject.Find("ButtonManager");
		buttonManagerScr = uiManager.GetComponent<ButtonManager>();
		isForSpecificPlacement = buttonManagerScr.selectedButtonData.isForSpecificPlacement;
	}
	#endregion

#region Placement Functions
	private void ItemPlacement()
	{
		GetUIManager();

		if(buttonManagerScr.isItemSelected == true)
		{
			if(buttonManagerScr.selectedButtonData.isForSpecificPlacement)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit Hit;
				if (Physics.Raycast(ray, out Hit, 1000, placementLayer))
				{
					itemPlacement = Hit.collider.gameObject;

					if(Input.GetButtonDown("Fire1"))
					{
						buttonManagerScr.destroyMouseTip = true;
						lastPlacedItem = Instantiate(buttonManagerScr.selectedButtonData.item, itemPlacement.transform.position, itemPlacement.transform.rotation);
                        GameMainManager.Instance._treasureGold -= lastPlacedItem.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
						lastPlacedItem.GetComponent<BlockItemsAttributes>().savedPlacement = itemPlacement;
						itemPlacement.SetActive(false);
                        if (lastPlacedItem.tag == "BlockItems")
                        {
                            lastPlacedItem.transform.SetParent(GameObject.Find("StationaryObjects").transform);
                        }
                    }
				}
			}
			else
			{
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (buttonManagerScr.selectedButtonData.item.GetComponent<BoxCollider>() != null)
				{
					boxCenter = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
					boxHalfExtents = new Vector3(buttonManagerScr.selectedButtonData.item.GetComponent<BoxCollider>().size.x/2, buttonManagerScr.selectedButtonData.item.GetComponent<BoxCollider>().size.y/2, buttonManagerScr.selectedButtonData.item.GetComponent<BoxCollider>().size.z/2);
					boxQuaternion = buttonManagerScr.selectedButtonData.item.GetComponent<BoxCollider>().transform.rotation;

					if (!Physics.BoxCast (boxCenter, boxHalfExtents, ray.direction, out Hit, boxQuaternion, 1000, excludeLayerMask))
					{
						canBePlaced = true;
					}
					else
					{
						canBePlaced = false;
					}
				}
				else if (buttonManagerScr.selectedButtonData.item.GetComponent<CapsuleCollider>() != null)
				{
					radius = buttonManagerScr.selectedButtonData.item.GetComponent<CapsuleCollider>().radius;

					if (!Physics.SphereCast (ray, radius, out Hit, 1000, excludeLayerMask))
					{
						canBePlaced = true;
					}
					else
					{
						canBePlaced = false;
					}
				}
				else if (buttonManagerScr.selectedButtonData.item.GetComponent<SphereCollider>() != null)
				{
					radius = buttonManagerScr.selectedButtonData.item.GetComponent<SphereCollider>().radius;

					if (!Physics.SphereCast (ray, radius, out Hit, 1000, excludeLayerMask))
					{
						canBePlaced = true;
					}
					else
					{
						canBePlaced = false;
					}
				}
					
				if (canBePlaced == true)
				{
					if (Physics.Raycast(ray, out Hit, 1000, placementLayer))
					{
						if(Input.GetButtonDown("Fire1"))
						{
							buttonManagerScr.destroyMouseTip = true;
							GameObject myItem = Instantiate(buttonManagerScr.selectedButtonData.item, Hit.point, Quaternion.identity);

							if(myItem.GetComponent<UnitAttributes>())
							{
								GameMainManager.Instance._treasureGold -= myItem.GetComponent<UnitAttributes>().unitAttributes.unitCost;
							}
							if(myItem.GetComponent<BlockItemsAttributes>())
							{
								GameMainManager.Instance._treasureGold -= myItem.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
							}

							if (myItem.tag == "RTSUnit")
							{
								myItem.transform.SetParent(GameObject.Find("HeroUnits").transform);
							}
							else if (myItem.tag == "BlockItems")
							{
								myItem.transform.SetParent(GameObject.Find("StationaryObjects").transform);
							}
						}
					}
				}			
			}
		}
	}

	private void GetLayerFromButton()
	{
		GetUIManager();
		placementLayer = buttonManagerScr.selectedItemLayer;
	}

	private void ShowItemsToBuild()
	{
		if(buttonManagerScr.selectedButtonData.isForSpecificPlacement)
		{
			placementsAvailable = GameObject.FindGameObjectsWithTag(buttonManagerScr.selectedItemTag.ToString());
			for (int i =0; i < placementsAvailable.Length; i++)
			{
				if(buttonManagerScr.isItemSelected == true)
				{
					placementsAvailable[i].GetComponent<MeshRenderer>().enabled = true;
				}
				else
				{
					placementsAvailable[i].GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
	}

	private void ChooseLayer()
	{
		excludeLayer = LayerMask.NameToLayer("BodyOfPlacedItems");
		avoidLayer = LayerMask.NameToLayer("LevelObjects");
		excludeLayerMask = 1 << excludeLayer | 1 << avoidLayer;
	}
#endregion
}
