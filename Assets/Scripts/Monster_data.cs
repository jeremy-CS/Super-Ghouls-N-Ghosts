using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Monster", order = 1)]

public class Monster_data : ScriptableObject
{
    public int hp;
    public int damage;
    public float speed;
    public float follow_range;
    public float lifetime;
}
