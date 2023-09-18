using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Interactable, IDamageable
{
    public CentralScore centralScore;

    public override void InteractionMechanism()
    {
        // TODO: Removes it with one click
        int depositedPoints = playerController.DepositBody(1);

        centralScore.AddScoreAmount(depositedPoints);
    }

    /// <summary>
    /// Takes a certain amount of damage and reduces the score accordingly
    /// </summary>
    /// <param name="damage">damage to do</param>
    public bool TakeDamage(int damageAmount){
        centralScore.ReduceScoreAmount(damageAmount);
        return true;
    }

}
