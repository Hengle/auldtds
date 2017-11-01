using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff : MonoBehaviour {

    public List<BuffBase> buffList = new List<BuffBase>();

    private BuffBase.BuffType buffType;
    private UnitAttributes unitAttributes;

    [SerializeField]
    private int damageBuff;
    [SerializeField]
    private int unitMinDamage;
    [SerializeField]
    private int unitMaxDamage;

    // Use this for initialization
    void Start ()
    {
        unitAttributes = GetComponent<UnitAttributes>();
        unitMinDamage = DefaultMinDamage();
        unitMaxDamage = DefaultMaxDamage();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckDamageBuff();
	}


    private int DefaultMinDamage()
    {
        int minDamage = unitAttributes.unitBaseAttributes.unitMinDamage;
        return minDamage;
    }

    private int DefaultMaxDamage()
    {
        int maxDamage = unitAttributes.unitBaseAttributes.unitMaxDamage;
        return maxDamage;
    }

    private void CheckDamageBuff()
    {
        //int damageBuff = 0;
        damageBuff = 0;
        foreach (BuffBase damage in buffList)
        {
            if (buffType == BuffBase.BuffType.Damage)
            {
                damageBuff += damage.buffAmount;
            }
            else
            {
                Debug.Log("NO Damage Buff detected");
            }
        }
        unitAttributes.unitBaseAttributes.unitMinDamage += damageBuff;
        unitMinDamage += damageBuff;
        unitMaxDamage += damageBuff;
    }
}
