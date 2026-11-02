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
		if(attackCount==0)attackCount = attackC.attackCount+1;
	}

	public override void UpdateState()
	{
		faceDirChange(playerFocus.transform.position);
		if(chargeCurrent.Value==0)attackCount--;
		chargeCurrent.Value += Time.deltaTime;
		if(chargeCurrent.Value > attackC.chargeRate/burstTime)
		{
			attackD.attack(attackC);
		}
		if(attackCount%attackC.burstCount!=0)
		{
			burstTime = attackC.burstTime;
		} else burstTime = 1;
	}

	public override wolfStateMachine.wolfStates NextState()
	{
		
		if(attackC.attackRange < distCheck()) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}

	public override void ExitState()
	{
		chargeCurrent.Value = 0;
	}
}
