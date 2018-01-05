using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

	[Header("Placement Variables")]
	[SerializeField]
	private GameObject itemPlacement;
	[SerializeField]
	private LayerMask placementLayer;
	[SerializeField]
	private GameObject[] placementsAvailable;
	//[HideInInspector]
	public GameObject lastPlacedItem;
	public RaycastHit Hit;

	[Header("Check If Other Item in Position")]
	public bool canBePlaced;
	[SerializeField]
	private float radius;
	[SerializeField]
	private int excludeLayer1;
	[SerializeField]
	private int excludeLayer2;
	[SerializeField]
	private int excludeLayer3;
    [SerializeField]
    private int excludeLayer4;
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
						GameMainManager.Instance._treasureGold -= lastPlacedItem.GetComponent<BlockItem.StateController>().blockItemStats.coinCost;
						GameMainManager.Instance._treasureMithril -= lastPlacedItem.GetComponent<BlockItem.StateController>().blockItemStats.mithrilCost;
						lastPlacedItem.GetComponent<BlockItem.StateController>().savedPlacement = itemPlacement;
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
								GameMainManager.Instance._treasureGold -= myItem.GetComponent<UnitAttributes>().unitBaseAttributes.unitCost;
								GameMainManager.Instance._treasureMithril -= myItem.GetComponent<UnitAttributes>().unitBaseAttributes.unitCostMithril;
							}
							if(myItem.GetComponent<BlockItemsAttributes>())
							{
								GameMainManager.Instance._treasureGold -= myItem.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
								GameMainManager.Instance._treasureMithril -= myItem.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCostMithril;
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
		placementLayer = buttonManagerScr.selectedButtonData.whereToSpawnItemLayer;
	}

	private void ShowItemsToBuild()
	{
		if(buttonManagerScr.selectedButtonData.isForSpecificPlacement)
		{
			placementsAvailable = GameObject.FindGameObjectsWithTag(buttonManagerScr.selectedButtonData.placementTag.ToString());
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
		else
		{
			for (int i =0; i < placementsAvailable.Length; i++)
			{
				placementsAvailable[i].GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	private void ChooseLayer()
	{
		excludeLayer1 = LayerMask.NameToLayer("EnemyUnits");
		excludeLayer2 = LayerMask.NameToLayer("LevelObjects");
		excludeLayer3 = LayerMask.NameToLayer("RTSUnits");
        excludeLayerMask = 1 << excludeLayer1 | 1 << excludeLayer2 | 1 << excludeLayer3;
	}
	#endregion
}
