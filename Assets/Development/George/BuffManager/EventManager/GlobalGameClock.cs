using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameClock : MonoBehaviour
{

    private float timer;
    [SerializeField]
    private int seconds;	

	void Update ()
    {
        timer += Time.deltaTime;
        seconds = Mathf.RoundToInt(timer);
    }
}
