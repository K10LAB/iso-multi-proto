using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using System.Linq;
using Unity.Services.Lobbies.Models;
using System.Numerics;
using UnityEngine.UIElements;


public class wolfAttackState : wolfBaseState
{
	public wolfAttackState(wolfStateMachine context, wolfStateMachine.wolfStates state): base(context, state){}
	int attackCount;
	
	public override void EnterState()
	{
		attackCount = attackD.attackCount;
		
	}

	public override void UpdateState()
	{
		
		
		chargeCurrent.Value += Time.deltaTime;
		if(chargeCurrent.Value > attackD.chargeRate)
		{
			attackD.attack();
			chargeCurrent.Value = 0;
			attackCount--;
		}
		
		
	}

	public override wolfStateMachine.wolfStates NextState()
	{
		float distCheck = (playerFocus.transform.position - navAgent.transform.position).magnitude;
		if(attackD.attackRange > distCheck && attackCount>0) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}

	public override void ExitState()
	{
		chargeCurrent.Value = 0;
	}
}
