using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUnitAction : ScriptableObject
{
	public abstract void Act (PlayerUnit.StateController controller);
}