using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase
{
    public string buffName;
    public enum BuffType  {Damage, AttackSpeed, Health, ToHit, CritScore, CritMultiplier, Armor};
    public BuffType buffType;
    public int buffAmount;
    public float buffDuration;
    public bool buffPermanent;
    public string functionName;
    
    
}
