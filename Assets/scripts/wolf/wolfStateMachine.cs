using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

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
    wolfContext context;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        context = new wolfContext(navAgent, detectionMax, detectionRate, detectionLevel);
        setupState();
        
    }
    public void setupState()
    {
        states.Add(wolfStates.Idle, new wolfIdleState(context, wolfStates.Idle));
        currentState = states[wolfStates.Idle];
    }

    // Update is called once per frame
    
}
public class wolfContext
{
    public readonly NavMeshAgent navAgent;
    public readonly float detectionMax;
    public readonly float detectionRate;

    public readonly NetworkVariable<float> detectionLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public wolfContext(NavMeshAgent pA, float dM, float dR, NetworkVariable<float> dL)
    {
        navAgent = pA;
        detectionMax = dM;
        detectionRate = dR;
        detectionLevel = dL;

    }
    
}
public abstract class wolfBaseState : BasedStateM<wolfStateMachine.wolfStates>
{
    protected NavMeshAgent navAgent;
    protected float detectionMax;
    protected float detectionRate;

    protected NetworkVariable<float> detectionLevel;

    public wolfBaseState(wolfContext context, wolfStateMachine.wolfStates stateKey) : base(stateKey)
    {
        navAgent = context.navAgent;
        detectionMax = context.detectionMax;
        detectionRate = context.detectionRate;
        detectionLevel = context.detectionLevel;
    }
}