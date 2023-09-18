using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageable
{
    public int health = 1;
    private int bulletDamage;

    private void Start() {
        Debug.Assert(gameObject.layer == (int) LayerName.ENEMY_BULLET, this + " is not correctly assigned to the ENEMY BULLET Layer");
    }


    /// <summary>
    /// Sets the damage of the bullet
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int damage){
        this.bulletDamage = damage;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Creates interaction based on what is being collided
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null){
            damageable.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

    public bool TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0){
            return false;
        }
        health = Mathf.Max(0, health -damageAmount);
        if (health <= 0 ){
            // TODO: Play Animation
            Destroy(gameObject);
        }
        return true;
    }
}
