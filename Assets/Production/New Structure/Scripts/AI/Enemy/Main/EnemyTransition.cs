using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTransition
{
	public EnemyDecision decision;
	public EnemyState trueState;
	public EnemyState falseState;
}
