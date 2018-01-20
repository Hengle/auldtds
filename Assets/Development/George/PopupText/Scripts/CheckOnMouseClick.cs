using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnMouseClick : MonoBehaviour
{

    public void OnMouseDown()
    {
        Debug.Log("Clicked Player");
        this.gameObject.GetComponent<PlayerDamage>().TakeDamage(Random.Range(1,50));
    }

}
