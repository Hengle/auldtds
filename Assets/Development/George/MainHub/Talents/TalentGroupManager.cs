using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentGroupManager : MonoBehaviour
{

    private static TalentGroupManager talentGroupManager;

    public static TalentGroupManager instance
    {
        get
        {
            if (!talentGroupManager)
            {
                talentGroupManager = FindObjectOfType(typeof(TalentGroupManager)) as TalentGroupManager;
                if (!talentGroupManager)
                {
                    Debug.LogError("There seems that you are missing an EventManager script on a GameObject in your Scene");
                }
                else
                {
                    return talentGroupManager;
                }
            }
            return talentGroupManager;
        }
    }


    [SerializeField]
    private int maxTalentsAllowed;
    public int curTalentsChosen;
    [SerializeField]
    private List<TalentMainClass> buttonGroup = new List<TalentMainClass>();
    private Text buttonText;

    	
	// Update is called once per frame
	void Update ()
    {
        CheckTalentGroupStatus();
    }

    public void AddTalentRank(int btnIndex)
    {

        buttonGroup[btnIndex].chosen = true;
        TalentAttributes talentAttr =  buttonGroup[btnIndex].talentButton.GetComponent<TalentAttributes>();
        if ((talentAttr.talentRank <= talentAttr.talentTotalRanks) && (curTalentsChosen <= maxTalentsAllowed ))
        {
            int talentRankCalc = talentAttr.talentRank + 1;
            talentAttr.talentRank = talentRankCalc;
            GameObject pressedButton = buttonGroup[btnIndex].talentButton;
            buttonText = pressedButton.GetComponentInChildren<Text>();
            buttonText.text = talentRankCalc.ToString();
            curTalentsChosen++;
        }
    }

    public void RemoveTalentRank(int btnIndex)
    {

        buttonGroup[btnIndex].chosen = true;
        TalentAttributes talentAttr = buttonGroup[btnIndex].talentButton.GetComponent<TalentAttributes>();
        if (talentAttr.talentRank < talentAttr.talentTotalRanks)
        {
            int talentRankCalc = talentAttr.talentRank - 1;
            talentAttr.talentRank = talentRankCalc;
            GameObject pressedButton = buttonGroup[btnIndex].talentButton;
            buttonText = pressedButton.GetComponentInChildren<Text>();
            buttonText.text = talentRankCalc.ToString();
            curTalentsChosen++;
        }
    }

    private void CheckTalentGroupStatus()
    {
        

        if (curTalentsChosen >= maxTalentsAllowed)
        {
            //disable all non chosen Buttons
            foreach (TalentMainClass talentBtn in buttonGroup)
            {
                if (!talentBtn.chosen)
                {
                    talentBtn.talentButton.GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            foreach (TalentMainClass talentBtn in buttonGroup)
            {
                if (!talentBtn.chosen)
                {
                    talentBtn.talentButton.GetComponent<Button>().interactable = true;
                }
            }
        }
    }
}
