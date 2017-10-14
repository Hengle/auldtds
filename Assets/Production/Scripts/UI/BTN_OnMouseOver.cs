using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BTN_OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject tooltip;
    public bool activeTooltip = false;
    public float xOffset;
    public float yOffset;
   

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!activeTooltip)
        {
            activeTooltip = true;
            tooltip.SetActive(true);
            tooltip.transform.position = new Vector3((Input.mousePosition.x + xOffset), (Input.mousePosition.y+yOffset),0);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeTooltip)
        {
            activeTooltip = false;
            tooltip.SetActive(false);
        }
    }
}
