using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class StateManagerM<Estate> : NetworkBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BasedStateM<Estate>> states = new Dictionary<Estate, BasedStateM<Estate>>();
    protected BasedStateM<Estate> currentState;
    
    

    public NetworkVariable<Estate> networkCurrentState = new NetworkVariable<Estate>
    (default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        
        //networkCurrentState.OnValueChanged += changeNetworkState;
        if(!IsServer || currentState == null)return;
        Debug.Log("startCode");
        networkCurrentState.Value = currentState.stateKey;
        currentState.EnterState();
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
        if(!IsServer || currentState == null)return;
        
        currentState.ExitState();
        currentState = states[nextState];
        networkCurrentState.Value = currentState.stateKey;
        currentState.EnterState();
        
        
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer || currentState == null)return;
        currentState.OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!IsServer || currentState == null)return;
        currentState.OnTriggerExit2D(other);
    }

    //private void changeNetworkState(Estate old, Estate New)
    //{
        //if(IsServer)return;

        
       //if (!old.Equals(New))
        //{
        //    currentState.ExitState();
        //    currentState = states[New];
        //    currentState.EnterState();
        //}
    //}

    //public override void OnNetworkDespawn()
    //{
        //base.OnNetworkDespawn();
        
        //currentState.stateKey.OnValueChanged -= changeNetworkState;

    //}
}
public abstract class BasedStateM<EState>  where EState : Enum
{
    public BasedStateM(EState key)
    {
        stateKey = key;
    }
    //public NetworkVariable<EState> stateKey = new NetworkVariable<EState>
    //(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public EState stateKey{get; private set;}
    public abstract void EnterState();

    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
    public abstract EState NextState();
   
}

