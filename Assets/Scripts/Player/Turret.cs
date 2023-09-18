using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : PlayerBody
{
    public LayerMask layersToTarget;
    public Color detectionRangeColor;
    public float circleRadius = 4;
    public float attackTimer = 1;
    public int damage = 2;

    private GameObject currentTarget;
    private float currentDistance;
    private float timer;

    protected override void PowerMechanism()
    {
        if (timer >= 0){
            timer -= Time.deltaTime;
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleRadius, layersToTarget);

        if (colliders.Length <= 0) {
            return;
        }
        
        IDamageable damageable = null;
        foreach (Collider2D collider in colliders){
            
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            Debug.Log(currentTarget);
            if ((currentTarget == null || distance > currentDistance) && collider.gameObject.TryGetComponent<IDamageable>(out damageable)){
                currentTarget = collider.gameObject;
                currentDistance = distance;
            }
        }

        if (damageable != null) {
            damageable.TakeDamage(damage);
            Debug.Log("Damaged Target");
        }
        currentTarget = null;
        timer = attackTimer;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = detectionRangeColor;

        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
