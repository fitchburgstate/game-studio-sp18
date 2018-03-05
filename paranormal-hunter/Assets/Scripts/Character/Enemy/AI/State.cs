using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;
using Hunter.Character;

//public class State
//{
//    public UtilityBasedAI[] utilityBasedAIs;
//    public Transition[] transitions;

//    public void UpdateState(GameObject controller)
//    {
//        DoActions(controller);
//        CheckTransitions(controller);
//    }

//    private void DoActions(GameObject controller)
//    {
//        for (var i = 0; i < utilityBasedAIs.Length; i++)
//        {
//            utilityBasedAIs[i].Act(controller);
//        }
//    }

//    private void CheckTransitions(GameObject controller)
//    {
//        for (var i = 0; i < transitions.Length; i++)
//        {
//            var decisionSucceeded = transitions[i].decision.Decide(controller);

//            if (decisionSucceeded)
//            {
//                // controller.TransitionToState(transitions[i].trueState);
//            }
//            else
//            {
//                // controller.TransitionToState(transitions[i].falseState);
//            }
//        }
//    }
//}

//public abstract class Decision : ScriptableObject
//{
//    public abstract bool Decide(GameObject controller);
//}

//public class Transition
//{
//    public Decision decision;
//    public State trueState;
//    public State falseState;
//}
