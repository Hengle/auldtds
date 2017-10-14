using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    [SerializeField]
    private GameObject registrationObject;
    [SerializeField]
    private GameObject loginObject;
    [SerializeField]
    private GameObject promptPanel;
    [SerializeField]
    private string registrationURL;

    
	// Use this for initialization
	void Start ()
    {
        registrationObject.SetActive(false);
        loginObject.SetActive(true);
        promptPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SwitchMenus()
    {
        if (registrationObject.activeSelf == true)
        {
            registrationObject.SetActive(false);
            loginObject.SetActive(true);
        }
        else
        {
            loginObject.SetActive(false);
            registrationObject.SetActive(true);
        }
    }

    public void CloseErrorPrompt()
    {
        promptPanel.SetActive(false);
    }

    public void OpenRegistrationURL(string url2go)
    {
        Application.OpenURL(url2go);
    }

}
