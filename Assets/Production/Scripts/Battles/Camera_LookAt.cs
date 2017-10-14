using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_LookAt : MonoBehaviour
{
    [SerializeField]
    private GameObject faceCamera;

    private void Awake()
    {
        if (faceCamera == null)
        {
            faceCamera = Camera.main.gameObject;
        }
    }

    void Update ()
	{
        //LookAtCamera();
        AltLookAtCamera();

    }

	private void LookAtCamera()
	{
		transform.LookAt(2 * transform.position - Camera.main.transform.position);
	}

    private void AltLookAtCamera()
    {
        this.transform.LookAt(this.transform.position + faceCamera.transform.rotation * Vector3.forward, faceCamera.transform.rotation * Vector3.up);
    }

	/*
	[SerializeField]
    private GameObject playerViewPoint;

	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(2 * transform.position - playerViewPoint.transform.position);
    }*/
}
