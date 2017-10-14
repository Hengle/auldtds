using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum unitClassType { Footman, Archer, Healer, Hero, Minion, Blocking };

[System.Serializable]
public class UnitBaseClass
{
    public int unitID;
    public unitClassType unitClass;
    [Header("Unit's basic Attributes")]
    public bool unitIsAlive = true;
    //public string unitPrefix;
    public int unitCost;
    public int unitLevel;
    public bool heroUnit;
    public int unitThreatFactor;
    public int unitEXPValue;
    public int unitTreasureFactor;


    [Header("Unit's Main Abilities")]
    [Range(1, 50)]
    public int unitMinDamage;
    [Range(1, 50)]
    public int unitMaxDamage;
    
    public float unitCDScore;
    public int unitToHitScore;
    public int unitCritScore;
    public int unitCritMultiplier;

    public int unitArmor;
    public int unitBaseAC;
    public int unitHealthPoints;
    public int unitTotalHealthPoints;

    public int unitManaPoints;
    public int unitManaRegen;
    public int unitHealthRegen;

   


}
