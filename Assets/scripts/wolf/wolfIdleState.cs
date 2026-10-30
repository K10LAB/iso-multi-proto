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
		if(detectionLevel.Value >= detectionMax) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}
	public override void EnterState()
    {
        randomWaitTime = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
		Debug.Log("Entering Idle");
		
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
	

	
    
}
