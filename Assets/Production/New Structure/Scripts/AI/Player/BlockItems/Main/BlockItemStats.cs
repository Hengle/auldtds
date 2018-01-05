using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementTag {None, DoorPlacement};

[CreateAssetMenu(menuName = "AI/Attributes/BlockItemAttributes")]
public class BlockItemStats: ScriptableObject 
{
	[Header("Main")]
	public string name;
	public GameObject mouseTip;

	[Header("Vitality")]
	public bool isAlive;
	public int currentHealth;
	public int totalHealth;

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
}
