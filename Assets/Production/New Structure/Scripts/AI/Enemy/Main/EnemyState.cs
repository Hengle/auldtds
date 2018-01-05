using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/State/EnemyState")]
public class EnemyState : ScriptableObject
{
	public EnemyAction[] actions;
	public EnemyTransition[] transitions;
	public Color sceneGizmoColor = Color.grey;

	public void UpdateState(Enemy.StateController controller)
	{
		DoActions(controller);
		CheckTransitions(controller);
	}
		
	private void DoActions(Enemy.StateController controller)
	{
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].Act(controller);
		}
	}
		
	private void CheckTransitions(Enemy.StateController controller)
	{
		for(int i = 0; i < transitions.Length; i++)
		{
			bool decisionSucceeded = transitions[i].decision.Decide(controller);

			if(decisionSucceeded)
			{
				controller.TransitionToState(transitions[i].trueState);
			}
			else
			{
				controller.TransitionToState(transitions[i].falseState);
			}
		}
	}
}
