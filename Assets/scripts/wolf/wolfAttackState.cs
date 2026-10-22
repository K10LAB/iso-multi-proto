using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using System.Linq;
using Unity.Services.Lobbies.Models;

public class wolfAttackState : wolfBaseState
{
	public wolfAttackState(wolfStateMachine context, wolfStateMachine.wolfStates state): base(context, state){}

	public bool attackEnded;
	public override wolfStateMachine.wolfStates NextState()
	{
		if(attackEnded) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}
	public override void EnterState()
    {
        Debug.Log("enter attack");
		triggerD.isProcessingDetection = false;

    }
	public override void ExitState()
	{
		triggerD.isProcessingDetection = true;
		attackEnded = false;

	}
	public override void UpdateState()
    {

        
		if (chargeCurrent.Value >= chargeRate)
		{
			attackD.attack();
		} else chargeCurrent.Value += Time.deltaTime;
    }
    
	
    
}
