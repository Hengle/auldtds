using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public GameObject unitEnemy;
    private UnitAttributes unitAttributes;

    private UnitAttributes enemyAttributes;
    public int unitBaseArmor = 10;

    private AudioSource audioSource;
    private Animator unitAnimator;

    // Use this for initialization

    private void Start()
    {
        unitAttributes = GetComponent<UnitAttributes>();
        unitAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        CheckUnitIsAlive();
    }

    public void DoDamage()
    {
        //Find the enemy
        unitEnemy = GetComponentInChildren<UnitThreatArea>().unitTarget;

        //Get Sound Source and play Sound

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        //Get Current Enemy Attributes
        enemyAttributes = unitEnemy.GetComponent<UnitAttributes>();
        int toHitScore = unitBaseArmor + enemyAttributes.unitBaseAttributes.unitArmor;

        if (enemyAttributes.unitBaseAttributes.unitHealthPoints <=0)
        {
            //Debug.Log("Enemy is DEAD");
            //this.GetComponent<UnitAnimations>().animStateIdle = true;
            //this.GetComponent<UnitAnimations>().animStateAttack = false;
            
        }
        else
        {
            //Get the main attributer as they Currently are.
            int damageMin = GetComponent<UnitAttributes>().unitBaseAttributes.unitMinDamage;
            int damageMax = GetComponent<UnitAttributes>().unitBaseAttributes.unitMaxDamage;
            int toHit = GetComponent<UnitAttributes>().unitBaseAttributes.unitToHitScore;
            int critChance = GetComponent<UnitAttributes>().unitBaseAttributes.unitCritScore;
            int critMult = GetComponent<UnitAttributes>().unitBaseAttributes.unitCritMultiplier;
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

    private void CheckUnitIsAlive()
    {
        if (unitAttributes.unitBaseAttributes.unitHealthPoints <= 0 && unitAttributes.unitBaseAttributes.unitIsAlive == true)
        {
            //Debug.Log("AAARRRGG i am dying");
            unitAttributes.unitBaseAttributes.unitIsAlive = false;

            unitAnimator.SetTrigger("DeathTrigger");
            StartCoroutine(DestroyOnDeath());
        }
    }

    private IEnumerator DestroyOnDeath()
    {
        yield return new WaitForSeconds(unitAttributes.unitBaseAttributes.unitDespawnTime);
        Destroy(gameObject);
    }
}
