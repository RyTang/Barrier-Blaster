using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : BaseBlock
{
    public LayerMask layersToTarget;
    public Color rangeColor;
    public float circleRadius = 2;
    public float attackTimer = 2;
    public int damage = 1;
    
    private float timer;

    protected override void PowerMechanism()
    {
        if (timer >= 0){
            timer -= Time.deltaTime;
            return;
        }
        timer = attackTimer;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleRadius, layersToTarget);

        if (colliders.Length <= 0) {
            return;
        }

        IDamageable target = null;
        foreach (Collider2D collider in colliders){
            target = null;
            if (collider.TryGetComponent<IDamageable>(out target)) target.TakeDamage(damage);
        }        
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = rangeColor; 

        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
