using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCameraRotation : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float camRotY = this.transform.rotation.y;
        camRotY = Mathf.Clamp(camRotY, -90f, 90f);
        Debug.Log("Camera Rot Y = " + camRotY);
    }
}
