using System;
using UnityEngine;
using System.Linq;

public interface IMoveable<T>
{
    void Move(T controller);

    void Dash(T controller);

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
