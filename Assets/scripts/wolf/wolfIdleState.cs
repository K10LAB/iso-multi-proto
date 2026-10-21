using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;

public class wolfIdleState : wolfBaseState
{
	public float randomWaitTime;
	
    
	public wolfIdleState(wolfStateMachine context, wolfStateMachine.wolfStates state): base(context, state){}

	public override wolfStateMachine.wolfStates NextState()
	{
		if(randomWaitTime<0)return wolfStateMachine.wolfStates.Patrol;
		if(isChase.Value) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}
	public override void EnterState()
    {
        randomWaitTime = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
		Debug.Log("Entering Idle");
		masterDelegation.Enter += playerListAdd;
		masterDelegation.Exit += playerListRemove;
    }


	public override void ExitState()
	{
		Debug.Log("Exiting Idle");
		randomWaitTime = 0;

	}
	public override void UpdateState()
    {
        if(detectionChange())return;
		randomWaitTime -= Time.deltaTime;
    }
	

	
    public override void OnTriggerExit2D(Collider2D other)
    {
        //playerListChange(false, other);
    }
    public override void OnTriggerEnter2D(Collider2D other)
	{
		//playerListChange(true, other);
		
	}
}
