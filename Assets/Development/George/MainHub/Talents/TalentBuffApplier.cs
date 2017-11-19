using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentBuffApplier : MonoBehaviour
{
    private static TalentBuffApplier talentBuffApplier;

    public static TalentBuffApplier instance
    {
        get
        {
            if (!talentBuffApplier)
            {
                talentBuffApplier = FindObjectOfType(typeof(TalentBuffApplier)) as TalentBuffApplier;
                if (!talentBuffApplier)
                {
                    Debug.LogError("There seems that you are missing an TalentBuffApplier script on a GameObject in your Scene");
                }
                else
                {
                    Debug.Log("TalentBuffAplier is loaded");
                    return talentBuffApplier;
                }
            }
            return talentBuffApplier;
        }
    }

    

}
