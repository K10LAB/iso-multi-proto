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

	
	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		
		if (!attackD.isAttacking)
		{
			attackD.performAttack();
		}
		attackD.checkAttack();
		//attackD.playerFocus = closestPlayer();
		faceDirChange(closestPlayer().transform.position);
		
	}

	public override wolfStateMachine.wolfStates NextState()
	{
		
		if(attackD.currentContext.attackAbSRange < distCheck() && !attackD.isAttacking) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}

	public override void ExitState()
	{
		
	}
}
