using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTextMinionScript : MonoBehaviour {

    private bool onCD;
    private float textYOffset = 0;
    
    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           CombatTextManager.Instance.CreateText(this.transform.position, textYOffset,"10", Color.green, false);

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "10", Color.green, true);

        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "DamageArea")
        {

            if (!onCD)
            {
                StartCoroutine(CoolDownDamage(other.GetComponent<AreaEffectStats>().effectCDScore));
                int randomRoll = Random.Range(0, 20);
                Debug.Log("DMG Random Roll " + randomRoll);
                if (randomRoll <=14)
                {
                    int dmgRoll = Random.Range(other.GetComponent<AreaEffectStats>().effectMinScore, other.GetComponent<AreaEffectStats>().effectMaxScore);
                    CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + dmgRoll, Color.red, false);
                }
                else
                {
                    Debug.Log("Crit");
                    int dmgRoll = Random.Range(other.GetComponent<AreaEffectStats>().effectMinScore, other.GetComponent<AreaEffectStats>().effectMaxScore);
                    CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + (dmgRoll * 2).ToString(), Color.red, true);
                }
            }
            
        }
        else if (other.name == "HealArea")
        {

            if (!onCD)
            {
                StartCoroutine(CoolDownDamage(other.GetComponent<AreaEffectStats>().effectCDScore));
                int randomRoll = Random.Range(0, 1);
                Debug.Log("HOT Random Roll " + randomRoll);
                if (randomRoll <= 14)
                {
                    int dmgRoll = Random.Range(other.GetComponent<AreaEffectStats>().effectMinScore, other.GetComponent<AreaEffectStats>().effectMaxScore);
                    CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "+" + dmgRoll, Color.green, false);
                }
                else
                {
                    Debug.Log("Crit");
                    int dmgRoll = Random.Range(other.GetComponent<AreaEffectStats>().effectMinScore, other.GetComponent<AreaEffectStats>().effectMaxScore);
                    CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "+" + (dmgRoll * 2).ToString(), Color.green, true);
                }
            }
            
        }
    }


    private IEnumerator CoolDownDamage(float cdScore)
    {
        onCD = true;
        yield return new WaitForSeconds(cdScore);
        onCD = false;
    }
}
