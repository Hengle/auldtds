using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlocked : MonoBehaviour
{

	
    public void DestroyMe()
    {
        Destroy(this.gameObject, 4.0f);
    }

}
