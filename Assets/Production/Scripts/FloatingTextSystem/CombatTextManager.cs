using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTextManager : MonoBehaviour
{

    
    private static CombatTextManager instance;
    public GameObject textPrefab;
    public RectTransform canvasTransform;
    public float yOffSet;

    public float textSpeed;
    public Vector3 textDirection;
    public float textFadeTime;
    public GameObject playerViewPoint;


    public static CombatTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    public void CreateText(Vector3 position, float mobYOffset, string text, Color color,bool crit )
    {
        position = new Vector3(position.x, position.y + mobYOffset, position.z);
        GameObject cmbtText = (GameObject)Instantiate(textPrefab, position, Quaternion.identity);
        cmbtText.transform.SetParent(canvasTransform);
        cmbtText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        cmbtText.GetComponent<CombatText>().InitializeText(textSpeed,textDirection, textFadeTime, crit);
        cmbtText.GetComponent<Text>().text = text;
        cmbtText.GetComponent<Text>().color = color;
    }

   

}
