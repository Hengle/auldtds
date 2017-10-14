using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UnitSelectionComponent : MonoBehaviour
{
    bool isSelecting = false;
    Vector3 mousePosition1;
    bool singleUnitSelection = false;

    //public GameObject selectionCirclePrefab;

    void Update()
    {
        SelectMultipleUnits();
        //SelectSingleUnit();
    }

    public void SelectMultipleUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;

            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (selectableObject.unitSelected == true)
                {
                    selectableObject.baseUnitObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
                    selectableObject.unitSelected = false;
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0) && (singleUnitSelection == false))
        {
            var selectedObjects = new List<SelectableUnitComponent>();
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectedObjects.Add(selectableObject);
                }
            }
            //Debug.Log("Ooops");
            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if (isSelecting)
        {
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (selectableObject.unitSelected == false)
                    {
                        selectableObject.baseUnitObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.1f);
                        selectableObject.unitSelected = true;
                    }
                }
                else
                {
                    if (selectableObject.unitSelected == true)
                    {
                        selectableObject.baseUnitObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
                        selectableObject.unitSelected = false;
                    }
                }
            }
        }
    }


    /*
    public void SelectSingleUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Select Single Unit");
            Vector3 mousePositionForUnit = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePositionForUnit);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "RTSUnit")
                {
                    //isSelecting = true;
                    singleUnitSelection = true;
                    Debug.Log(hit.collider.tag);
                    SelectableUnitComponent selectableObject = hit.collider.gameObject.GetComponent<SelectableUnitComponent>();
                    selectableObject.baseUnitObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.1f);
                    selectableObject.unitSelected = true;
                }
            }
        }
    }*/

    public bool IsWithinSelectionBounds( GameObject gameObject )
    {
        if( !isSelecting )
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds( camera, mousePosition1, Input.mousePosition );
        return viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
    }

    void OnGUI()
    {
        if( isSelecting )
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect( mousePosition1, Input.mousePosition );
            Utils.DrawScreenRect( rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
            Utils.DrawScreenRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ) );
        }
    }
}