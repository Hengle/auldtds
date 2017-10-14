using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTrigger : MonoBehaviour
{
    enum lootAwardType {gold,mithril,rare};

    [SerializeField]
    private Texture2D defaultCursor;
    [SerializeField]
    private Texture2D pickupLootCursor;
    [SerializeField]
    private int lootValue;
    [SerializeField]
    private lootAwardType lootType;

    private void Start()
    {
        Debug.Log("Loot Item is active");
    }

    private void AwardLoot()
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
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        Debug.Log("loot");
        Cursor.SetCursor(pickupLootCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Debug.Log("left loot");
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseOver()
    {
        Debug.Log("Hovering over loot");
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Clicked on loot");
            AwardLoot();
        }
    }
}
