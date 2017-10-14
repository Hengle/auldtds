using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public float speed;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, vertical) * (speed * Time.deltaTime));
	}
}
