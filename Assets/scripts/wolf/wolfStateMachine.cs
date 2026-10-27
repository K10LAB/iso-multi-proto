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

public class wolfStateMachine : StateManagerM<wolfStateMachine.wolfStates>
{
    

    public enum wolfStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }
    

    
    
    
    [SerializeField] public NetworkVariable<float> chargeCurrent = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public triggerDelegation triggerD;
    [SerializeField] public attackDelegation attackD;

    [NonSerialized] public HashSet<GameObject> playerList = new HashSet<GameObject>();
    [SerializeField] public NetworkVariable<bool> isChase = new NetworkVariable<bool>
    (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public NetworkVariable<float> faceDir = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public float detectionMax;
    [SerializeField] public float detectionRate;
    [SerializeField] public readonly NetworkVariable<float> detectionLevel = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public float idleTimeMax;
    [SerializeField] public float idleTimeMin;

    [SerializeField] public float patrolSpeed;

    [SerializeField] public float patrolRadius;

    [SerializeField] public int patrolPositions;
    [SerializeField] public Vector3 spawnPoint;

    //[SerializeField] public CircleCollider2D detectionCollider;

    //[SerializeField] public CircleCollider2D hitboxCollider;
    [SerializeField] public float chaseSpeed;
    

    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        
        
        //detectionCollider = GetComponent<CircleCollider2D>();
        //navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
        spawnPoint = transform.position;
        setupState();
        if(NetworkManager.Singleton!=null)GetComponent<NetworkObject>().Spawn();
        
    }
    public void setupState()
    {
        
        states.Add(wolfStates.Idle, new wolfIdleState(this, wolfStates.Idle));
        states.Add(wolfStates.Patrol, new WolfPatrolState(this, wolfStates.Patrol));
        states.Add(wolfStates.Chase, new wolfChaseState(this, wolfStates.Chase));
        states.Add(wolfStates.Attack, new wolfAttackState(this, wolfStates.Attack));
        currentState = states[wolfStates.Idle];
    }
    public void playerListAdd(triggerDelegation.OnTriggerDelegation dele)
    {
        if(dele.Self != triggerD.detectionCollider)return;
        if (dele.Other.CompareTag("Player"))
        {
            playerList.Add(dele.Other.gameObject);
        }
    }

    public void melee(attackDelegation.attackSystem dele)
    {
        dele.c.attackGObj.GetComponent<attackDelegation>().currentContext = dele.c;
    }
    public void range(attackDelegation.attackSystem dele)
    {
        GameObject proj = Instantiate(dele.c.attackGObj, transform.position, transform.rotation);
    }

    public void playerListRemove(triggerDelegation.OnTriggerDelegation dele)
    {
        if(dele.Self != triggerD.detectionCollider)return;
        if (dele.Other.CompareTag("Player"))
        {
            playerList.Add(dele.Other.gameObject);
        }
    }
    
   
}

    

public abstract class wolfBaseState : BasedStateM<wolfStateMachine.wolfStates>
{
    public GameObject playerFocus;
    public readonly NetworkVariable<float> chargeCurrent;
    
    public readonly HashSet<GameObject> playerList;
    public readonly NetworkVariable<bool> isChase;
    public readonly NetworkVariable<float> faceDir;
    protected NavMeshAgent navAgent;
    public readonly float detectionMax;
    public readonly float detectionRate;
    public readonly NetworkVariable<float> detectionLevel;

    public readonly float idleTimeMax;
    public readonly float idleTimeMin;
    
    public readonly float patrolRadius;
    public readonly float patrolSpeed;

    public readonly int patrolPositions;
    public readonly Vector2 spawnPoint;

    //public readonly CircleCollider2D detectionCollider;
    //public readonly CircleCollider2D hitboxCollider;
    public readonly float chaseSpeed;
    public readonly triggerDelegation triggerD;
    public readonly attackDelegation attackD;

    public attackContext attackC;
    

    
    //public readonly SpriteRenderer stateVisual;

    public wolfBaseState(wolfStateMachine context, wolfStateMachine.wolfStates stateKey) : base(stateKey)
    {
        
        playerList = context.playerList;
        isChase = context.isChase;
        faceDir = context.faceDir;
        navAgent = context.navAgent;
        detectionMax = context.detectionMax;
        detectionRate = context.detectionRate;
        detectionLevel = context.detectionLevel;
        idleTimeMax = context.idleTimeMax;
        idleTimeMin = context.idleTimeMin;
        patrolPositions = context.patrolPositions;
        patrolRadius = context.patrolRadius;
        patrolSpeed = context.patrolSpeed;
        spawnPoint = context.spawnPoint;
        
        chargeCurrent = context.chargeCurrent;
        
        chaseSpeed = context.chaseSpeed;
        triggerD = context.triggerD;
        changeAttackContext(0);
    }
    public virtual void changeAttackContext(int i)
    {
        attackC = attackD.attackContexts[i];
    }

    public virtual void faceDirChange()
    {
        Vector2 flatDir = cloestPlayer().transform.position - navAgent.transform.position;
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
			if(detectionLevel.Value<detectionMax){
				detectionLevel.Value += detectionRate * Time.deltaTime;	
			}
			else
			{
				isChase.Value = true;
			}	
		} else {
            if(detectionLevel.Value > 0)
            {
                detectionLevel.Value -= detectionRate * Time.deltaTime;
            } else
            {
                isChase.Value = false;
            }
        }
        
        return playerList.Count != 0 || isChase.Value;
    }

    public virtual GameObject cloestPlayer()
    {
        if(playerList.Count != 0){
            float closeDistance = triggerD.detectionCollider.GetComponent<CircleCollider2D>().radius;
            foreach (GameObject player in playerList)
            {
                playerFocus = player;

                Transform playerT = playerFocus.transform;

                Vector3 directionToPlayer = playerT.position - navAgent.transform.position;

                float distanceToPlayer = directionToPlayer.magnitude;
               
                if (distanceToPlayer <= closeDistance)
                {
                    Debug.Log(navAgent.pathEndPosition);
                    navAgent.SetDestination(playerT.position);
                    closeDistance = distanceToPlayer;
                }  
            }
        }
        return playerFocus;
    }

    

    
    

    
}