using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public abstract class Enemy : Character
    {
        public State currentState;
        public State remainState;

        /// <summary>
        /// Element Type of the Enemy
        /// </summary>
        public ElementType type;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown
        /// </summary>
        public OPTIONS elementType;

        public float stateTimeElapsed;

        private void Update()
        {
            currentState.UpdateState(this);
        }

        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                // OnExitState();
            }
        }

        public bool CheckIfCountdownElapsed(float duration)
        {
            stateTimeElapsed += Time.deltaTime;
            return (stateTimeElapsed >= duration);
        }

        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable
        /// </summary>
        /// <param name="elementTypeOption">Option for the Element Type</param>
        public void SetElementType(OPTIONS elementTypeOption)
        {
            switch (elementTypeOption)
            {
                case OPTIONS.Fire:
                    type = new Elements.Fire();
                    break;
                case OPTIONS.Ice:
                    type = new Elements.Ice();
                    break;
                case OPTIONS.Disease:
                    type = new Elements.Disease();
                    break;
                case OPTIONS.Silver:
                    type = new Elements.Silver();
                    break;
                case OPTIONS.Blood:
                    type = new Elements.Blood();
                    break;
                case OPTIONS.Lightning:
                    type = new Elements.Lightning();
                    break;
                case OPTIONS.Mechanical:
                    type = new Elements.Mechanical();
                    break;
                case OPTIONS.Stone:
                    type = new Elements.Stone();
                    break;
            }
        }
    }
}
