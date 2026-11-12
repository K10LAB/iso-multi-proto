using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using UnityEditor;
using Unity.Mathematics;
using Unity.Netcode.Components;
using System.Net.Mail;
using System.Runtime.Serialization;

public class wolfStateMachine : StateManagerM<wolfStateMachine.wolfStates>
{
    

    public enum wolfStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }
    

    
    
    
    
    [SerializeField] public triggerDelegation triggerD;
    [SerializeField] public attackDelegation attackD;

    [NonSerialized] public HashSet<GameObject> playerList = new HashSet<GameObject>();
  
    [SerializeField] public NetworkVariable<float> faceDir = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public NavMeshAgent navAgent;
    //[SerializeField] public float detectionMax;
    [SerializeField] public float detectionRate;
    [SerializeField] public readonly NetworkVariable<float> detectionLevel = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public float idleTimeMax;
    [SerializeField] public float idleTimeMin;

    [SerializeField] public float patrolSpeed;

    [SerializeField] public float patrolRadius;

    [SerializeField] public int patrolPositions;
    [SerializeField] public Vector3 spawnPoint;

    
    [SerializeField] public float chaseSpeed;

    


    [SerializeField] public wolfSyncVis vis;
    
    

    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
        spawnPoint = transform.position;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(!IsServer) return;
        netSetUp();
        stateSetUp();
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if(!IsServer) return;
        netUnSetUp();
        stateSetUp();
    }
    public void netUnSetUp()
    {
        faceDir.OnValueChanged -= attackD.curFace;
        faceDir.OnValueChanged -= vis.onFaceDirChange;
        vis.onFaceDirChange(0f, faceDir.Value);

        networkCurrentState.OnValueChanged -= vis.onStateChange;
        vis.onStateChange(default, networkCurrentState.Value);

        detectionLevel.OnValueChanged -= vis.onDetectionChange;
        vis.onDetectionChange(0f, detectionLevel.Value);
    }

    public void netSetUp()
    {
        faceDir.OnValueChanged += attackD.curFace;
        faceDir.OnValueChanged += vis.onFaceDirChange;
        vis.onFaceDirChange(0f, faceDir.Value);

        networkCurrentState.OnValueChanged += vis.onStateChange;
        vis.onStateChange(default, networkCurrentState.Value);

        detectionLevel.OnValueChanged += vis.onDetectionChange;
        vis.onDetectionChange(0f, detectionLevel.Value);
    }
    public void stateSetUp()
    {
        states.Add(wolfStates.Idle, new wolfIdleState(this, wolfStates.Idle));
        currentState = states[wolfStates.Idle];
        states.Add(wolfStates.Patrol, new WolfPatrolState(this, wolfStates.Patrol));
        states.Add(wolfStates.Chase, new wolfChaseState(this, wolfStates.Chase));
        states.Add(wolfStates.Attack, new wolfAttackState(this, wolfStates.Attack));
    }
    public void playerListAdd(triggerDelegation.OnTriggerDelegation dele)
    {
        if(dele.Self != triggerD.detectionCollider)return;
        if (dele.Other.CompareTag("Player"))
        {
            playerList.Add(dele.Other.gameObject);
        }
    }
    public void playerListRemove(triggerDelegation.OnTriggerDelegation dele)
    {
        if(dele.Self != triggerD.detectionCollider)return;
        if (dele.Other.CompareTag("Player"))
        {
            playerList.Remove(dele.Other.gameObject);
        }
    }

    
    

    
    
   
}

    

public abstract class wolfBaseState : BasedStateM<wolfStateMachine.wolfStates>
{
    public GameObject playerFocus;
    
    
    public readonly HashSet<GameObject> playerList;
    //public readonly NetworkVariable<bool> isChase;
    public readonly NetworkVariable<float> faceDir;
    protected NavMeshAgent navAgent;
    //public readonly float detectionMax;
    public readonly float detectionRate;
    public readonly NetworkVariable<float> detectionLevel;

    public readonly float idleTimeMax;
    public readonly float idleTimeMin;
    
    public readonly float patrolRadius;
    public readonly float patrolSpeed;

    public readonly int patrolPositions;
    public readonly Vector2 spawnPoint;

    
    public readonly float chaseSpeed;
    public readonly triggerDelegation triggerD;
    public readonly attackDelegation attackD;

    

    
    

    
    //public readonly SpriteRenderer stateVisual;

    public wolfBaseState(wolfStateMachine context, wolfStateMachine.wolfStates stateKey) : base(stateKey)
    {
        
        playerList = context.playerList;
        //isChase = context.isChase;
        faceDir = context.faceDir;
        navAgent = context.navAgent;
        
        detectionRate = context.detectionRate;
        detectionLevel = context.detectionLevel;
        idleTimeMax = context.idleTimeMax;
        idleTimeMin = context.idleTimeMin;
        patrolPositions = context.patrolPositions;
        patrolRadius = context.patrolRadius;
        patrolSpeed = context.patrolSpeed;
        spawnPoint = context.spawnPoint;
        
        
        
        chaseSpeed = context.chaseSpeed;
        triggerD = context.triggerD;
        
    }
    

    public virtual void faceDirChange(Vector2 flatDir)
    {
        flatDir -= new Vector2(navAgent.transform.position.x, navAgent.transform.position.y);
        float mathDir = Mathf.Atan2(flatDir.x, flatDir.y) * Mathf.Rad2Deg;
        if(Mathf.Abs(faceDir.Value) < navAgent.stoppingDistance + 0.1)return;
        if (mathDir < 0)
        {
            faceDir.Value = 360f + mathDir;
        } else faceDir.Value = mathDir;
    }
    

    public virtual void wolfMove()
    {
        navAgent.transform.position += navAgent.desiredVelocity * Time.deltaTime;
        navAgent.nextPosition = navAgent.transform.position;
    }

    public virtual bool detectionChange()
    {
        if(playerList.Count != 0){
            
			if(detectionLevel.Value<=100){
				detectionLevel.Value += detectionRate * Time.deltaTime;	
			}
            closestPlayer();
            return true;
		} else if(detectionLevel.Value >= 0)
        {
            detectionLevel.Value -= detectionRate * Time.deltaTime;
        }
        return false;
        
    }

    public virtual GameObject closestPlayer()
    {
        
        if(playerList.Count !=0){
            float closeDistance = float.MaxValue;
            foreach (GameObject player in playerList)
            {

                Transform playerT = player.transform;

                Vector3 directionToPlayer = playerT.position - navAgent.transform.position;

                float distanceToPlayer = directionToPlayer.magnitude;
                Debug.Log(distanceToPlayer);
                if (distanceToPlayer <= closeDistance)
                {
                    playerFocus = player;
                    closeDistance = distanceToPlayer;
                    
                }  
            }
        }
        return playerFocus;
    }

    public virtual float distCheck(){
        
        return (closestPlayer().transform.position - navAgent.transform.position).magnitude;
    }
    
}