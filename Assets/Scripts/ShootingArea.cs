using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArea : MonoBehaviour
{
    public List<Rect> areasToShootFrom;
    public GameObject objectToShoot;
    public Transform targetToShoot;
    
    public float shootFrequency = 1f;

    public float projectileSpeed = 8f;

    public int projectileDamage = 1;

    private float timeDuration;
    private float timer = 0f;

    private void Start() {
        timeDuration = 1f / shootFrequency;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot(){
        timer += Time.deltaTime;
        if (timer < timeDuration){
            return;
        }

        timer = timer % timeDuration;
        
        // Randomise position to spawn from
        int position = Random.Range(0, areasToShootFrom.Count);
        Rect roughArea = areasToShootFrom[position];
        Vector2 spawnPosition = new Vector2(roughArea.xMin + Random.Range(0, roughArea.width), roughArea.yMin + Random.Range(0, roughArea.height));
        
        // spawn object
        GameObject projectile = Instantiate(objectToShoot, spawnPosition, Quaternion.identity);
        // Set direction and speed

        Vector2 direction = ((Vector2) targetToShoot.position - spawnPosition).normalized;
        
        Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();

        Debug.Assert(projectileBody != null, this + ": " + objectToShoot + " has no component RigidBody2D");

        projectileBody.velocity = direction * projectileSpeed;

        Bullet bullet = projectile.GetComponent<Bullet>();
        
        Debug.Assert(bullet != null, this + ": " + objectToShoot + " has no component Bullet");

        bullet.SetDamage(projectileDamage);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        foreach (Rect area in areasToShootFrom){
            Gizmos.DrawCube(area.center, new Vector2(area.width, area.height));
        }        
    }
}
