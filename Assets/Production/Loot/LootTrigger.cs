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
    [SerializeField]
    private float destroyAfterSeconds = 60.0f;

    private void Start()
    {
        //Debug.Log("Loot Item is active");
        Invoke("ExpireLoot", destroyAfterSeconds);
    }

    public void AwardLoot()
    {
        if (lootType == lootAwardType.gold)
        {
            GameMainManager.Instance._treasureGold += lootValue;
        }
        if (lootType == lootAwardType.mithril)
        {
            GameMainManager.Instance._treasureMithril += lootValue;
        }
        if (lootType == lootAwardType.rare)
        {
            int randomGold = Random.Range(50, 150);
            GameMainManager.Instance._treasureGold += randomGold;
            int randomMithril = Random.Range(1, 5);
            GameMainManager.Instance._treasureMithril += randomMithril;
        }
        Destroy(gameObject);
    }

    private void ExpireLoot()
    {
        Destroy(gameObject);
    }
}
