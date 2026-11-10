using System.Collections.Generic;
using System;
using NUnit.Framework;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class triggerDelegation : NetworkBehaviour
{
    [SerializeField]public Collider2D detectionCollider;
    [SerializeField]public Collider2D hitboxCollider;
    
    [SerializeField]public bool isProcessingDetection;
    [SerializeField]public bool isProcessingHitBox;
    
    
    public UnityEvent<OnTriggerDelegation> Enter;

    public UnityEvent<OnTriggerDelegation> Exit;
    [NonSerialized]public Dictionary<Collider2D, HashSet<Collider2D>> currColliders = new Dictionary<Collider2D, HashSet<Collider2D>>();

    [NonSerialized]public Dictionary<Collider2D, HashSet<Collider2D>> prevColliders = new Dictionary<Collider2D, HashSet<Collider2D>>();
    public ContactFilter2D overlapFilter = new ContactFilter2D();
    public Collider2D[] overlapBuffer = new Collider2D[30];

    void Awake()
    {
        overlapFilter.useTriggers = true;
        overlapFilter.useLayerMask = true;
        overlapFilter.layerMask = Physics2D.AllLayers;
        colliderInitialize(detectionCollider);
        colliderInitialize(hitboxCollider);
    }

    void colliderInitialize(Collider2D col)
    {
        currColliders[col] = new HashSet<Collider2D>();
        prevColliders[col] = new HashSet<Collider2D>();

        
    }
    void FixedUpdate()
    {
        if(!IsServer)return;
        if (isProcessingDetection)
        {
            checkingCollider(detectionCollider);
        }
    }
    void checkingCollider(Collider2D col)
    {
        HashSet<Collider2D>prevT = prevColliders[col];
        HashSet<Collider2D>currT = currColliders[col];
        currT.Clear();

        for(int i = 0; i<col.Overlap(overlapFilter, overlapBuffer); i++)
        {
            Collider2D other = overlapBuffer[i];
            if(other == detectionCollider || other == hitboxCollider) continue;
            currT.Add(other);
            if(!prevT.Contains(other))Enter.Invoke(new OnTriggerDelegation(col, other));
        }

        foreach (Collider2D other in prevT)
        {
            if(!currT.Contains(other))Exit.Invoke(new OnTriggerDelegation(col, other));
        }
        prevColliders[col] = currT;
        currColliders[col] = prevT;
    }



    public struct OnTriggerDelegation
    {
        public Collider2D Self { get; private set;}
        public Collider2D Other { get; private set;}

        public OnTriggerDelegation(Collider2D self, Collider2D other)
        {
            Self = self;
            Other = other;
        }
    }

    
}
