using System;
using UnityEngine;
using System.Linq;
using InControl;
using Hunter;
using UnityEngine.AI;

public interface IMoveable
{
    void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject characterModel, NavMeshAgent agent);

    void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject characterModel, NavMeshAgent agent, Transform target);

    void Dash(CharacterController controller);
}

public interface IUtilityBasedAI
{
    void Idle(CharacterController controller, NavMeshAgent agent);

    void Wander(CharacterController controller, Vector3 finalTarget, NavMeshAgent agent);
}

public interface IDamageable
{
    void DealDamage(int damage, bool isCritical);
}

public interface IAttack
{
    void Attack();

    void EnableMeleeHitbox();

    void DisableMeleeHitbox();

    void GunFiring();
}

public static class Utility
{
    public static T Cast<T>(object o)
    {
        return (T)o;
    }
}
