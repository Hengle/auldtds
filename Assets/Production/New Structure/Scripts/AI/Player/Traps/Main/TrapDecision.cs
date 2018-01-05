using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapDecision : ScriptableObject
{
	public abstract bool Decide (Trap.StateController controller);
}