
using UnityEngine;

public class WolfPatrolState : wolfBaseState
{
    public WolfPatrolState(wolfStateMachine context, wolfStateMachine.wolfStates state) : base(context, state){}
    public int currentPositionPoints;
    public override void EnterState()
    {
        
        currentPositionPoints = patrolPositions+1;
        patrolSetPostion();
        navAgent.speed = patrolSpeed;
    }

    public override void UpdateState()
    {
        navAgent.isStopped = true;
        if(detectionChange())return;
        navAgent.isStopped = false;
        faceDirChange();
        patrolSetPostion();
        wolfMove();
    }

    
    
    public void patrolSetPostion()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= 0.11f)
        {
            if (currentPositionPoints > 0 && currentPositionPoints!=patrolPositions+1)
            {
                Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
                Vector2 newPatrolPosition = new Vector2(randomOffset.x, randomOffset.y) + spawnPoint;
                navAgent.SetDestination(newPatrolPosition);
            } else if(currentPositionPoints>-1)navAgent.SetDestination(spawnPoint);
            currentPositionPoints--;
        }
    }
    public override wolfStateMachine.wolfStates NextState()
    {
        if (currentPositionPoints == -2)return wolfStateMachine.wolfStates.Idle;
        
        if (isChase.Value)return wolfStateMachine.wolfStates.Chase;

        return stateKey;
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        playerListChange(false, other);
    }
    public override void OnTriggerEnter2D(Collider2D other)
	{
		playerListChange(true, other);
	}

    public override void ExitState()
    {
        navAgent.ResetPath();
        currentPositionPoints = -3;
    }
}
