using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minCamZoom = 10f;
    public float maxCamZoom = 60f;
    public GameObject parentCamHandler;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 camPos = this.transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y > (Screen.height - panBorderThickness))
        {
            camPos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            camPos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= (Screen.width - panBorderThickness))
        {
            camPos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            camPos.x -= panSpeed * Time.deltaTime;
        }

        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //camPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        if (Input.GetAxis("Mouse ScrollWheel") >0)
        {
            if (this.GetComponent<Camera>().fieldOfView>= minCamZoom)
            {
                this.GetComponent<Camera>().fieldOfView -= scrollSpeed * 10f * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (this.GetComponent<Camera>().fieldOfView <= maxCamZoom)
            {
                this.GetComponent<Camera>().fieldOfView += scrollSpeed * 10f * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxis("Mouse X")<0 && Input.GetMouseButton(0))
        {
            parentCamHandler.transform.Rotate(0, 20 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxis("Mouse X") > 0 && Input.GetMouseButton(0))
        {
            parentCamHandler.transform.Rotate(0, -20 * Time.deltaTime, 0);
        }
        camPos.x = Mathf.Clamp(camPos.x, -panLimit.x, panLimit.x);
        camPos.z = Mathf.Clamp(camPos.z, -panLimit.y, panLimit.y);

        transform.position = camPos;
	}
}
