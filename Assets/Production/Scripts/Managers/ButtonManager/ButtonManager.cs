using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ButtonDataClass
{
	public int itemIndex;
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
	[HideInInspector]
	public int itemCostMithril;
}

[System.Serializable]
public class ButtonSettingClass
{
	public Button itemButton;
	public int setItemIndex;
	public string shortKey;
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
	[HideInInspector]
	public string selectedButtonShortKey;
	private Button pressedButton;

	[Header("Gold")]
	private int availableCash;
	private int availableMithril;

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
		SetClickOnButton();
	}

	// Update is called once per frame
	void Update () 
	{
		GetPlayersCash();
		DestroyMouseItem();
		DisableButtons();
		SetShortKey();
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
				buttonData[i].itemCostMithril = buttonData[i].item.GetComponent<UnitAttributes>().unitBaseAttributes.unitCostMithril;
			}
			else if(buttonData[i].itemTag == "BlockItems")
			{
				buttonData[i].whereToSpawnItemLayer = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitSpawnLayer;
				buttonData[i].isForSpecificPlacement = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.isForSpecificPlacement;
				buttonData[i].placementTag = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.placementTag;
				buttonData[i].itemCost = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
				buttonData[i].itemCostMithril = buttonData[i].item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCostMithril;
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

	private void ItemSelection()
	{
		for (int i = 0; i < buttonSetting.Length; i++)
		{
			if(buttonSetting[i].itemButton.GetComponentInChildren<Text>().text == pressedButton.GetComponentInChildren<Text>().text)
			{
				selectedItemIndex = buttonSetting[i].setItemIndex;
				selectedButtonData = buttonSetting[i].setButtonData;
				selectedButtonShortKey = buttonSetting[i].shortKey;
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
				if ((buttonSetting[i].setButtonData.itemCost > availableCash) || (buttonSetting[i].setButtonData.itemCostMithril > availableMithril))
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

	private void PushButton()
	{
		pressedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

		for (int i = 0; i < buttonSetting.Length; i++)
		{
			if(buttonSetting[i].itemButton == pressedButton)
			{
				ItemSelection();
				InstatiateItem();
			}
		}
	}

	private void SetClickOnButton()
	{
		for (int i = 0; i < buttonSetting.Length; i++)
		{
			buttonSetting[i].itemButton.onClick.AddListener(PushButton);
		}
	}

	private void SetShortKey()
	{
		for (int i = 0; i < buttonSetting.Length; i++)
		{
			if (Input.GetKeyDown(buttonSetting[i].shortKey))
			{
				pressedButton = buttonSetting[i].itemButton;
				ItemSelection();
				InstatiateItem();
			}
		}
	}

#endregion

#region General Functions
	private void GetPlayersCash()
	{
		availableCash = GameMainManager.Instance._treasureGold;
		availableMithril = GameMainManager.Instance._treasureMithril;
	}
#endregion

#region Mouse Functions
	private void InstatiateItem()
	{
		Destroy(selectedItem);
		if (selectedButtonData.item.tag == "RTSUnit")
		{
			selectedItem = Instantiate(selectedButtonData.item.GetComponent<UnitAttributes>().unitBaseAttributes.mouseTipItem, transform.position, Quaternion.identity);
		}
		else if (selectedButtonData.item.tag == "BlockItems")
		{
			selectedItem = Instantiate(selectedButtonData.item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.mouseTipItem, transform.position, Quaternion.identity);
		}
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