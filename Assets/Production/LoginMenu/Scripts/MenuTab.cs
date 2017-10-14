using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;


public class MenuTab : MonoBehaviour
{
    public GameObject[] uiElements;
    private int currentFocus = 0;
    private int totalUIelements;

    private string error_passwordMismatch = "Password Mismatch. Please confirm your password";
    private string error_emailInvalid = "Your designated e-mail address is not valid";
    public List<string> error_messages = new List<string>();


    // Use this for initialization
    void Start ()
    {
        totalUIelements = uiElements.Length;
        SelectUIElement(uiElements[currentFocus]);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift))
        {
            if (currentFocus < (totalUIelements-1))
            {
                currentFocus += 1;
                SelectUIElement(uiElements[currentFocus]);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            if (currentFocus > 0)
            {
                currentFocus -= 1;
                SelectUIElement(uiElements[currentFocus]);
            }
        }
    }

    public void SelectUIElement(GameObject uiObject)
    {
        if (uiObject.tag == "UIButton")
        {
            uiObject.GetComponent<Button>().Select();
        }
        else
        {
            uiObject.GetComponent<InputField>().Select();
            uiObject.GetComponent<InputField>().ActivateInputField();
        }
    }

    public void ValidateLoginInputFields()
    {
        bool errorsExist = false;
        foreach (GameObject uiItem in uiElements)
        {
            if (uiItem.name == "Login-Username")
            {
                if (uiItem.GetComponent<InputField>().text.Length <=0)
                {
                    error_messages.Add("Please specify a valid Username");
                    errorsExist = true;
                }
            }
            if (uiItem.name == "Login-Password")
            {
                if (uiItem.GetComponent<InputField>().text.Length <= 0)
                {
                    error_messages.Add("Please specify a valid Password");
                    errorsExist = true;
                }
            }
        }
        if (errorsExist)
        {
            Debug.Log("Errors Exist");
        }
    }
}
