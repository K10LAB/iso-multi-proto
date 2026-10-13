using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class StateManagerM<Estate> : NetworkBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BasedStateM<Estate>> states = new Dictionary<Estate, BasedStateM<Estate>>();
    public BasedStateM<Estate> currentState;
    
    

    //protected NetworkVariable<Estate> networkCurrentState = new NetworkVariable<Estate>
    //(default(Estate), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        //networkCurrentState.OnValueChanged += changeNetworkState;
        Debug.Log("IsSpawned");
        Debug.Log(currentState);
        Debug.Log(IsHost);
        if(!IsHost || currentState == null)return;
        Debug.Log("startCode");
        currentState.EnterState();

        //networkCurrentState.Value = currentState.stateKey;

    }

    void Update()
    {

        if(!IsHost || currentState == null)return;
        Estate nextState = currentState.NextState();
        if (nextState.Equals(currentState.stateKey))
        {
            currentState.UpdateState();
        } else ChangeState(nextState);
    }
    void ChangeState(Estate nextState)
    {
        if(!IsHost || currentState == null)return;
        //Debug.Log(currentState);
        //Debug.Log("changeState");
        currentState.ExitState();
        currentState = states[nextState];
        //networkCurrentState.Value = currentState.stateKey;
        
        currentState.EnterState();
        //Debug.Log(currentState);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(!IsHost || currentState == null)return;
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if(!IsHost || currentState == null)return;
        currentState.OnTriggerExit(other);
    }

    private void changeNetworkState(Estate old, Estate New)
    {
        if(IsHost)return;

        if (states.ContainsKey(New))
        {
            currentState = states[New];
        }
    }
    //public override void OnNetworkDespawn()
    //{
        //base.OnNetworkDespawn();
        
        //networkCurrentState.OnValueChanged -= changeNetworkState;

    //}
}
public abstract class BasedStateM<EState>  where EState : Enum
{
    public BasedStateM(EState key)
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

