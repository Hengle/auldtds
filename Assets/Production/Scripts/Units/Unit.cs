using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public GameObject unitEnemy;
    private MinionAttributes enemyAttributes;
    public int unitBaseArmor = 10;

	// Use this for initialization
	
    
    

    public void DoDamage()
    {
        //Find the enemy
        unitEnemy = GetComponentInChildren<UnitThreatArea>().unitTarget;

        //Get Current Enemy Attributes
        enemyAttributes = unitEnemy.GetComponent<MinionAttributes>();
        int toHitScore = unitBaseArmor + enemyAttributes.minionAttributes.unitArmor;

        if (enemyAttributes.minionAttributes.unitHealthPoints <=0)
        {
            //Debug.Log("Enemy is DEAD");
            this.GetComponent<UnitAnimations>().animStateIdle = true;
            this.GetComponent<UnitAnimations>().animStateAttack = false;
        }
        else
        {
            //Get the main attributer as they Currently are.
            int damageMin = GetComponent<UnitAttributes>().unitAttributes.unitMinDamage;
            int damageMax = GetComponent<UnitAttributes>().unitAttributes.unitMaxDamage;
            int toHit = GetComponent<UnitAttributes>().unitAttributes.unitToHitScore;
            int critChance = GetComponent<UnitAttributes>().unitAttributes.unitCritScore;
            int critMult = GetComponent<UnitAttributes>().unitAttributes.unitCritMultiplier;
            int critScore = 20 - critChance;
            //Roll the Chance to hit.
            int toHitRoll = (Random.Range(1, 21) + toHit);

            if ((toHitRoll >= toHitScore))
            {
                if ((toHitRoll - toHit) >= critScore)
                {

                    //Roll the Damage
                    int damageRoll = (Random.Range(damageMin, damageMax + 1) * critMult);
                    //Tell Current enemy to take damage
                    unitEnemy.GetComponent<MinionDamages>().TakeDamage(damageRoll, true);
                }
                else
                {

                    //Roll the Damage
                    int damageRoll = Random.Range(damageMin, damageMax + 1);
                    //Tell Current enemy to take damage
                    unitEnemy.GetComponent<MinionDamages>().TakeDamage(damageRoll, false);
                }
            }
            else if ((toHitRoll - toHit) < toHitScore)
            {

                string damageRoll = "Miss";
                //Tell Current enemy to take damage
                unitEnemy.GetComponent<MinionDamages>().MissDamage(damageRoll, true);
            }
        }
    }
}
