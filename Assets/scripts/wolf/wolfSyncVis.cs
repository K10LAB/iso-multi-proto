using Unity.Netcode;
using UnityEngine;

public class wolfSyncVis : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer stateVisual;
    

    [SerializeField] public wolfStateMachine wSM;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("camstartfind");
        
        wSM = GetComponent<wolfStateMachine>();
        wSM.networkCurrentState.OnValueChanged += onStateChange;
        onStateChange(default, wSM.networkCurrentState.Value);
        //wSM.detectionLevel.OnValueChanged+= onStateChange;
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        wSM.networkCurrentState.OnValueChanged -=  onStateChange;
    }
    
    void onStateChange(wolfStateMachine.wolfStates old, wolfStateMachine.wolfStates New)
    {
        switch (New)
        {
            case wolfStateMachine.wolfStates.Idle:
            stateVisual.color = Color.green;
            break;
            case wolfStateMachine.wolfStates.Patrol:
            stateVisual.color = Color.yellow;
            break;
            case wolfStateMachine.wolfStates.Chase:
            stateVisual.color = Color.red;
            break;
        }
    }
    
}
