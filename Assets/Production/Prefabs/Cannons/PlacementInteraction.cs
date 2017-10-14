using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementInteraction : MonoBehaviour
{

    public bool isSelected = false;
    public GameObject cannonRangeIndicator;

    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isSelected)
        {
            Renderer cannonRangeRenderer = cannonRangeIndicator.GetComponent<Renderer>();
            cannonRangeRenderer.enabled = false;
        }
	}

    private void OnMouseDown()
    {
        isSelected = !isSelected;
    }
}
