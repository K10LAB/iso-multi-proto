using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class attackDelegation : NetworkBehaviour
{
    [SerializeField] public List<attackContext> attackContexts = new List<attackContext>();
    
    
    //[SerializeField] public attackContext currentContext;
    public struct attackSystem
    {
        public attackContext c;
        //public int attackCount{get; set;}
        public attackSystem(attackContext c)
        {
            this.c = c;
        }
    }
    [System.Serializable]
    public class attackContext
    {
        public enum attackType
        {
            Range, 
            Melee
        }
        public attackType Type;
        public float chargeRate;
        public float attackCoolDown;
        public int attackCount;
        public GameObject source;
        public GameObject attackGobj;
        public float attackRange;
        public float chaseRange;
        public float attackSpeed;
        public float burstCount;
        public float burstTime;

        public UnityEvent<attackSystem> AttackSys;
    }
    public void attack(attackContext currentContext)
    {
        currentContext.AttackSys.Invoke(new attackSystem(currentContext));
    }
    
}

