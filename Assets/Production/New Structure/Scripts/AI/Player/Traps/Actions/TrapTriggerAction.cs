using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Traps/BasicAttack")]
public class TrapTriggerAction : TrapAction
{

    public override void Act(Trap.StateController controller)
    {
        TrapAttack(controller);
    }

    private void TrapAttack(Trap.StateController controller)
    {
        controller.TrapTrigger();
    }
}
