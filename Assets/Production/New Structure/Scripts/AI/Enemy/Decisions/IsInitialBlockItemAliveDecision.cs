using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isInitialBlockItemAlive")]
public class IsInitialBlockItemAliveDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isInitialBlockItemTarget = CheckInitialBlockItemTarget(controller);
		return isInitialBlockItemTarget ;
	}

	private bool CheckInitialBlockItemTarget(Enemy.StateController controller)
	{
		if(controller.blockItemTarget)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}