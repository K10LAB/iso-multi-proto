using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class attackDelegation : NetworkBehaviour
{
    [SerializeField]public bool isAttacking;
    public float currentFaceDir;
    public GameObject playerFocus;
    public GameObject Source;
    public void curFace(float old, float New)
    {
        currentFaceDir = New;
    }
    [SerializeField]public List<attackContext> attackContexts = new List<attackContext>();

    [SerializeField]public attackContext currentContext;
    
    
    //[SerializeField] public attackContext currentContext;

    public void changeContext(int i)
    {
        currentContext = attackContexts[i];
    }
    public void performAttack()
    {
        foreach (GameObject attack in currentContext.listOfAttacks)
        {
            attack.SetActive(true);
        }
        
        isAttacking = true;
    }
    public void checkAttack()
    {
        isAttacking = currentContext.attackMarker.activeInHierarchy;
    }
    [System.Serializable]
    public class attackContext
    {
        public GameObject attackMarker;
        public int attackCount;
        public int attackCountTemp;
        public float attackAbSRange;
        public List<GameObject> listOfAttacks;
    }
    
    
}

