using UnityEngine;

public class attackContext : ScriptableObject
{
    public float colliderWidth;
    public float colliderLength;
    public float attackRange;
    public float attackSpeed;
    public GameObject source;
    public GameObject attackGObj;
    public readonly float chargeRate;
    public int attackCount;
    public float attackCoolDown;
    public float burstCount;
    public float burstTime;
}
