using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class test1 : MonoBehaviour
{

    private UnityAction someListener;

    [SerializeField]
    private GameObject playerBuffManager;
    private PlayerBuffManager playerBM;

    private void Awake()
    {
        playerBM = playerBuffManager.GetComponent<PlayerBuffManager>();
        someListener = new UnityAction(playerBM.SomeFunction);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Test", someListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Test", someListener);
    }

    void SomeFunction()
    {
        Debug.Log("Triggered Evenet Some function for " + gameObject.name);
    }
}
