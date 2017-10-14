using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEffectCollisions : MonoBehaviour {

    public float damageCD;
    public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Minion")
        {
            MinionDamages minionDamages = other.gameObject.GetComponent<MinionDamages>();
            if (minionDamages.dotEffect == false)
            {
                //Debug.Log("DOT timeee");
                minionDamages.dotEffect = true;
                minionDamages.TriggerDOT(damage, false, damageCD);
            }
        }
        else
        {
            Debug.Log("ahem");
        }
    }
}
