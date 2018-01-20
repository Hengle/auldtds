using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int health;

    private void Start()
    {
        //this has to be moved to a Game Manager Object or something
        FloatingTextController.Initialize();
    }

    public virtual void TakeDamage(int amount)
    {
        FloatingTextController.CreateFloatingText(amount.ToString(), this.transform);
        //Debug.LogFormat("{0} was dealt {1} damage", gameObject, amount);
        if ((health -= amount)<= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " has died");
    }

}
