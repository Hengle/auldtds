using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentBuffSubscriber : MonoBehaviour
{
    private UnitAttributes unitAttributes;
    [SerializeField]
    private List<TalentGroupManager> groupManagers = new List<TalentGroupManager>();

    private void OnEnable()
    {
        groupManagers.Add(GameObject.Find("Group-01").GetComponent<TalentGroupManager>());
        groupManagers.Add(GameObject.Find("Group-02").GetComponent<TalentGroupManager>());
        groupManagers.Add(GameObject.Find("Group-03").GetComponent<TalentGroupManager>());

        unitAttributes = GetComponent<UnitAttributes>();
        if (unitAttributes.unitBaseAttributes.unitClass == unitClassType.Footman)
        {
            ApplyToughened();
            ApplyBloodStrike();
            TalentEventManager.OnToughened += ApplyToughened;
            TalentEventManager.OnBloodStrike += ApplyBloodStrike;
        }
    }

    private void OnDisable()
    {
        if (unitAttributes.unitBaseAttributes.unitClass == unitClassType.Footman)
        {
            //TalentEventManager.OnClicked -= TestTalent;
            TalentEventManager.OnToughened -= ApplyToughened;
        }
    }

    public void ApplyToughened()
    {
        Debug.Log("Toughened is Applied to ");
        BuffAbility buffAbility = new BuffAbility();
        foreach (TalentGroupManager talentGroup in groupManagers)
        {
            foreach (TalentMainClass item in talentGroup.buttonGroup)
            {
                if (item.talentButton.name == "Toughened")
                {
                    TalentAttributes toughenedAttr = item.talentButton.GetComponent<TalentAttributes>();
                    int buffRankIndex = toughenedAttr.talentRank - 1;
                    if (buffRankIndex >= 0)
                    {
                        buffAbility = (BuffAbility)toughenedAttr.buffObject[buffRankIndex];
                    }
                }
            }
        }

        UnitAttributes myUnitAttributes = GetComponent<UnitAttributes>();
        myUnitAttributes.unitBaseAttributes.unitArmor += Mathf.RoundToInt(buffAbility.buffAmount); 
    }

    public void ApplyBloodStrike()
    {
        Debug.Log("BloodAttack is Applied to ");
        BuffAbility buffAbility = new BuffAbility();
        foreach (TalentGroupManager talentGroup in groupManagers)
        {
            foreach (TalentMainClass item in talentGroup.buttonGroup)
            {
                if (item.talentButton.name == "BloodStrike")
                {
                    TalentAttributes bloodstrikeAttr = item.talentButton.GetComponent<TalentAttributes>();
                    int buffRankIndex = bloodstrikeAttr.talentRank - 1;
                    if (buffRankIndex >= 0)
                    {
                        buffAbility = (BuffAbility)bloodstrikeAttr.buffObject[buffRankIndex];
                    }
                }
            }
        }
        UnitAttributes myUnitAttributes = GetComponent<UnitAttributes>();
        myUnitAttributes.unitBaseAttributes.unitMinDamage += Mathf.RoundToInt(buffAbility.buffAmount);
        myUnitAttributes.unitBaseAttributes.unitMaxDamage += Mathf.RoundToInt(buffAbility.buffAmount); 
    }
}
