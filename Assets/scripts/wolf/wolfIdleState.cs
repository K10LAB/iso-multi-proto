using UnityEngine;
using System;
using UnityEngine.AI;

public class wolfIdleState : wolfBaseState
{
	public float randomWaitTime;
	public bool transitionToPatrol;
    
	public wolfIdleState(wolfContext context, wolfStateMachine.wolfStates state): base(context, state){}

	public override wolfStateMachine.wolfStates NextState()
	{
		if(transitionToPatrol) {
			Debug.Log("transitioning to patrol");
			detectionLevel.Value = 100f;
			return wolfStateMachine.wolfStates.Patrol;
		}
		return stateKey;
		
	}
	public override void EnterState()
    {
        randomWaitTime = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
		transitionToPatrol = false;
		Debug.Log("Entering Idle");
    }
	public override void ExitState()
	{
		Debug.Log("Exiting Idle");
	}

	public override void UpdateState()
    {
        randomWaitTime -= Time.deltaTime;
		if(randomWaitTime<0) transitionToPatrol = true;
		Debug.Log(randomWaitTime);
		
		
    }

	
    public override void OnTriggerExit(Collider other)
    {
        
    }
    public override void OnTriggerEnter(Collider other)
	{
	}
}
