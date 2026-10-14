using UnityEngine;

public class WolfPatrolState : wolfBaseState
{
    public WolfPatrolState(wolfContext context, wolfStateMachine.wolfStates state) : base(context, state){}

    public override void EnterState()
    {
        //Debug.Log("Enter Patrol");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }

    //public override wolfStateMachine.wolfStates NextState()
    //{
    //    return stateKey;
    //}

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }
}
