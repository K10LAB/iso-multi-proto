using Unity.VisualScripting;
using UnityEngine;

public class slashAttack : wolfAttackState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public GameObject projectile{get; set;}
    public slashAttack(wolfStateMachine context, wolfStateMachine.wolfStates state): base(context, state){}
    
    
	public override wolfStateMachine.wolfStates NextState()
	{
		//if(attackEnded) return wolfStateMachine.wolfStates.Chase;
		return stateKey;
        
	}
	public override void EnterState()
    {
        Debug.Log("enter attack");
		triggerD.isProcessingDetection = false;

    }
	public override void ExitState()
	{
		triggerD.isProcessingDetection = true;
		

	}
	public override void UpdateState()
    {

        
		if (chargeCurrent.Value >= chargeRate)
		{
			
		} else chargeCurrent.Value += Time.deltaTime;
    }
    
	
   
}
