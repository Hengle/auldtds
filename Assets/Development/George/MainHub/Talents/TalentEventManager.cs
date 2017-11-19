using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentEventManager : MonoBehaviour
{

    public delegate void Toughened();
    public static event Toughened OnToughened;

    public delegate void BloodStrike();
    public static event BloodStrike OnBloodStrike;


    public void TriggerToughened()
    {
        if (OnToughened != null)
        {
            OnToughened();
        }
    }

    public void TriggerBloodStrike()
    {
        if (OnBloodStrike != null)
        {
            OnBloodStrike();
        }
    }
}
