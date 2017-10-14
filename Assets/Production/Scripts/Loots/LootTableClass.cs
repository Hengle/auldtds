using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTableClass : MonoBehaviour {


    [System.Serializable]
    public class LootItem
    {
        public string lootItemName;
        public GameObject lootItemPrefab;
        public int lootItemDropRarity;
    }

    public List<LootItem> LootTable = new List<LootItem>();
    public int lootDropChance;
    [SerializeField]
    private LayerMask lootLayer;

    private void Update()
    {
        
    }

    public void CalculateLoot(Vector3 lootPosition)
    {
        int calc_dropChange = Random.Range(0, 101);
        if (calc_dropChange > lootDropChance)
        {
            //Debug.Log("No Loot has dropped "+ calc_dropChange);
            return;
        }
        if (calc_dropChange <= lootDropChance)
        {
            //Debug.Log("Something Dropped. Oh Shinny " + calc_dropChange);
            int itemWeight = 0;
            for (int i = 0; i < LootTable.Count; i++)
            {
                itemWeight += LootTable[i].lootItemDropRarity;
            }
            //Debug.Log("ItemWeight= " + itemWeight);
            int randomRoll = Random.Range(0, itemWeight);
            for (int j = 0; j < LootTable.Count; j++)
            {
                if (randomRoll <= LootTable[j].lootItemDropRarity)
                {
                    Instantiate(LootTable[j].lootItemPrefab, lootPosition, Quaternion.identity);
                    return;
                }
                randomRoll -= LootTable[j].lootItemDropRarity;
               // Debug.Log("Random Value Decreased " + randomRoll);
            }
        }
    }

    
}
