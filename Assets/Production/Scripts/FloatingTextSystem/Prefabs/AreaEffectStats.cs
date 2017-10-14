using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum areaEffectType { HoT, DoT};

public class AreaEffectStats : MonoBehaviour
{

    public areaEffectType effectType;
    [Range(1,50)]
    public int effectMinScore;
    [Range(1, 50)]
    public int effectMaxScore;

    public int effectCDScore;

    public int effectDuration;	
}
