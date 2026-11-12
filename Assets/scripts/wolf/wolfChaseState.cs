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

        if(distCheck()<attackD.currentContext.attackAbSRange) return wolfStateMachine.wolfStates.Attack;
        
        
		return stateKey;	
	}
	public override void EnterState()
    {
        navAgent.speed = chaseSpeed;
        
        chaseSetPostion();
    }
	public override void ExitState()
	{
		
        navAgent.ResetPath();
        

	}
	public override void UpdateState()
    {
        chaseSetPostion();
        faceDirChange(playerFocus.transform.position);
        if(detectionChange()) return;
    }

    public void chaseSetPostion()
    {
       
        GameObject ji = closestPlayer();
        
        if(ji == null)return;
        navAgent.SetDestination(ji.transform.position);
        wolfMove();
    }
    
    
	
    
}
