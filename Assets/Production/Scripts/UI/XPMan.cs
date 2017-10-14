using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPMan : MonoBehaviour
{
    private static XPMan instance;
    public static XPMan Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<XPMan>();
            }
            return instance;
        }
    }


    public GameObject[] xpBarElements;
    public GameObject activeXPBarObject;
    public int currentXPBarNumber = 0;
    public Text xpText;

    public int playerLevel = 0;
    public int currentXP;
    public int nextLevelXP;

    public int numberOfXPBars;
    public float xpPerBar;
    float fillAmount = 0;


   public void LevelUp()
    {
        GameMainManager.Instance._playerCurrentLevel += 1;
        currentXPBarNumber = 0;
        activeXPBarObject = xpBarElements[0];
    }

    public void AwardXP(int xp)
    {
        int xpCalc = GameMainManager.Instance._playerCurrentLevelXP += xp;
        if (xpCalc >= GameMainManager.Instance._playerNextLevelXP)
        {
            //int newXP = GameMainManager.Instance._playerNextLevelXP - xp
            LevelUp();
            ResetXPBar();
        }
        if (xpCalc < GameMainManager.Instance._playerNextLevelXP)
        {
            GameMainManager.Instance._playerCurrentLevelXP += xp;
        }
    }

    private void ResetXPBar()
    {
        foreach (GameObject xpBar in xpBarElements)
        {
            xpBar.GetComponent<Image>().fillAmount = 0;
        }
    }



}
