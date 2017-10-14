using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPMan_backup : MonoBehaviour
{
    private static XPMan_backup instance;
    public static XPMan_backup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<XPMan_backup>();
            }
            return instance;
        }
    }


    public GameObject[] xpBarElements;
    public GameObject activeXPBarObject;
    public int currentXPBarNumber = 0;
    public Text xpText;
   
    public int currentLevelXP;
    public int nextLevelXP;

    public int numberOfXPBars;
    public float xpPerBar;
    float fillAmount = 0;

    private void Start()
    {
        SetActiveXPBar();
        DisplayXPScore();
    }

    private void Update()
    {
        DisplayXPScore();
    }

    public void SetActiveXPBar()
    {
        //currentLevelXP = GameMainManager.Instance._playerCurrentLevelXP;
        //nextLevelXP = GameMainManager.Instance._playerNextLevelXP;

        numberOfXPBars = xpBarElements.Length;
        xpPerBar = nextLevelXP / numberOfXPBars;
        if (currentLevelXP <= xpPerBar)
        {
            if (currentLevelXP == xpPerBar)
            {
                currentXPBarNumber = 0;
                activeXPBarObject = xpBarElements[currentXPBarNumber];
                float xpDifference = 1.0f;
                //Debug.Log("Difference is: " + xpDifference);
                activeXPBarObject.GetComponent<Image>().fillAmount = xpDifference;
                currentXPBarNumber++;
                activeXPBarObject = xpBarElements[currentXPBarNumber];
            }
            else
            {
                currentXPBarNumber = 0;
                activeXPBarObject = xpBarElements[currentXPBarNumber];

                float xpCalculation = currentLevelXP / xpPerBar;
                float xpDifference = (xpCalculation) % 1;
                //Debug.Log("Difference is: " + xpDifference);
                activeXPBarObject.GetComponent<Image>().fillAmount = xpDifference;
            }
            
        }
        else
        {
            float xpCalculation = currentLevelXP / xpPerBar;
            Debug.Log("XP Calculation: " + xpCalculation);
            float mainXPbar = Mathf.Floor(xpCalculation);
            Debug.Log("Main is: " + mainXPbar);
            for (int i = 0; i < mainXPbar; i++)
            {
                activeXPBarObject = xpBarElements[i];
                activeXPBarObject.GetComponent<Image>().fillAmount = 1;
            }

            float xpDifference = (xpCalculation) % 1;
            Debug.Log("Difference is: " + xpDifference);
            activeXPBarObject = xpBarElements[((int)mainXPbar)];
            activeXPBarObject.GetComponent<Image>().fillAmount = xpDifference;

        }
    }


    public void DisplayXPScore()
    {
        Text xpDisplay = xpText.GetComponent<Text>();
        currentLevelXP = GameMainManager.Instance._playerCurrentLevelXP;
        xpDisplay.text = currentLevelXP.ToString()+"/"+GameMainManager.Instance._playerNextLevelXP.ToString();
        //Debug.Log("Text is: " + xpDisplay.text);
    }


    private void ResetXPBar()
    {
        Debug.Log("Reseting XP Bar to empty");
        foreach (GameObject xpBar in xpBarElements)
        {
            Debug.Log("reseting " + xpBar.name);
            xpBar.GetComponent<Image>().fillAmount = 0;
        }
    }

    public void AwardXP(int xpReward)
    {
        int tempXPcalc = GameMainManager.Instance._playerCurrentLevelXP + xpReward;
        //check to see if with the current reward the player earns a level. If yes then 
        //Current Level is +1 and reward the difference of the XP.
        if (tempXPcalc >= GameMainManager.Instance._playerNextLevelXP)
        {
            Debug.Log("Reached enough XP to Level UP");
            int tempXPforNextLevel = tempXPcalc - GameMainManager.Instance._playerNextLevelXP;
            ResetXPBar();
            GameMainManager.Instance._playerCurrentLevel += 1;
            GameMainManager.Instance._playerCurrentLevelXP = 0;
            GameMainManager.Instance._playerNextLevelXP = GameMainManager.Instance._gameXPLevels[GameMainManager.Instance._playerCurrentLevel - 1];
            GameMainManager.Instance._playerCurrentLevelXP += tempXPforNextLevel;
            SetActiveXPBar();
            DisplayXPScore();
        }
        else
        {
            GameMainManager.Instance._playerCurrentLevelXP += xpReward;
            SetActiveXPBar();
            DisplayXPScore();
        }
       
    }
}
