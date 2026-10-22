using UnityEngine;
using UnityEngine.Events;

public class attackDelegation : MonoBehaviour
{
    public UnityEvent<attackSystem> Attack1;

    public void attack()
    {
        Attack1.Invoke(new attackSystem());
    }
    public struct attackSystem
    {
        public bool attackEnded;
        public Collider2D attackCollider{get; set;}
        public float attackRange{get; set;}
        public int attackCount{get; set;}
        public attackSystem(Collider2D attackCollider, float attackRange, int attackCount)
        {
            this.attackCollider = attackCollider;
            this.attackRange =  attackRange;
            this.attackCount = attackCount;
            attackEnded = false;
        }
    }
}
