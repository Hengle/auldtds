using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanelHandle : MonoBehaviour
{
    public enum DEFAULT_STATE { ON, OFF};
    public DEFAULT_STATE achievementPanelState;
    private bool achievPanelIsOn = false;
    public GameObject achievementPanel;
    private AchievementManager achMan;
    private bool initiatePanel = false;

	// Use this for initialization
	void Start ()
    {
        if (achievementPanelState == DEFAULT_STATE.OFF)
        {
            achievPanelIsOn = false;
        }
        else
        {
            achievPanelIsOn = true;
        }
        if (!achievPanelIsOn)
        {
            achievementPanel.SetActive(false);
        }
        achMan = this.GetComponent<AchievementManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (achievPanelIsOn)
        {
            OpenAchievPanel();
        }
        CheckKeyOpenAchievements();
    }

    public void CloseAchievPanel()
    {
        achievementPanel.SetActive(false);
        achievPanelIsOn = false;
    }

    public void OpenAchievPanel()
    {
        achievementPanel.SetActive(true);
        achievPanelIsOn = true;
        if (!initiatePanel)
        {
            achMan.CreateAchievementList();
            initiatePanel = true;
        }
    }

    private void CheckKeyOpenAchievements()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            achievPanelIsOn = !achievPanelIsOn;
            if (achievPanelIsOn)
            {
                OpenAchievPanel();
                if (!initiatePanel)
                {
                    achMan.CreateAchievementList();
                    initiatePanel = true;
                }
            }
            else
            {
                CloseAchievPanel();
            }
        }
    }
}
