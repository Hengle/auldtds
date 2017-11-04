using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager : MonoBehaviour
{
    public  List<BuffBase> buffList = new List<BuffBase>();


    // Use this for initialization
    public void SomeFunction()
    {
        Debug.Log("Run from Class "+gameObject.name);
    }
}
