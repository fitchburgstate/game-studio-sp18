using System;
using UnityEngine;
using System.Linq;
using InControl;
using Hunter;
using UnityEngine.AI;

public interface IMoveable
{
    void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject playerRoot, NavMeshAgent agent);

    void Dash(CharacterController controller);

}

public interface IDamageable
{
    void DealDamage (int damage, bool isCritical);
}

//public interface IHealth<T>
//{
//    void SetMaxHealth(T amount);

//    void SetStartingHealth(T amount);

//    void SetCurrentHealth(T amount);

//}
