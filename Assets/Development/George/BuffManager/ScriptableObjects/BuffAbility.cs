using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buffs/New Buff", order = 1)]
public class BuffAbility : Buff
{
    public enum BuffElement {ATTHIT, ATTSPEED, ATTDMG, DEFAC, DEFNAT };
    public BuffElement buffElement;
    public float buffAmount;
    public float buffDuration;

    private UnitAttributes unitAttributes;


    /*
    public override void Initialize(GameObject obj)
    {
        unitAttributes = obj.GetComponent<UnitAttributes>();
        Debug.Log("Init");
    }

    public override void TriggerBuff()
    {
        Debug.Log("Triggered");
    }

    public override IEnumerator EndBuff()
    {
        yield return new WaitForSeconds(buffDuration);
        Destroy(this);
    }
    */
}
