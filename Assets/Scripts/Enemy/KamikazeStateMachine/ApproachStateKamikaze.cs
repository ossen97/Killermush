using UnityEngine;
using System.Collections;

public class ApproachStateKamikaze: IKamikazeState
{
    private readonly StatePatternKamikaze kamikaze;

    public ApproachStateKamikaze(StatePatternKamikaze statePatternKamikaze)
    {
        kamikaze = statePatternKamikaze;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        kamikaze.pathTimer = kamikaze.updatePathTimer;
        kamikaze.agent.speed = kamikaze.moveSpeed;
        kamikaze.distance = Vector3.Distance(kamikaze.transform.position, kamikaze.target.transform.position);
    }

    public void FixedUpdateState()
    {
        kamikaze.pathTimer -= Time.deltaTime;
        if(kamikaze.pathTimer<=0)
        {
            kamikaze.agent.SetDestination(kamikaze.target.position);
            kamikaze.pathTimer = kamikaze.updatePathTimer;
            kamikaze.distance = Vector3.Distance(kamikaze.transform.position, kamikaze.target.transform.position);
        }
        
        if(kamikaze.distance<kamikaze.rangeMax)
        {
            ToAttackState();
        }

        if (!kamikaze.transform.GetComponent<MeshRenderer>().enabled)
            ToBoomState();
    }

    public void ToApproachState()
    {
        kamikaze.approachState.EnterState();
        kamikaze.currentState = kamikaze.approachState;
    }

    public void ToAttackState()
    {
        kamikaze.attackState.EnterState();
        kamikaze.currentState = kamikaze.attackState;
    }

    public void ToBoomState()
    {
        kamikaze.boomState.EnterState();
        kamikaze.currentState = kamikaze.boomState;
    }
}
