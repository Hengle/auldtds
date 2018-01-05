using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {Minion, Boss}
[CreateAssetMenu(menuName = "AI/Attributes/EnemyAttributes")]
public class EnemyStats : ScriptableObject
{
	[Header("Main")]
	public string name;
	public EnemyType enemyType;

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

	[Header("RoomLoot")]
	public int lootCapacity;

	[Header("EyeSight")]
	public float lookSphereCastRadius;
	public float eyeSight;

	[Header("Award")]
	public int AwardExp;
	public int AwardGold;

	[Header("Locked")]
	public float lockedOnUnitDistance;
	public float closeToFullEngagedBlockItem;
}
