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
                // Transition to a true state
            }
            else
            {
                // Transition to a false state
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

[System.Serializable]
public class Transition
{
    public Decision decision;
    public State trueState;
    public State falseState;
}
