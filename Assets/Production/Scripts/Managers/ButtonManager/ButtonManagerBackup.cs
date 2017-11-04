/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlacementTags {DoorPlacement, BlockItemPlacement, MobPlacement};

[System.Serializable]
public class ButtonDataClass
{
	//public int itemCost;
	public LayerMask itemLayers;
	public PlacementTags itemTags;
	public bool isForSpecificPlacement;
	public GameObject selectedTipPrefab;
	public GameObject item;
}

[System.Serializable]
public class ButtonIndexSetting
{
	public Button itemButton;
	public int itemButtonIndex;
	//[HideInInspector]
	public int buttonCost;
}


public class ButtonManagerBackup : MonoBehaviour
{

	[Header("CashAndPoints")]
    [SerializeField]
    private int availableCash;
	private int isAvailalbleCashEnough;

	[Header("Button Setting")]
	[SerializeField]
	private ButtonIndexSetting[] allButtons;

	[Header("Select Button Variables")]
	[HideInInspector]
	public PlacementTags selectedItemTag;
	[HideInInspector]
	public ButtonDataClass selectedButtonData;
	[HideInInspector]
	public LayerMask selectedItemLayer;
	private int selectedItemIndex;
	private GameObject selectedItem;
	[SerializeField]
	private ButtonDataClass[] setButtonData;

	[Header("Mouse")]
	public bool destroyMouseTip;
	public bool isItemSelected;

    [Header("Submenus")]
    public bool trapMenuActive = false;
    public GameObject trapMenu;
    public bool unitMenuActive = false;
    public GameObject unitMenu;


    #region System Functions
    // Use this for initialization
    void Start ()
    {
		destroyMouseTip = false;
		isItemSelected = false;
	}

	// Update is called once per frame
	void Update () 
	{
        GetGold();
        DestroyMouseItem();
		AssignItemDataToButton();
		SetSelectedItemLayerAndItemTag();
        DisableButtons();

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

    #region Buttons Functions

    private void GetGold()
    {
        availableCash = GameMainManager.Instance._treasureGold;
    }
    public void ItemSelection(Button clickedButton)
	{
		for (int i = 0; i < allButtons.Length; i++)
		{
			if(allButtons[i].itemButton == clickedButton)
			{
				selectedItemIndex = allButtons[i].itemButtonIndex;
				selectedButtonData = setButtonData[selectedItemIndex];
			}
		}
	}

	private void AssignItemDataToButton()
	{
		for (int i = 0; i < allButtons.Length; i++)
		{
			for (int j =0; j < setButtonData.Length; j++)
			{
				if(allButtons[i].itemButtonIndex == j)
				{
                    allButtons[i].buttonCost = GetItemCost(setButtonData[j].item);
				}
			}
		}
	}

    private void DisableButtons()
    {
        if (Time.timeScale <1)
        {
            for (int i = 0; i < allButtons.Length; i++)
            {
                allButtons[i].itemButton.interactable = false;
            }                
        }
        else
        {
            for (int i = 0; i < allButtons.Length; i++)
            {
                if (allButtons[i].buttonCost > availableCash)
                {
                    allButtons[i].itemButton.interactable = false;
                }
                else
                {
                    allButtons[i].itemButton.interactable = true;
                }
            }
        }
    }

	private void SetSelectedItemLayerAndItemTag()
	{
		selectedItemLayer = selectedButtonData.itemLayers;
		selectedItemTag = selectedButtonData.itemTags; 
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

    private int GetItemCost(GameObject item)
    {
        if (item.tag == "RTSUnit")
        {
            int itemCosting;
            itemCosting = item.GetComponent<UnitAttributes>().unitBaseAttributes.unitCost;
            return itemCosting;
        }
        else if (item.tag == "BlockItems")
        {
            int itemCosting;
            itemCosting = item.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitCost;
            return itemCosting;
        }

        return 0; 
    }
    

    #endregion
}*/