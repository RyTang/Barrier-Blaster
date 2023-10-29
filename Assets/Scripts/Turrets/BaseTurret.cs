using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Building Block that is spawnable by the Player
/// </summary>
public abstract class BaseBlock : MonoBehaviour, IDamageable
{
    public float aliveTime = 10;

    protected virtual void Update()
    {
        if (aliveTime <= 0){
            Destroy(gameObject);            
        }
        aliveTime -= Time.deltaTime;

        PowerMechanism();
    }

    /// <summary>
    /// Performs any power that this body prefab should have on every Update()
    /// </summary>
    protected abstract void PowerMechanism();

    /// <summary>
    /// What happens when this object gets damaged
    /// </summary>
    /// <param name="damageAmount">Damaged Amount</param>
    /// <returns>True when interaction has occured succesfully</returns>
    public virtual bool TakeDamage(int damageAmount){
        aliveTime -= damageAmount;

        return true;
    }
}
