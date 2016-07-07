using UnityEngine;
using System.Collections;

public class EscapeStateMelmoso : IMelmosoState
{
    private readonly StatePatternMelmoso melmoso;

    public EscapeStateMelmoso(StatePatternMelmoso statePatternMelmoso)
    {
        melmoso = statePatternMelmoso;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        melmoso.approachState.EnterState();
        melmoso.currentState = melmoso.approachState;
    }

    public void ToAttackState()
    {
        melmoso.attackState.EnterState();
        melmoso.currentState = melmoso.attackState;
    }

    public void ToEscapeState()
    {
        melmoso.escapeState.EnterState();
        melmoso.currentState = melmoso.escapeState;
    }
}
