using UnityEngine;
[System.Serializable]
public class attackContext
{
    public float chargeRate;
    public float attackCoolDown;
    public int attackCount;
    public GameObject source;
    public GameObject attackGobj;

    public Sprite melAttackSkin;
    public float attackRange;
    public float chaseRange;
    public float attackSpeed;
    public float burstCount;
    public float burstTime;
}
