using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using System.Linq;
using Unity.Services.Lobbies.Models;

public class wolfChaseState : wolfBaseState
{
	public wolfChaseState(wolfStateMachine context, wolfStateMachine.wolfStates state): base(context, state){}

    
    
	public override wolfStateMachine.wolfStates NextState()
	{
		if(detectionLevel.Value<=0) return wolfStateMachine.wolfStates.Patrol;

        if(distCheck()<attackC.attackRange*attackC.chaseRange &&chargeCurrent.Value>=attackC.attackCoolDown) return wolfStateMachine.wolfStates.Attack;
        
        
		return stateKey;	
	}
	public override void EnterState()
    {
        navAgent.speed = chaseSpeed;
        chaseSetPostion();
    }
	public override void ExitState()
	{
		Debug.Log("left chase");
        navAgent.ResetPath();
        chargeCurrent.Value = 0;

	}
	public override void UpdateState()
    {
        chaseSetPostion();
        if(attackC.attackCoolDown>=chargeCurrent.Value)chargeCurrent.Value += Time.deltaTime;
        if(detectionChange()) return;
    }

    public void chaseSetPostion()
    {
        GameObject ji = cloestPlayer();
        if(ji == null)return;
        navAgent.SetDestination(ji.transform.position);
        wolfMove();
    }
    
    
	
    
}
