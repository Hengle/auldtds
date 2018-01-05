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

	[Header("EyeSight")]
	public float lookSphereCastRadius;
}
