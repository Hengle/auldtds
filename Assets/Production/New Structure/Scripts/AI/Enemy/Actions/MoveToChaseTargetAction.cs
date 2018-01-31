using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/MoveToChaseTarget")]
public class MoveToChaseTargetAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		if(controller.enemyStats.isAlive)
		{
			MoveToTarget(controller);
		}
	}

	private void MoveToTarget(Enemy.StateController controller)
	{
		if(controller.chaseTarget != null)
		{
			controller.navMeshAgent.destination = controller.chaseTarget.position;
			controller.navMeshAgent.speed = controller.enemyStats.moveSpeed;
			controller.navMeshAgent.stoppingDistance = controller.enemyStats.attackRange;
			controller.navMeshAgent.isStopped = false;

			float minionVelocity;
			minionVelocity = controller.navMeshAgent.velocity.magnitude;
			controller.anim.SetFloat("MinionVelocity", minionVelocity);

			if(controller.chaseTarget == controller.blockItemPointTarget)
			{
				if (controller.blockItemTarget.GetComponent<BlockItem.StateController>().fullEngaged)
				{
					if(controller.IsFullEngagedBlockItemClose())
					{
						controller.navMeshAgent.speed = 0;

					}
					else
					{
						controller.navMeshAgent.speed = controller.enemyStats.moveSpeed;
					}
				}	
			}
		}
	}
}