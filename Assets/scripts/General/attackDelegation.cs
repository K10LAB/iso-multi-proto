using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class attackDelegation : NetworkBehaviour
{
    public UnityEvent<attackSystem> Attack1;

    public bool attackEnded;
    public float burstTime;
    public int burstCount;
    public float colliderWidth;
    public float colliderLength;
    public float attackRange;
    public float attackSpeed;
    
    public readonly float chargeRate;

    public int attackCount;
    public GameObject source{get; set;}
    public GameObject attackGObj;

    public bool attackActive;
    public float attackCoolDown;

    //public float attackRange;

    public void attack()
    {
        Attack1.Invoke(new attackSystem(colliderWidth, colliderLength, attackRange, attackSpeed, source));
    }
    public struct attackSystem
    {
    
        public float colliderWidth;
        public float colliderLength;
        public float attackRange;
        public float attackSpeed;
        public GameObject source;
        //public int attackCount{get; set;}
        public attackSystem(float colliderWidth, float colliderLength, float attackDuration, float attackSpeed, GameObject source)
        {
            this.colliderWidth = colliderWidth;
            this.colliderLength = colliderLength;
            this.attackRange =  attackDuration;
            this.source = source;
            this.attackSpeed = attackSpeed;
            
        }
    }
}
