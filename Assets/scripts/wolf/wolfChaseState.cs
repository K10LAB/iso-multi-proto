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
		if(!isChase.Value) return wolfStateMachine.wolfStates.Patrol;

        if(navAgent.remainingDistance<attackRange&&chargeCurrent.Value>=attackCoolDown) return wolfStateMachine.wolfStates.Attack;
        Debug.Log(navAgent.remainingDistance);
        navAgent.speed = chaseSpeed;
		return stateKey;	
	}
	public override void EnterState()
    {
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
        wolfMove();
        faceDirChange();
        chargeCurrent.Value += Time.deltaTime;
        if(detectionChange()) return;
    }

    public void chaseSetPostion()
    {
        if(playerList.Count() != 0){
            playerFocus = playerList.Last();
            float closeDistance = triggerD.detectionCollider.GetComponent<CircleCollider2D>().radius;
            foreach (GameObject player in playerList)
            {
                playerFocus = player;

                Transform playerT = playerFocus.transform;

                Vector3 directionToPlayer = playerT.position - navAgent.transform.position;

                float distanceToPlayer = directionToPlayer.magnitude;
               
                if (distanceToPlayer <= closeDistance)
                {
                    Debug.Log(navAgent.pathEndPosition);
                    navAgent.SetDestination(playerT.position);
                    closeDistance = distanceToPlayer;
                }  
            }
        }else navAgent.SetDestination(playerFocus.transform.position);
    }
    
	
    
}
