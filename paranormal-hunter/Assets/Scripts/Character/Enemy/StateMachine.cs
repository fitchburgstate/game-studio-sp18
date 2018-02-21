using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

[CreateAssetMenu(menuName = "Enemy AI/State")]
public abstract class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;

    public void UpdateState(Enemy controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void DoActions(Enemy controller)
    {
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(Enemy controller)
    {
        for (var i = 0; i < transitions.Length; i++)
        {
            var decisionMade = transitions[i].decision.Decide(controller);

            if (decisionMade)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}

public abstract class Action : ScriptableObject
{
    public abstract void Act(Enemy controller);
}

public abstract class Decision : ScriptableObject
{
    public abstract bool Decide(Enemy controller);
}

public class WanderAction : Action
{
    public override void Act(Enemy controller)
    {
        Wander(controller);
    }

    private void Wander(Enemy controller)
    {
        // Wander mechanics go here
    }
}

public class IdleAction: Action
{
    public override void Act(Enemy controller)
    {
        Idle(controller);
    }

    private void Idle(Enemy controller)
    {
        // Idle mechanics go here
    }
}

public class ChaseAction : Action
{
    public override void Act(Enemy controller)
    {
        Chase(controller);
    }

    private void Chase(Enemy controller)
    {
        // Chase mechanics go here
    }
}

public class DashAction : Action
{
    public override void Act(Enemy controller)
    {
        Dash(controller);
    }

    private void Dash(Enemy controller)
    {
        // Chase mechanics go here
    }
}

public class AttackAction : Action
{
    public override void Act(Enemy controller)
    {
        Attack(controller);
    }

    private void Attack(Enemy controller)
    {
        // Attack mechanics go here
    }
}

public class ScanDecision : Decision
{
    public override bool Decide(Enemy controller)
    {
        var noEnemyFound = Scan(controller);
        return noEnemyFound;
    }

    private bool Scan(Enemy controller)
    {
        // Scan mechanics go here
        return false; // This must be changed later
    }
}

public class ActiveStateDecision : Decision
{
    public override bool Decide(Enemy controller)
    {
        // Active State Decision mechanics go here
        var hasTarget = false;
        return hasTarget;
    }
}

[System.Serializable]
public class Transition
{
    public Decision decision;
    public State trueState;
    public State falseState;
}
