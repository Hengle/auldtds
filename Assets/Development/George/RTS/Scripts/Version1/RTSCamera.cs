using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{

    public float horizontalSpeed = 40;
    public float verticalSpeed = 40;
    public float cameraRotateSpeed = 80;
    public float cameraDistance = 40;
    public float cameraZoomMouseFactor = 4;


    private float curDistance;

    
    
	
	// Update is called once per frame
	void Update ()
    {
        //Camera Movement Left or Right.
        float horizontal = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * horizontal);


        //Camera ZOOM with Keys
        float vertical = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * vertical);
        //Debug.Log(vertical);

        float verticalMouse = Input.GetAxis("Mouse ScrollWheel") * verticalSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * (verticalMouse * cameraZoomMouseFactor));
        //Debug.Log(verticalMouse);

    }
}
