using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Hunter.Characters;

namespace Hunter
{
    public static class Utility
    {
        /// <summary>
        /// Finds random point on navmesh based on a stating center position, a range/distance it can go, and a vector3 navposition
        /// </summary>
        public static bool RandomNavMeshPoint(Vector3 center, float range, out Vector3 result)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        public static Element ElementOptionToElement(ElementOption elementOption)
        {
            switch (elementOption)
            {
                case ElementOption.Fire:
                    return new Element.Fire();
                case ElementOption.Ice:
                    return new Element.Ice();
                case ElementOption.Silver:
                    return new Element.Silver();
                case ElementOption.Electric:
                    return new Element.Electric();
                //case ElementOption.Nature:
                //    return new Element.Nature();
                case ElementOption.None:
                    return null;
                default:
                    Debug.LogError("You tried to convert to an Element that doesn't have an option in the Element Converter method. Check that the method has the option you are looking for or check that you passed a valid value.");
                    break;
            }
            return null;
        }

        //Yeah this breaks style guide, dont fucking touch it -Connor
        public static ElementOption ElementToElementOption(Element element)
        {
                 if (element is Element.Fire)       { return ElementOption.Fire; }
            else if (element is Element.Ice)        { return ElementOption.Ice; }
            else if (element is Element.Electric)   { return ElementOption.Electric; }
          //else if (element is Element.Nature)     { return ElementOption.Nature; }
            else if (element is Element.Silver)     { return ElementOption.Silver; }
            else                                    { return ElementOption.None; }
        }

        public static Vector3 GetClosestPointOnNavMesh(Vector3 target, NavMeshAgent agent, Transform transform)
        {
            var hit = new NavMeshHit();
            // This gives us a sample radius for the NavMesh check relative to our NavMesh agent size, so given either scenerio where we are passed a floor point or the character's position, we should be able to find a point on the NavMesh
            var sampleRadius = agent.height + agent.baseOffset;

            if (NavMesh.SamplePosition(target, out hit, sampleRadius, NavMesh.AllAreas))
            {
                target = hit.position;
                //Debug.Log("Hit Position of NavMesh Sample from RayCast: " + target);
            }
            else if (NavMesh.SamplePosition(transform.position, out hit, sampleRadius, NavMesh.AllAreas))
            {
                target = hit.position;
                Debug.LogWarning("Could not find a NavMesh point with the given target. Falling back to the character's current position. Hit Position of NavMesh Sample from current position: " + target);
            }
            else
            {
                target = transform.position;
                Debug.LogError("Could not find a closest point on the NavMesh from either the RayCast Hit Position or the character's current location. Are you sure the character is on the NavMesh?");
            }
            return target;
        }

        public static IEnumerator FadeCanvasGroup (CanvasGroup canvasGroup, AnimationCurve fadeCurve, float fadeDuration, FadeType fadeType)
        {
            canvasGroup.alpha = (float)fadeType;
            if (fadeDuration == 0)
            {
                canvasGroup.alpha = Mathf.Abs(canvasGroup.alpha - 1);
            }
            else
            {
                float curvePos = 0;
                while (curvePos < 1)
                {
                    curvePos += (Time.deltaTime / fadeDuration);
                    if (fadeType == FadeType.In)
                    {
                        canvasGroup.alpha = fadeCurve.Evaluate(1 - curvePos);
                    }
                    else
                    {
                        canvasGroup.alpha = fadeCurve.Evaluate(curvePos);
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    #region Interfaces
    //Interface behaviours for characters and interactables
    public interface IMoveable
    {
        void Move(Vector3 moveDirection, Vector3 lookDirection);

        void Move(Transform navMeshTarget);

        void Dash();

        void Interact();
    }

    public interface IUtilityBasedAI
    {
        void Idle();

        void Wander(Vector3 point);

        void Turn(Transform target);
    }

    public interface IDamageable
    {
        void Damage(int damage, bool isCritical, Weapon weaponAttackedWith);

        void Damage (int damage, bool isCritical, Element damageElement);

        void Heal (int restore, bool isCritical);
    }

    public interface IAttack
    {
        void Attack();

        IEnumerator PlayAttackAnimation();

        void AttackAnimationEvent();

        void CycleWeapons(bool cycleUp);

        void CycleElements(bool cycleUp);

        void SwitchWeaponType(bool switchToMelee);
    }


    public interface IInteractable
    {
        void FireInteraction(Character characterTriggeringInteraction);

        bool IsImportant { get; }
    }
    #endregion
}
