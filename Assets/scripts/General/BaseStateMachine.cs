using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class StateManager<Estate> : NetworkBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BasedState<Estate>> states = new Dictionary<Estate, BasedState<Estate>>();
    protected BasedState<Estate> currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsServer && currentState != null)currentState.EnterState();
    }

    void Update()
    {
        if(!IsServer || currentState == null)return;

        Estate nextState = currentState.NextState();
        if (nextState.Equals(currentState.stateKey))
        {
            currentState.UpdateState();
        } else ChangeState(nextState);
    }
    void ChangeState(Estate nextState)
    {
        Debug.Log("ChangeState");
        if(!IsServer || currentState == null)return;
        currentState.ExitState();
        currentState = states[nextState];
        currentState.EnterState();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(!IsServer || currentState == null)return;
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if(!IsServer || currentState == null)return;
        currentState.OnTriggerExit(other);
    }
}
public abstract class BasedState<EState>  where EState : Enum
{
    public BasedState(EState key)
    {
        stateKey = key;
    }
    public EState stateKey{get; private set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerExit(Collider other);
    public abstract EState NextState();
   
}

