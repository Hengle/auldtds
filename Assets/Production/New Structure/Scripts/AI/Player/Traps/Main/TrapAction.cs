using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapAction : ScriptableObject
{
	public abstract void Act (Trap.StateController controller);
}