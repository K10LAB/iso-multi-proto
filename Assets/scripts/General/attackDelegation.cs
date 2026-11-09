using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class attackDelegation : NetworkBehaviour
{
    [SerializeField]public bool isAttacking;
    [SerializeField]public List<attackContext> attackContexts = new List<attackContext>();

    [SerializeField]public attackContext currentContext;
    
    
    //[SerializeField] public attackContext currentContext;

    public void changeContext(int i)
    {
        currentContext = attackContexts[i];
    }
    public void performAttack()
    {
        foreach (attackMod f in currentContext.listOfAttacks)
        {
            f.Initialized();
        }
        isAttacking = true;
    }
    public void checkAttack()
    {
        isAttacking = currentContext.listOfAttacks[currentContext.listOfAttacks.Count].processingAttack;
    }
    [System.Serializable]
    public class attackContext
    {
        public int attackCount;
        public int attackCountTemp;
        public float attackAbSRange;
        public List<attackMod> listOfAttacks;
    }
    
    
}

