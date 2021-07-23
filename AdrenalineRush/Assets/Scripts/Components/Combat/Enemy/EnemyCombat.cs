using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour 
{
    private EnemyGFX gfx;
    private new Rigidbody2D rigidbody;

    [SerializeField] private int maxHealth = 2;
    private int currentHealth;

    private LayerMask playerLayer;

    [SerializeField] private int attackDamage = 1;

    private int deadEnemyLayer = 10; // Layer for dead enemies
    private string deadEnemySortingLayer = "DeadEnemies";

    [SerializeField] private float knockbackValueX = 3000f;
    [SerializeField] private float knockbackValueY = 400f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        gfx = GetComponent<EnemyGFX>();

        playerLayer = LayerMask.GetMask("Player");
    }

    /*
     * Make enemy hurt player when in contact.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(attackDamage, transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(attackDamage, transform.position);
        }
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        currentHealth -= damage;
        rigidbody.velocity = Vector2.zero;

        gfx.Hurt(damageSourcePosition);
        ApplyKnockback(damageSourcePosition);
        
        if(currentHealth <= 0) {
            Die();
        }
        /*
        else {
            ApplyKnockback(damageSourcePosition);
        }
        */

    }

    public void ApplyKnockback(Vector2 damageSourcePosition)
    {
        Vector2 knockbackDirection = new Vector2(transform.position.x - damageSourcePosition.x, transform.position.y - damageSourcePosition.y).normalized;
        Vector2 knockbackVector = new Vector2(knockbackDirection.x * knockbackValueX, knockbackDirection.y * knockbackValueY);

        rigidbody.AddForce(knockbackVector);
    }

    private void Die()
    {
        gfx.Death();
        Debug.Log("Enemy is dead");

        // Change layer of dead enemy to ignore collisions with player
        gameObject.layer = deadEnemyLayer;
        foreach(Transform child in transform) {
            child.gameObject.layer = deadEnemyLayer;
        }
        GetComponent<SpriteRenderer>().sortingLayerName = deadEnemySortingLayer;

        GetComponent<Enemy>().DisableEnemy();
    }

}
