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
		if(attackCount==0)attackCount = attackC.attackCount+1;
	}

	public override void UpdateState()
	{
		if (!attackD.isAttacking)
		{
			attackCount--;
			if(attackCount!=0){
				attackD.attack(attackC);
			}else {
				int currentIndex = attackD.attackContexts.IndexOf(attackC);
				currentIndex = currentIndex + 1>attackD.attackContexts.Count ? 0 : currentIndex+1 ;
				attackC = attackD.attackContexts[currentIndex];
				attackCount = attackC.attackCount+1;
			}
		}
		
	}

	public override wolfStateMachine.wolfStates NextState()
	{
		
		if(attackC.attackRange < distCheck() && !attackD.isAttacking) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
	}

	public override void ExitState()
	{
		
	}
}
