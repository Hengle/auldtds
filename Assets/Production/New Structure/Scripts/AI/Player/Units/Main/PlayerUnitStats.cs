using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerUnitType {Hero, Melee, Range, Caster}
[CreateAssetMenu(menuName = "AI/Attributes/PlayerUnitAttributes")]
public class PlayerUnitStats : ScriptableObject
{
	[Header("Main")]
	public string name;
	public PlayerUnitType playerUnitType;
	public GameObject mouseTip;

	[Header("Vitality")]
	public bool isAlive;
	public int currentHealth;
	public int totalHealth;

	[Header("Move")]
	public float moveSpeed;

	[Header("Attack")]
	public float attackSpeed;
	public float attackRange;
	public int minDamage;
	public int maxDamage;
	public int toHitScore;
	public int critScore;
	public int critMultiplier;

	[Header("Armor")]
	public int baseArmor;
	public int unitArmor;

	[Header("Engaged")]
	public int totalEngagedEnemies;

	[Header("Placement")]
	public bool isForSpecificLayer;
	public LayerMask whereToSpawnLayer;
	public PlacementTag placementTag;

	[Header("Costs")]
	public int coinCost;
	public int mithrilCost;

	[Header("EyeSight")]
	public float lookSphereCastRadius;
	public float eyeSight;

	[Header("LockedOnUnit")]
	public float lockedOnUnitDistance;
}
