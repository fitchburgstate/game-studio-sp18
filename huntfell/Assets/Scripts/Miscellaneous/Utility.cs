using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Hunter.Characters;

namespace Hunter
{
    public static class Utility
    {
        #region RandomNavMeshPoint Function
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
        #endregion

        #region ElementOptionToElement Function
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
                case ElementOption.Lightning:
                    return new Element.Lightning();
                case ElementOption.Nature:
                    return new Element.Nature();
                case ElementOption.None:
                    return null;
                default:
                    Debug.LogError("You tried to convert to an Element that doesn't have an option in the Element Converter method. Check that the method has the option you are looking for or check that you passed a valid value.");
                    break;
            }
            return null;
        }
        #endregion

        #region ElementToElementOption Function
        //Yeah this breaks style guide lines, dont fucking touch it -Connor
        public static ElementOption ElementToElementOption(Element element)
        {
            if (element is Element.Fire) { return ElementOption.Fire; }
            else if (element is Element.Ice) { return ElementOption.Ice; }
            else if (element is Element.Lightning) { return ElementOption.Lightning; }
            else if (element is Element.Nature) { return ElementOption.Nature; }
            else if (element is Element.Silver) { return ElementOption.Silver; }
            else { return ElementOption.None; }
        }
        #endregion
    }

    #region Interfaces
    //Interface behaviours for characters and interactables
    public interface IMoveable
    {
        void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection);

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
        void TakeDamage(int damage, bool isCritical, Weapon weaponAttackedWith);
    }

    public interface IAttack
    {
        void Attack();

        IEnumerator PlayAttackAnimation();

        void WeaponAnimationEvent();

        void CycleWeapons(bool cycleUp);

        void CycleElements(bool cycleUp);

        void SwitchWeaponType(bool switchToMelee);
    }


    public interface IInteractable
    {
        void FireInteraction(Character characterTriggeringInteraction);

        bool IsImportant();
    }
    #endregion
}
