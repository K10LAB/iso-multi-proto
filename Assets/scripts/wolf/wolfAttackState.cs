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

	public override wolfStateMachine.wolfStates NextState()
	{
		return stateKey;	
	}
	public override void EnterState()
    {
        Debug.Log("enter attack");
		
    }
	public override void ExitState()
	{
		

	}
	public override void UpdateState()
    {
        
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
