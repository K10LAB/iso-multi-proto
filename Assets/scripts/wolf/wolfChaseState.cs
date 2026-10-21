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

    
    GameObject lastPlayer;
	public override wolfStateMachine.wolfStates NextState()
	{
		if(!isChase.Value) return wolfStateMachine.wolfStates.Patrol;

        if(!navAgent.pathPending && navAgent.remainingDistance <= attackRange) return wolfStateMachine.wolfStates.Attack;
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

	}
	public override void UpdateState()
    {
        chaseSetPostion();
        wolfMove();
        faceDirChange();
        if(detectionChange()) return;
    }

    public void chaseSetPostion()
    {
        if(playerList.Count() != 0){
            lastPlayer = playerList.Last();
            float closeDistance = detectionCollider.GetComponent<CircleCollider2D>().radius;
            foreach (GameObject player in playerList)
            {
                
                Transform playerT = player.transform;

                Vector3 directionToPlayer = playerT.position - navAgent.transform.position;

                float distanceToPlayer = directionToPlayer.magnitude;
               
                if (distanceToPlayer <= closeDistance)
                {
                    Debug.Log(navAgent.pathEndPosition);
                    navAgent.SetDestination(playerT.position);
                    closeDistance = distanceToPlayer;
                }  
            }
        }else navAgent.SetDestination(lastPlayer.transform.position);
    }
    
	
    public override void OnTriggerExit2D(Collider2D other)
    {
        //playerListChange(false, other);
    }
    public override void OnTriggerEnter2D(Collider2D other)
	{
		//playerListChange(true, other);
		
	}
}
