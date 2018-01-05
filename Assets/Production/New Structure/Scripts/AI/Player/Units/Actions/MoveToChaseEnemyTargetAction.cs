using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/PlayerUnit/MoveToChaseTarget")]
public class MoveToChaseEnemyTargetAction : PlayerUnitAction
{
	public override void Act(PlayerUnit.StateController controller)
	{
		MoveToTarget(controller);
	}

	private void MoveToTarget(PlayerUnit.StateController controller)
	{
		if(controller.chaseTarget != null)
		{
			controller.navMeshAgent.destination = controller.chaseTarget.position;
			controller.navMeshAgent.speed = controller.playerUnitStats.moveSpeed;
			controller.navMeshAgent.stoppingDistance = controller.playerUnitStats.attackRange;
			controller.navMeshAgent.isStopped = false;

			float minionVelocity;
			minionVelocity = controller.navMeshAgent.velocity.magnitude;
			controller.anim.SetFloat("UnitVelocity", minionVelocity);
		}
	}
}