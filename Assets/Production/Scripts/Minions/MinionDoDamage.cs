using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDoDamage : MonoBehaviour
{
	#region Properties
	[Header("Enemy")]
	[SerializeField]
	private GameObject minionEnemy;
	//private BlockItemsAttributes enemyAttributes;
	private int enemyBaseArmor = 10;
    private AudioSource audioSource;
	#endregion
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void DoDamage()
	{
		//Find the enemy
		minionEnemy = GetComponent<MinionAI>().destinationTarget;

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        
        //Get Current Enemy Attributes
        if (minionEnemy.CompareTag("BlockItemPoints"))
		{
			minionEnemy = GetComponent<MinionAI>().destinationTarget.transform.parent.parent.gameObject;
			//BlockItemsAttributes enemyAttributes = new BlockItemsAttributes();
			BlockItemsAttributes enemyAttributes = minionEnemy.GetComponent<BlockItemsAttributes>();
			int toHitScore = enemyBaseArmor + enemyAttributes.blockItemsAttributes.unitArmor;

			if (enemyAttributes.blockItemsAttributes.unitHealthPoints <=0)
			{
				Debug.Log("Enemy is DEAD");
				//NEXT MOVE
			}
			else
			{
				//Get the main attributer as they Currently are.
				int damageMin = GetComponent<MinionAttributes>().minionAttributes.unitMinDamage;
				int damageMax = GetComponent<MinionAttributes>().minionAttributes.unitMaxDamage;
				int toHit = GetComponent<MinionAttributes>().minionAttributes.unitToHitScore;
				int critChance = GetComponent<MinionAttributes>().minionAttributes.unitCritScore;
				int critMult = GetComponent<MinionAttributes>().minionAttributes.unitCritMultiplier;
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
						minionEnemy.GetComponent<BlockItemsDamages>().TakeDamage(damageRoll, true);
					}
					else
					{

						//Roll the Damage
						int damageRoll = Random.Range(damageMin, damageMax + 1);
						//Tell Current enemy to take damage
						minionEnemy.GetComponent<BlockItemsDamages>().TakeDamage(damageRoll, false);
					}
				}
				else if ((toHitRoll - toHit) < toHitScore)
				{

					string damageRoll = "Miss";
					//Tell Current enemy to take damage
					minionEnemy.GetComponent<BlockItemsDamages>().MissDamage(damageRoll, true);
				}
			}
		}

		if (minionEnemy.CompareTag("RTSUnit"))
		{
			//UnitAttributes  enemyAttributes = new UnitAttributes();
			UnitAttributes enemyAttributes = minionEnemy.GetComponent<UnitAttributes>();
			int toHitScore = enemyBaseArmor + enemyAttributes.unitAttributes.unitArmor;

			if (enemyAttributes.unitAttributes.unitHealthPoints <=0)
			{
				Debug.Log("Enemy is DEAD");
				//NEXT MOVE
			}
			else
			{
				//Get the main attributer as they Currently are.
				int damageMin = GetComponent<MinionAttributes>().minionAttributes.unitMinDamage;
				int damageMax = GetComponent<MinionAttributes>().minionAttributes.unitMaxDamage;
				int toHit = GetComponent<MinionAttributes>().minionAttributes.unitToHitScore;
				int critChance = GetComponent<MinionAttributes>().minionAttributes.unitCritScore;
				int critMult = GetComponent<MinionAttributes>().minionAttributes.unitCritMultiplier;
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
						minionEnemy.GetComponent<UnitDamages>().TakeDamage(damageRoll, true);
					}
					else
					{

						//Roll the Damage
						int damageRoll = Random.Range(damageMin, damageMax + 1);
                        //Tell Current enemy to take damage
                        minionEnemy.GetComponent<UnitDamages>().TakeDamage(damageRoll, false);
					}
				}
				else if ((toHitRoll - toHit) < toHitScore)
				{

					string damageRoll = "Miss";
					//Tell Current enemy to take damage
					minionEnemy.GetComponent<UnitDamages>().MissDamage(damageRoll, true);
				}
			}
		}
	}
}
