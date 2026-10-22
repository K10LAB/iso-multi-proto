using UnityEngine;
using UnityEngine.Events;

public class attackDelegation : MonoBehaviour
{
    public UnityEvent<attackSystem> Attack1;

    public bool attackEnded;
    public float colliderWidth{get; set;}
    public float colliderLength{get; set;}
    public float attackRange{get; set;}
    public float attackSpeed{get; set;}
    public GameObject source{get; set;}

    public void attack()
    {
        Attack1.Invoke(new attackSystem(colliderWidth, colliderLength, attackRange, attackSpeed, source));

    }
    public struct attackSystem
    {
    
        public float colliderWidth{get; set;}
        public float colliderLength{get; set;}
        public float attackRange{get; set;}
        public float attackSpeed{get; set;}
        public GameObject source{get; set;}
        //public int attackCount{get; set;}
        public attackSystem(float colliderWidth, float colliderLength, float attackRange, float attackSpeed, GameObject source)
        {
            this.colliderWidth = colliderWidth;
            this.colliderLength = colliderLength;
            this.attackRange =  attackRange;
            this.source = source;
            this.attackSpeed = attackSpeed;
            
        }
    }
}
