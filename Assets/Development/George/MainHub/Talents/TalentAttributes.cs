using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TalentAttributes : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    
    public string talentName;

    public int talentRank;

    public int talentTotalRanks;

    public int talentGoldCost;

    public int talentMithrilCost;
    [SerializeField]
    [Multiline]
    private string talentDescription;
    private GameObject infoBox;

    private void Start()
    {
        infoBox = GameObject.Find("InfoBox");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RightClick on button " + this.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Over Button "+this.gameObject.name);
        infoBox.transform.Find("Title").GetComponent<Text>().text = this.talentName;
        infoBox.transform.Find("Description").GetComponent<Text>().text = this.talentDescription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Over Button " + this.gameObject.name);
    }
}
