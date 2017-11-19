using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public string buffName;
    public int buffLvlReq;
    public bool buffIsStackable;

    /*
    public abstract void Initialize(GameObject obj);
    public abstract void TriggerBuff();
    public abstract IEnumerator EndBuff();
    */
}
