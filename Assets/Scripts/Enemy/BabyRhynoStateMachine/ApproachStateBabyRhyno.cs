using UnityEngine;
using System.Collections;

public class ApproachStateBabyRhyno : IBabyRhynoState
{
    private readonly StatePatternBabyRhyno babyRhyno;

    public ApproachStateBabyRhyno(StatePatternBabyRhyno statePatternBabyRhyno)
    {
        babyRhyno = statePatternBabyRhyno;
    }

    public void UpdateState()
    {
        
    }

    public void EnterState()
    {
        babyRhyno.pathTimer = babyRhyno.updatePathTimer;
        babyRhyno.agent.speed = babyRhyno.moveSpeed;
        babyRhyno.agent.enabled = true;
        babyRhyno.agent.updatePosition = true;
        babyRhyno.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        RaycastHit hit;

        babyRhyno.pathTimer -= Time.deltaTime;
        if (babyRhyno.pathTimer <= 0)
        {
            babyRhyno.agent.SetDestination(babyRhyno.target.position);
            babyRhyno.pathTimer = babyRhyno.updatePathTimer;
        }

        babyRhyno.distance = Vector3.Distance(babyRhyno.transform.position, babyRhyno.target.position);
        if (babyRhyno.distance < babyRhyno.range)
            if (Physics.Raycast(babyRhyno.transform.position, babyRhyno.transform.forward, out hit, babyRhyno.range + 1))
                if (hit.transform == babyRhyno.target.transform)
                    ToAttackState();
    }

    public void ToApproachState()
    {
        babyRhyno.approachState.EnterState();
        babyRhyno.currentState = babyRhyno.approachState;
    }

    public void ToAttackState()
    {
        babyRhyno.attackState.EnterState();
        babyRhyno.currentState = babyRhyno.attackState;
    }

    public void ToRestState()
    {
        babyRhyno.restState.EnterState();
        babyRhyno.currentState = babyRhyno.restState;
    }
}
