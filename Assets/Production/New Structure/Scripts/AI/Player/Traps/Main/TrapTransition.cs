using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapTransition
{
	public TrapDecision decision;
	public TrapState trueState;
	public TrapState falseState;
}
