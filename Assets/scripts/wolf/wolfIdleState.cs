using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Services.Multiplayer;

public class wolfIdleState : wolfBaseState
{
	public float randomWaitTime;
	
    
	public wolfIdleState(wolfContext context, wolfStateMachine.wolfStates state): base(context, state){}

	//public override wolfStateMachine.wolfStates NextState()
	//{
		//if(transitionToPatrol) {
		//	Debug.Log("transitioning to patrol");
		//	detectionLevel.Value = 100f;
		//	return wolfStateMachine.wolfStates.Patrol;
		//}
		//return stateKey;	
	//}
	public override void EnterState()
    {
        randomWaitTime = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
		
		
		Debug.Log("Entering Idle");
    }
	public override void ExitState()
	{
		Debug.Log("Exiting Idle");
	}

	public override void UpdateState()
    {
        randomWaitTime -= Time.deltaTime;
		if(randomWaitTime<0) stateKey.Value = wolfStateMachine.wolfStates.Patrol;
		Debug.Log(randomWaitTime);
		
		
    }

	
    public override void OnTriggerExit(Collider other)
    {
        
    }
    public override void OnTriggerEnter(Collider other)
	{
	}
}
