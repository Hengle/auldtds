using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDamages : MonoBehaviour
{
    public float textYOffset = 0;
    private UnitAttributes unitAttributes;
    //private UnitAnimationPlayer unitAnimPlayer;
    public bool dotEffect = false;
    private Animator animator;

	// Use this for initialization
	void Start ()
    {
		unitAttributes = this.GetComponent<UnitAttributes>();
        animator = this.GetComponent<Animator>();
        //unitAnimPlayer = this.GetComponent<UnitAnimationPlayer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!unitAttributes.unitBaseAttributes.unitIsAlive)
        {
            StopDot();
        }
	}

    public void TakeDamage(int damage, bool critical)
    {
        CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
        if (unitAttributes.unitBaseAttributes.unitIsAlive)
        {
            animator.SetTrigger("GetHit");
            unitAttributes.unitBaseAttributes.unitHealthPoints -= damage;
        }
     }

    public void MissDamage(string miss, bool critical)
    {
        CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
        if (unitAttributes.unitBaseAttributes.unitIsAlive)
        {
            animator.SetTrigger("Block");
        }
    }


    public void TriggerDOT(int dotValue, bool dotCrit, float dotT)
    {
        //Debug.Log("Triggering DOT");
        StartCoroutine(TakeDamageOverTime(dotValue, dotCrit, dotT));
    }

    public void StopDot()
    {
        //Debug.Log("Stop DOT");
        StopAllCoroutines();
    }

    IEnumerator TakeDamageOverTime(int damage, bool critical, float t)
    {
        while (true)
        {
           //Debug.Log("DOT Effect");
            TakeDamage(damage, critical);

            yield return new WaitForSeconds(t);
        }
    }
}
