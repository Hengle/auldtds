using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{

    public GameObject[] xpBarElements;
    public GameObject activeXPBarObject;
    public int currentXPBarNumber = 0;
    public Text xpText;
    
    public int currentLevelXP = 0;
    public int nextLevelXP = 2000;
    
    public int numberOfXPBars;
    public float xpPerBar;
    float fillAmount = 0;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("George Edit");
        numberOfXPBars = xpBarElements.Length;
        xpPerBar = nextLevelXP / numberOfXPBars;
        InitializeXPBars();
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetCurrentActiveXPBar();
        SetXPBarFill(activeXPBarObject);
	}

    public void SetCurrentActiveXPBar()
    {

        if (currentLevelXP <=nextLevelXP)
        {
            if (currentLevelXP <= xpPerBar)
            {
                activeXPBarObject = xpBarElements[0];
                currentXPBarNumber = 1;
            }
            else if (currentLevelXP > xpPerBar && currentLevelXP <= (xpPerBar * 2))
            {
                activeXPBarObject = xpBarElements[1];
                currentXPBarNumber = 2;
            }
            else if (currentLevelXP > (xpPerBar * 2) && currentLevelXP <= (xpPerBar * 3))
            {
                activeXPBarObject = xpBarElements[2];
                currentXPBarNumber = 3;
            }
            else if (currentLevelXP > (xpPerBar * 3) && currentLevelXP <= (xpPerBar * 4))
            {
                activeXPBarObject = xpBarElements[3];
                currentXPBarNumber = 4;
            }
            else if (currentLevelXP > (xpPerBar * 4) && currentLevelXP <= (xpPerBar * 5))
            {
                activeXPBarObject = xpBarElements[4];
                currentXPBarNumber = 5;
            }
            else if (currentLevelXP > (xpPerBar * 5) && currentLevelXP <= (xpPerBar * 6))
            {
                activeXPBarObject = xpBarElements[5];
                currentXPBarNumber = 6;
            }
            else if (currentLevelXP > (xpPerBar * 6) && currentLevelXP <= (xpPerBar * 7))
            {
                activeXPBarObject = xpBarElements[6];
                currentXPBarNumber = 7;
            }
            Debug.Log("Running, CurrentXP=" + currentLevelXP + " Current Active Bar:" + activeXPBarObject.name);
        }
        else
        {
            currentLevelXP = nextLevelXP;
        }
         
        
    }

    public void InitializeXPBars()
    {
        foreach (GameObject item in xpBarElements)
        {
            item.GetComponent<Image>().fillAmount = 0;
        }

        SetCurrentActiveXPBar();
    }

    public void AwardXP(int xpreward)
    {
        currentLevelXP += xpreward;
    }

    public void SetXPBarFill(GameObject xpBar)
    {
        Debug.Log("Current Active Bar is " + xpBar.name +" Current LevelXP: "+currentLevelXP);
        Image barfill = xpBar.GetComponent<Image>();
        float maxXP = xpPerBar;  //this is the 100% so fill amount = 1;
        
        if (currentLevelXP == (xpPerBar * currentXPBarNumber))
        {
            fillAmount = 1;
        }
        else
        {
            fillAmount = (currentLevelXP / xpPerBar) % 1;
        }
        Debug.Log("The fillamountCALC currently is " + fillAmount);
        barfill.fillAmount = fillAmount;
    }
}
