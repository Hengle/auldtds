using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{

    public GameObject selectedUnit;
    MouseManager myMouseManager;

    public Vector3 newPosition;
    public Vector3 curPosition;
    public float unitSpeed;
    public float unitRotationSpeed;

    public bool unitMoving = false;


	// Use this for initialization
	void Start ()
    {
        myMouseManager = FindObjectOfType<MouseManager>();
        selectedUnit = myMouseManager.selectedObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        selectedUnit = myMouseManager.selectedObject;
        if (selectedUnit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //unitMoving = true;
                GetUnitTargetPosition();
            }

            MoveUnit(selectedUnit,newPosition);
        }
    }

    public void GetUnitTargetPosition()
    {
        Ray unitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPosition;
        if (Physics.Raycast(unitRay, out hitPosition))
        {
            newPosition = hitPosition.point;
        }
    }

    public void MoveUnit(GameObject unitObject, Vector3 targetPosition)
    {
        //Debug.Log("Calculating new position");
        curPosition = this.transform.position;
        
        Vector3 targetDirection = (targetPosition - selectedUnit.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        selectedUnit.transform.position = Vector3.MoveTowards(selectedUnit.transform.position, targetPosition, Time.deltaTime * unitSpeed);
        //selectedUnit.transform.rotation = Quaternion.Slerp(selectedUnit.transform.rotation, lookRotation, Time.deltaTime * unitRotationSpeed);
        selectedUnit.transform.rotation = Quaternion.Slerp(selectedUnit.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * unitRotationSpeed);
        //selectedUnit.transform.LookAt(targetPosition);

    }
}
