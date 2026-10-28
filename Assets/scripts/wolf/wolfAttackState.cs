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

	float burstTime;
	
	public override void EnterState()
	{
		if(attackCount==0)attackCount = attackC.attackCount;
	}

	public override void UpdateState()
	{
		chargeCurrent.Value += Time.deltaTime;
		if(chargeCurrent.Value > attackC.chargeRate/burstTime)
		{
			chargeCurrent.Value = 0;
			attackCount--;
			attackD.attack(attackD.attackContexts.IndexOf(attackC));
		}
		if(attackCount%attackC.burstCount!=0)
		{
			burstTime = attackC.burstTime;
		} else burstTime = 1;
	}

	public override wolfStateMachine.wolfStates NextState()
	{
		float distCheck = (playerFocus.transform.position - navAgent.transform.position).magnitude;
		if(attackC.attackRange > distCheck && attackCount>0) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}

	public override void ExitState()
	{
		chargeCurrent.Value = 0;
	}
}
