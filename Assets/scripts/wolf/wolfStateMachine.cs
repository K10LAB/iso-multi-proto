using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using UnityEditor;

public class wolfStateMachine : StateManagerM<wolfStateMachine.wolfStates>
{
    

    public enum wolfStates
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private float detectionMax;
    [SerializeField] private float detectionRate;
    [SerializeField] private NetworkVariable<float> detectionLevel = new NetworkVariable<float>
    (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private float idleTimeMax;
    [SerializeField] private float idleTimeMin;

    [SerializeField] private float patrolSpeed;

    [SerializeField] private int patrolPositions;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float chaseSpeed;
    wolfContext context;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        navAgent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;

        context = new wolfContext
        (navAgent, detectionMax, detectionRate, detectionLevel, idleTimeMax, idleTimeMin, patrolSpeed, patrolPositions, 
        spawnPoint, chaseDistance,chaseSpeed);

        setupState();
        
    }
    public void setupState()
    {
        //states.Add(wolfStates.Idle, new wolfIdleState(context, wolfStates.Idle));
        states.Add(wolfStates.Patrol, new wolfIdleState(context, wolfStates.Patrol));
        currentState = states[wolfStates.Patrol];
    }

    // Update is called once per frame
    
}
public class wolfContext
{
    public readonly NavMeshAgent navAgent;
    public readonly float detectionMax;
    public readonly float detectionRate;
    public readonly NetworkVariable<float> detectionLevel;

    public readonly float idleTimeMax;
    public readonly float idleTimeMin;

    public readonly float patrolSpeed;

    public readonly int patrolPositions;
    public readonly Vector3 spawnPoint;

    public readonly float chaseDistance;
    public readonly float chaseSpeed;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public wolfContext(NavMeshAgent pA, float dM, float dR, NetworkVariable<float> dL, float iTX, float iTN, float pS, int pP, Vector3 sP, float cD, float cS)
    {
        navAgent = pA;
        detectionMax = dM;
        detectionRate = dR;
        detectionLevel = dL;
        idleTimeMax = iTX;
        idleTimeMin = iTN;
        patrolSpeed = pS;
        patrolPositions = pP;
        spawnPoint = sP;
        chaseDistance = cD;
        chaseSpeed = cS;
    }
    
}
public abstract class wolfBaseState : BasedStateM<wolfStateMachine.wolfStates>
{
    protected NavMeshAgent navAgent;
    public readonly float detectionMax;
    public readonly float detectionRate;
    public readonly NetworkVariable<float> detectionLevel;

    public readonly float idleTimeMax;
    public readonly float idleTimeMin;

    public readonly float patrolSpeed;

    public readonly int patrolPositions;
    public readonly Vector3 spawnPoint;

    public readonly float chaseDistance;
    public readonly float chaseSpeed;

    public wolfBaseState(wolfContext context, wolfStateMachine.wolfStates stateKey) : base(stateKey)
    {
        navAgent = context.navAgent;
        detectionMax = context.detectionMax;
        detectionRate = context.detectionRate;
        detectionLevel = context.detectionLevel;
        idleTimeMax = context.idleTimeMax;
        idleTimeMin = context.idleTimeMin;
        patrolPositions = context.patrolPositions;
        spawnPoint = context.spawnPoint;
        chaseDistance = context.chaseDistance;
        chaseSpeed = context.chaseSpeed;
    }
}