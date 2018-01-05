using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/State/PlayerUnitState")]
public class PlayerUnitState : ScriptableObject
{
	public PlayerUnitAction[] actions;
	public PlayerUnitTransition[] transitions;
	public Color sceneGizmoColor = Color.grey;

	public void UpdateState(PlayerUnit.StateController controller)
	{
		DoActions(controller);
		CheckTransitions(controller);
	}

	private void DoActions(PlayerUnit.StateController controller)
	{
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].Act(controller);
		}
	}

	private void CheckTransitions(PlayerUnit.StateController controller)
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
