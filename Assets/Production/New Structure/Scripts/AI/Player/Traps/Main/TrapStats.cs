using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType {Physical, Magic, Gas, Poison}
[CreateAssetMenu(menuName = "AI/Attributes/TrapAttributes")]
public class TrapStats : ScriptableObject
{
	[Header("Main")]
	public string name;
	public TrapType enemyType;

    [Header("Vitality")]
    public bool isAlive;
    public int currentHealth;
    public int totalHealth;

    [Header("Attack")]
    public float attackSpeed;
    public float attackRange;
    public int minDamage;
    public int maxDamage;
    public int critScore;
    public int critMultiplier;

    [Header("Armor")]
    public int baseArmor;
    public int unitArmor;

    [Header("Placement")]
    public bool isForSpecificLayer;
    public LayerMask whereToSpawnLayer;
    public PlacementTag placementTag;

    [Header("Costs")]
    public int coinCost;
    public int mithrilCost;

    [Header("EyeSight")]
	public float lookSphereCastRadius;
}
