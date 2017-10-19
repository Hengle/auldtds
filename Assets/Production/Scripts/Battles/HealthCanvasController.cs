using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject healthCanvas;

	// Update is called once per frame
	void Update ()
    {
        if (GameMainManager.Instance.showHealthBars == true)
        {
            //Debug.Log("Toggle Health Bars ON");
            healthCanvas.gameObject.SetActive(true);
        }
        else
        {
            //Debug.Log("Toggle Health Bars OFF");
            healthCanvas.gameObject.SetActive(false);
        }
	}


}
