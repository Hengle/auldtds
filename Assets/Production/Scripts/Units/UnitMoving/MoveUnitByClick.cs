using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveUnitByClick : MonoBehaviour {
#region Properties
	[Header("PlayerSelection")]
	public bool isPlayerSelected;
    
    [Header("NavMeshMovement")]
	private NavMeshAgent navAgent;
	private Transform destinationTarget;
	public LayerMask movementLayer;
	#endregion

#region System Functions
	// Use this for initialization
	void Start () 
	{
        transform.SetParent(GameObject.Find("HeroUnits").transform);
        navAgent = GetComponent<NavMeshAgent>();
        UnitAnimations unitAnimations = GetComponent<UnitAnimations>();
        isPlayerSelected = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        CheckUnitSelected();
        MoveByClicking();
        if (CheckIfUnitReachPoint())
        {
            UnitAnimations unitAnimations = GetComponent<UnitAnimations>();
            unitAnimations.animStateMove = false;
            unitAnimations.animStateCharging = false;
            if (unitAnimations.animStateAttack == false)
            {
                unitAnimations.animStateIdle = true;
            }
            else
            {
                unitAnimations.animStateIdle = false;
            }
            
        }
        

    }
#endregion

#region NavMesh Functions
	private void MoveByClicking()
	{
		if(isPlayerSelected)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit Hit;
			if (Physics.Raycast(ray, out Hit, 1000, movementLayer))
			{
				if(Input.GetButtonDown("Fire2") && !Input.GetKey(KeyCode.LeftControl))  //[GA]Changed to Fire2 because FIRE1 is used to deselect the unit.
				{
					navAgent.SetDestination(Hit.point);
                    UnitAnimations unitAnimations = GetComponent<UnitAnimations>();
                    navAgent.speed = 3.5f;
                    //Debug.Log("Firing walking animation");
                    unitAnimations.animStateIdle = false;
                    unitAnimations.animStateAttack = false;
                    unitAnimations.animStateCharging = false;
                    unitAnimations.animStateMove = true;
				}
                if (Input.GetButtonDown("Fire2") && Input.GetKey(KeyCode.LeftControl))  //[GA]Added Charging.
                {
                    //Debug.Log("Charging YEYY");
                    navAgent.SetDestination(Hit.point);
                    navAgent.speed = 7;
                    UnitAnimations unitAnimations = GetComponent<UnitAnimations>();
                    //Debug.Log("Firing Charging animation");
                    unitAnimations.animStateIdle = false;
                    unitAnimations.animStateMove = false;
                    unitAnimations.animStateAttack = false;
                    unitAnimations.animStateCharging = true;

                }


            }
		}
	}
#endregion

#region [GA]
    private void CheckUnitSelected()
    {
        SelectableUnitComponent selectableComponent = GetComponent<SelectableUnitComponent>();
        isPlayerSelected = selectableComponent.unitSelected;
    }

    private bool CheckIfUnitReachPoint()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (Vector3.Distance(this.gameObject.transform.position, navAgent.destination)<=1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
#endregion

}
