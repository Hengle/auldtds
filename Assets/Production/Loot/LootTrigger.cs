using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTrigger : MonoBehaviour
{
    enum lootAwardType {gold,mithril,rare};

   
    [SerializeField]
    private int lootValue;
    [SerializeField]
    private lootAwardType lootType;

    private void Start()
    {
        //Debug.Log("Loot Item is active");
    }

    public void AwardLoot()
    {
        if (lootType == lootAwardType.gold)
        {
            GameMainManager.Instance._treasureGold += lootValue;
            Debug.Log("Awarded:" + lootValue + " gold");
        }
        if (lootType == lootAwardType.mithril)
        {
            GameMainManager.Instance._treasureMithril += lootValue;
            Debug.Log("Awarded:" + lootValue + " mithril");
        }
        if (lootType == lootAwardType.rare)
        {
            int randomGold = Random.Range(50, 150);
            GameMainManager.Instance._treasureGold += randomGold;
            Debug.Log("Awarded:" + randomGold + " gold");
            int randomMithril = Random.Range(1, 5);
            GameMainManager.Instance._treasureMithril += randomMithril;
            Debug.Log("Awarded:" + randomMithril + " mithril");
        }
        Destroy(gameObject);
    }
}
