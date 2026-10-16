
using UnityEngine;

public class WolfPatrolState : wolfBaseState
{
    public WolfPatrolState(wolfContext context, wolfStateMachine.wolfStates state) : base(context, state){}
    public int currentPositionPoints;
    public override void EnterState()
    {
        Debug.Log("Enter Patrol");
        currentPositionPoints = patrolPositions;
        
        navAgent.speed = patrolSpeed;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {

            if (currentPositionPoints > 0)
            {
                Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        
                Vector2 newPatrolPosition = new Vector2(randomOffset.x, randomOffset.y) + spawnPoint;
        
                navAgent.SetDestination(newPatrolPosition);
            } else if(currentPositionPoints>-1)navAgent.SetDestination(spawnPoint);
            
            currentPositionPoints--;
            Debug.Log(currentPositionPoints);
        }

        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint, patrolRadius);
    }

    public override wolfStateMachine.wolfStates NextState()
    {
        if (currentPositionPoints == -2)
        {
            return wolfStateMachine.wolfStates.Idle;
        }
        return stateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }
}
