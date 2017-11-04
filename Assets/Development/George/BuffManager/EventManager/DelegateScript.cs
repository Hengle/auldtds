using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateScript : MonoBehaviour {

    delegate void MyDelegate(int num);
    MyDelegate myDelegate;


    private void OnEnable()
    {
        EventManagerScript.OnClicked += Teleport;
    }

    private void OnDisable()
    {
        EventManagerScript.OnClicked -= Teleport;
    }
    // Use this for initialization
    void Start ()
    {
        myDelegate = PrintNum;
        myDelegate(50);

        myDelegate = DoubleNum;
        myDelegate(50);
    }
	

    void PrintNum(int num)
    {
        print("Print Num: " + num);
    }

    void DoubleNum (int num)
    {

        print("Double Num: " + num * 2);
    }

    void Teleport()
    {
        Debug.Log("Teleporting");
    }
}
