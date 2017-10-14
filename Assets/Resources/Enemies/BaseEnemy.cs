using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName ="Enemies/New Enemy", order =1)]
public class BaseEnemy : ScriptableObject
{
    public string enemyName;
    public int enemyLevel;
}
