using UnityEngine;
using System;
using UnityEngine.AI;

public class wolfIdleState : wolfBaseState
{
    
	public wolfIdleState(wolfContext context, wolfStateMachine.wolfStates state): base(context, state){}

	public override wolfStateMachine.wolfStates NextState()
	{
		return wolfStateMachine.wolfStates.Idle;
        
	}

	public override void ExitState()
	{
	}

	public override void UpdateState()
    {
        detectionLevel.Value += Time.deltaTime;
        Debug.Log(detectionLevel.Value);
    }

	public override void EnterState()
    {
        
    }
    public override void OnTriggerExit(Collider other)
    {
        
    }
    public override void OnTriggerEnter(Collider other)
	{
	}
}
