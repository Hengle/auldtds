using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    public GameObject selectedObject;
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left Click detected");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag == "RTSUnit")
                {
                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    Debug.Log("Ray " + hitObject.name);
                    SelectObject(hitObject);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Right Click detected");
            ClearSelection();
        }
	}

    void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            if (obj == selectedObject)
                return;
            ClearSelection();
        }
        selectedObject = obj;
        Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = Color.green;
            r.material = m;
        }
    }


    void ClearSelection()
    {
        if (selectedObject == null)
        {
            return;
        }
        Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = Color.white;
            r.material = m;
        }
        selectedObject = null;
    }
}
