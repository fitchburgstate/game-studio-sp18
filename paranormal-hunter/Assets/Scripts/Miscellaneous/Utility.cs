using System;
using UnityEngine;
using System.Linq;
using InControl;
using Hunter;
using UnityEngine.AI;

public interface IMoveable
{
    void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject gameObjectRoot, NavMeshAgent agent);

    void Dash(CharacterController controller);

}

public interface IAttack
{
    void Attack();

    void EnableMeleeHitbox();

    void DisableMeleeHitbox();

    void GunFiring();
}

//public interface IDamageable<T, V>
//{
//    void TakeDamage(T healthValue, V amount);

//    void DealDamage(T targetHealthValue, V amount);
//}

//public interface IHealth<T>
//{
//    void SetMaxHealth(T amount);

//    void SetStartingHealth(T amount);

//    void SetCurrentHealth(T amount);

//}
