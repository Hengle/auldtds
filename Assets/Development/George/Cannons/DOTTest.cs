using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTTest : MonoBehaviour
{

    public int dotDamage;
    public int dotCD;
    public GameObject dotTarget;
    public bool enableDotEffect = false;

    /*
    public void Update()
    {
        if (enableDotEffect)
        {
            StartCoroutine(DamageOverTime(dotDamage, dotCD));
        }
        else
        {
            DisableDOT();
        }
    }*/

    private void Start()
    {
        StartCoroutine(DamageOverTime(dotDamage, dotCD));
    }


    public void DisableDOT()
    {
        StopAllCoroutines();
    }

    IEnumerator DamageOverTime(int damage, float interval)
    {

        while (true)
        {
            Debug.Log("DOT dam: " + damage + " every " + interval);
            yield return new WaitForSeconds(interval);
        }
    }
}
