using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using UnityEditor;
using Unity.Mathematics;

public class wolfStateMachine : StateManagerM<wolfStateMachine.wolfStates>
{
    

    public enum wolfStates
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    
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

    [SerializeField] public CircleCollider2D detectionCollider;
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
}

    

public abstract class wolfBaseState : BasedStateM<wolfStateMachine.wolfStates>
{
    [SerializeField] public GameObject selfObj;
    public readonly HashSet<GameObject> playerList = new HashSet<GameObject>();
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

    public readonly CircleCollider2D detectionCollider;
    public readonly float chaseSpeed;

    
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
        detectionCollider = context.detectionCollider;
        chaseSpeed = context.chaseSpeed;
    }

    public virtual void faceDirChange()
    {
        Vector2 flatDir = navAgent.pathEndPosition - navAgent.transform.position;
        float mathDir = Mathf.Atan2(flatDir.x, flatDir.y) * Mathf.Rad2Deg;
        if(navAgent.remainingDistance < navAgent.stoppingDistance + 0.1)return;
        if (mathDir < 0)
        {
            faceDir.Value = 360f + mathDir;
        } else faceDir.Value = mathDir;
    }

    public virtual void wolfMove()
    {
        if(!navAgent.pathPending && navAgent.remainingDistance>0.1f)navAgent.transform.position += navAgent.desiredVelocity * Time.deltaTime;
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

    public virtual void playerListChange(bool addOrMinus,Collider2D player)
    {
        Debug.Log("enterBuggle");
        Debug.Log(player.CompareTag("Player"));
        if (player.CompareTag("Player"))
        {
            Debug.Log("register");
            if (addOrMinus)
            {
                playerList.Add(player.gameObject);
            } else playerList.Remove(player.gameObject);
        }
    }    
}