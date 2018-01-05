using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Trap/isTriggered")]
public class IsTriggeredDecision : TrapDecision
{
	public override bool Decide(Trap.StateController controller)
	{
		bool isTrapTriggered = CheckIfTrapIsTriggered(controller);
		return isTrapTriggered;
	}

	private bool CheckIfTrapIsTriggered(Trap.StateController controller)
	{
		return true;
	}
}