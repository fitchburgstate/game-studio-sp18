using System;
using UnityEngine;
using System.Linq;

public interface IMoveable<T, V>
{
    void Move(T controller, V device);

    void Dash(T controller, V device);

}

public interface IDamageable<T>
{
    void TakeDamage(T amount);

    void DealDamage(T amount);
}
