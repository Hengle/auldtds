using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUnitTransition
{
	public PlayerUnitDecision decision;
	public PlayerUnitState trueState;
	public PlayerUnitState falseState;
}
