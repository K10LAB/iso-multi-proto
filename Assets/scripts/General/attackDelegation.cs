using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class attackDelegation : NetworkBehaviour
{
    [SerializeField] public List<attackContext> attackContexts = new List<attackContext>();
    [SerializeField] public List<UnityEvent<attackSystem>> Attack = new List<UnityEvent<attackSystem>>();
    
    [SerializeField] public attackContext currentContext;

    public void attack(int i)
    {
        Attack[i].Invoke(new attackSystem(attackContexts[i]));
    }
    public struct attackSystem
    {
        public attackContext c;
        //public int attackCount{get; set;}
        public attackSystem(attackContext c)
        {
            this.c = c;
        }
    }
}
