using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using InControl;
using Hunter;
using UnityEngine.AI;

public interface IMoveable
{
    void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection);

    void Move (Transform navMeshTarget);

    void Dash();
}

public interface IUtilityBasedAI
{
    void Idle();

    void Wander(Vector3 finalTarget);
}

public interface IDamageable
{
    void DealDamage(int damage, bool isCritical);
}

public interface IAttack
{
    void Attack();

    IEnumerator PlayAttackAnimation ();

    void WeaponAnimationEvent ();

    void SwitchWeapon (bool cycleRanged, bool cycleMelee);
}

