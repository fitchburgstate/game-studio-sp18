using System;
using UnityEngine;
using System.Linq;

public interface IMoveable<T>
{
    void Move(T controller);

    void Dash(T controller);

}

public interface IDamageable<T>
{
    void TakeDamage(T amount);

    void DealDamage(T amount);
}
