using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    public BaseEnemy SO_Enemy;
    // Use this for initialization
	void Start ()
    {

        SO_Enemy = Instantiate(Resources.Load("Enemies/Orc Level 5", typeof(BaseEnemy))) as BaseEnemy;
        this.name = SO_Enemy.enemyName;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SO_Enemy = Instantiate(Resources.Load("Enemies/Orc Level 4", typeof(BaseEnemy))) as BaseEnemy;
            this.name = SO_Enemy.enemyName;
        }
	}
}
