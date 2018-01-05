using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUnitDecision : ScriptableObject
{
	public abstract bool Decide (PlayerUnit.StateController controller);
}
