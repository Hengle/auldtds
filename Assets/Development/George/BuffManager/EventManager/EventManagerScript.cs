using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerScript : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    public void ButtonBehavior()
    {
        if (OnClicked != null)
        {
            OnClicked();
        }
    }
    
}
