using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ButtonDataClass
{
	public int itemIndex;
	public GameObject selectedTipPrefab;
	public GameObject item;
	[HideInInspector]
	public string itemTag;
	[HideInInspector]
	public LayerMask whereToSpawnItemLayer;
	[HideInInspector]
	public PlacementTags placementTag;
	[HideInInspector]
	public bool isForSpecificPlacement;
	[HideInInspector]
	public int itemCost;
}

[System.Serializable]
public class ButtonSettingClass
{
	public Button itemButton;
	public int setItemIndex;
	[HideInInspector]
	public ButtonDataClass setButtonData;
}
	
public class ButtonManager : MonoBehaviour
{
#region Variables
	[Header("Item Properties")]
	[SerializeField]
	private ButtonDataClass[] buttonData;

	[Header("SetButtons")]
	[SerializeField]
	private ButtonSettingClass[] buttonSetting;

	[Header("SelectedButton")]
	private int selectedItemIndex;
	[HideInInspector]
	public ButtonDataClass selectedButtonData;
	//[SerializeField]
	private string clickedButtonName;

	[Header("Gold")]
	private int availableCash;

	[Header("Mouse")]
	private GameObject selectedItem;
	[HideInInspector]
	public bool destroyMouseTip;
	[HideInInspector]
	public bool isItemSelected;

	[Header("Submenus")]
	public bool trapMenuActive = false;
	public GameObject trapMenu;
	public bool unitMenuActive = false;
	public GameObject unitMenu;
#endregion

#region System Functions
	// Use this for initialization
	void Start ()
	{
		SetItemProperties();
		destroyMouseTip = false;
		isItemSelected = false;
	}

	// Update is called once per frame
	void Update () 
	{
		GetGold();
		DestroyMouseItem();
		DisableButtons();
	}
#endregion

#region Buttons Functions
	private void SetItemProperties()
	{
		for (int i =0; i < buttonData.Length; i++)
		{
			buttonData[i].itemTag = buttonData[i].item.tag;
			if (buttonData[i].itemTag == "RTSUnit")
			{
			buttonData[i].whereToSpawnItemLayer = buttonData[i].item.GetComponent<UnitAttributes>().unitBaseAttributes.unitSpawnLayer;
			buttonData[i].isForSpecificPlacement = buttonData[i].item.GetComponent<UnitAttributes>().unitBaseAttributes.isForSpecificPlacement;
			buttonData[i].placementTag = buttonData[i].item.GetComponent<UnitAttributes>().unitBaseAttributes.placementTag;
			buttonData[i].itemCost = buttonData[i].item.GetComponent<UnitAttributes>().unitBaseAttributes.unitCost;
			}
			else if(buttonData[i].itemTag == "BlockItems")
			{
				buttonData[i].whereToSpawnItemLayer = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitSpawnLayer;
				buttonData[i].isForSpecificPlacement = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.isForSpecificPlacement;
				buttonData[i].placementTag = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.placementTag;
				buttonData[i].itemCost = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
			}
		}

		for (int j = 0; j < buttonSetting.Length; j++)
		{
			for (int z = 0; z < buttonData.Length; z++)
			{
				if(buttonData[z].itemIndex == buttonSetting[j].setItemIndex)
				{
					buttonSetting[j].setButtonData = buttonData[z];
				}
			}
		}
	}

	public void ItemSelection()
	{
		clickedButtonName = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;

		for (int i = 0; i < buttonSetting.Length; i++)
		{
			if(buttonSetting[i].itemButton.GetComponentInChildren<Text>().text == clickedButtonName)
			{
				selectedItemIndex = buttonSetting[i].setItemIndex;
				selectedButtonData = buttonSetting[i].setButtonData;
			}
		}
	}

	private void DisableButtons()
	{
		if (Time.timeScale <1)
		{
			for (int i = 0; i < buttonSetting.Length; i++)
			{
				buttonSetting[i].itemButton.interactable = false;
			}                
		}
		else
		{
			for (int i = 0; i < buttonSetting.Length; i++)
			{
				if (buttonSetting[i].setButtonData.itemCost > availableCash)
				{
					buttonSetting[i].itemButton.interactable = false;
				}
				else
				{
					buttonSetting[i].itemButton.interactable = true;
				}
			}
		}
	}
#endregion

#region General Functions
	private void GetGold()
	{
		availableCash = GameMainManager.Instance._treasureGold;
	}
#endregion

#region Mouse Functions
	public void InstatiateItem()
	{
		Destroy(selectedItem);
		selectedItem = Instantiate(selectedButtonData.selectedTipPrefab, transform.position, Quaternion.identity);
		isItemSelected = true;
	}

	private void DestroyMouseItem()
	{
		if((destroyMouseTip == true) || (Input.GetKey(KeyCode.Escape)))
		{
			Destroy(selectedItem);
			destroyMouseTip = false;
			isItemSelected = false;
		}
	}
#endregion

#region Buttons Functions for Submenus [GA]
	public void ShowMenu(string menuName)
	{
		destroyMouseTip = true;

		if (menuName == "GameActionBar-Units")
		{
			if (!unitMenuActive)
			{
				unitMenuActive = true;
				trapMenuActive = false;
				trapMenu.SetActive(false);
				unitMenu.SetActive(true);
			}
		}
		if (menuName == "GameActionBar-Traps")
		{
			if (!trapMenuActive)
			{
				unitMenuActive = false;
				trapMenuActive = true;
				unitMenu.SetActive(false);
				trapMenu.SetActive(true);
			}
		}
	}
#endregion
}