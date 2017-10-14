using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{

    private Animator unitAnimator;
    public bool animStateIdle = true;
    public bool animStateAttack = false;
    public bool animStateMove = false;
    public bool animStateCharging = false;
    public bool animStateDeath = false;

    // Use this for initialization
    void Start ()
    {
        unitAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (animStateAttack == true)
        {
            unitAnimator.SetBool("Idle", false);
            unitAnimator.SetBool("Walking", false);
            unitAnimator.SetBool("Charging", false);
            unitAnimator.SetBool("Attacking", true);
        }
        else if (animStateIdle == true)
        {
            unitAnimator.SetBool("Attacking", false);
            unitAnimator.SetBool("Walking", false);
            unitAnimator.SetBool("Charging", false);
            unitAnimator.SetBool("Idle", true);
        }
        else if (animStateMove == true)
        {
            unitAnimator.SetBool("Idle", false);
            unitAnimator.SetBool("Attacking", false);
            unitAnimator.SetBool("Charging", false);
            unitAnimator.SetBool("Walking", true);
        }
        else if (animStateCharging == true)
        {
            unitAnimator.SetBool("Idle", false);
            unitAnimator.SetBool("Attacking", false);
            unitAnimator.SetBool("Walking", false);
            unitAnimator.SetBool("Charging", true);
            
        }
        else if (animStateDeath == true)
        {
            unitAnimator.SetBool("Idle", false);
            unitAnimator.SetBool("Attacking", false);
            unitAnimator.SetBool("Walking", false);
            unitAnimator.SetBool("Charging", false);
            unitAnimator.SetBool("Death", true);

        }
    }
}
