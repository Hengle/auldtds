using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementTip : MonoBehaviour
{
	#region Properties
	[Header("AvailablePlacement")]
	private Material[] materialInBody;
	private List<Material> availablePlacementMaterial;
	[SerializeField]
	private Material[] nonAvailablePlacementMaterial;
	private bool canBePlaced;
	[SerializeField]
	private GameObject[] bodyOfItem;

	[Header("BuildManager")]
	private GameObject buildManager;
	private BuildManager buildManagerScr;
	#endregion

	#region System Functions
	void Awake()
	{
		GetBuildManager();
	}

    // Use this for initialization
    void Start()
    {
		availablePlacementMaterial = new List<Material>();
		GetInitialMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
		CanBeBuildColor();
    }

	private void GetBuildManager()
	{
		buildManager = GameObject.Find("BuildManager");
		buildManagerScr = buildManager.GetComponent<BuildManager>();
	}
	#endregion

	#region Tip Functions
    private void Move()
    {
		if (buildManagerScr.isForSpecificPlacement == true)
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20.0f));
		}
		else
		{
			transform.position = buildManagerScr.Hit.point;
		}
    }

	private void CanBeBuildColor()
	{
		canBePlaced = buildManagerScr.canBePlaced;
		FixColor();
	}

	private void FixColor()
	{
		if (buildManagerScr.isForSpecificPlacement == false)
		{
			for (int i = 0; i < bodyOfItem.Length; i++)
			{
				if (bodyOfItem[i].GetComponent<SkinnedMeshRenderer>())
				{
					if(canBePlaced == true)
					{
						bodyOfItem[i].GetComponent<SkinnedMeshRenderer>().materials = availablePlacementMaterial.ToArray();
					}
					else
					{
						bodyOfItem[i].GetComponent<SkinnedMeshRenderer>().materials = nonAvailablePlacementMaterial;
					}
				}
				else if (bodyOfItem[i].GetComponent<MeshRenderer>())
				{
					if(canBePlaced == true)
					{
						bodyOfItem[i].GetComponent<MeshRenderer>().materials = availablePlacementMaterial.ToArray();
					}
					else
					{
						bodyOfItem[i].GetComponent<MeshRenderer>().materials = nonAvailablePlacementMaterial;
					}
				}
			}
		}
	}

	private void GetInitialMaterial()
	{
		for(int i = 0; i < bodyOfItem.Length; i++)
		{
			if (bodyOfItem[i].GetComponent<SkinnedMeshRenderer>() != null)
			{
				Material[] materialInBody = bodyOfItem[i].GetComponent<SkinnedMeshRenderer>().materials;
				for(int x = 0; x < materialInBody.Length; x++)
				{
					availablePlacementMaterial.Add(materialInBody[x]);
				}
			}	
			else if (bodyOfItem[i].GetComponent<MeshRenderer>() != null)
			{
				Material[] materialInBody = bodyOfItem[i].GetComponent<MeshRenderer>().materials;
				for(int x = 0; x < materialInBody.Length; x++)
				{
					availablePlacementMaterial.Add(materialInBody[x]);
				}
			}
		}
	}
	#endregion
}
