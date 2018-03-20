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

    void Wander(CharacterController controller, NavMeshAgent agent);
}

public interface IAttack
{
    void Attack();

    void EnableMeleeHitbox();

    void DisableMeleeHitbox();

    void GunFiring();
}
