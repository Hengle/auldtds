using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum unitClassType { Footman, Archer, Healer, Hero, Minion, Blocking };
public enum PlacementTags {None, DoorPlacement};

[System.Serializable]
public class UnitBaseClass
{
    public int unitID;
    public unitClassType unitClass;
    [Header("Unit's basic Attributes")]
    public bool unitIsAlive = true;
    public int unitCost;
	public int unitCostMithril;
    public int unitLevel;
    public bool heroUnit;
    public int unitThreatFactor;
    public int unitEXPValue;
    public int unitTreasureFactor;
    public float unitDespawnTime;

	public LayerMask unitSpawnLayer;
	public bool isForSpecificPlacement;
	public PlacementTags placementTag;
	public GameObject mouseTipItem;

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

	public int unitEnemyEngagePoints;
	public float unitSpeed;

}
