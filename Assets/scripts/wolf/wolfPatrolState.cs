
using UnityEngine;

public class WolfPatrolState : wolfBaseState
{
    public WolfPatrolState(wolfContext context, wolfStateMachine.wolfStates state) : base(context, state){}
    public int currentPositionPoints;
    public override void EnterState()
    {
        Debug.Log("Enter Patrol");
        currentPositionPoints = patrolPositions;
        Debug.Log(currentPositionPoints);
        navAgent.speed = patrolSpeed;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        
            Vector3 newPatrolPosition = new Vector3(randomOffset.x, 0, randomOffset.y) + spawnPoint;
        
            navAgent.SetDestination(newPatrolPosition);
        
            currentPositionPoints--;
            Debug.Log(currentPositionPoints);
        }
    }

    public override wolfStateMachine.wolfStates NextState()
    {
        if (currentPositionPoints == 0)
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
