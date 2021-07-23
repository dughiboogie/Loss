using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////
// PASTED FROM OLD ENEMY COMBAT
//////////////////////////////////

public class EnemyCombatAttack : MonoBehaviour
{
    private Animator animator;

    private new Rigidbody2D rigidbody;

    [SerializeField] private int maxHealth = 2;
    private int currentHealth;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = .5f;

    [SerializeField] private float attackCooldown = 1f;
    private float attackCooldownCounter = 0f;

    [SerializeField] private int attackDamage = 1;

    private int deadEnemyLayer = 10; // Layer for dead enemies
    private string deadEnemySortingLayer = "DeadEnemies";

    [SerializeField] private float knockbackValueX = 3000f;
    [SerializeField] private float knockbackValueY = 400f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(attackCooldownCounter > 0) {
            attackCooldownCounter -= Time.deltaTime;
        }
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

    public void Attack()
    {
        if(attackCooldownCounter <= 0) {

            attackCooldownCounter = attackCooldown;
            animator.SetTrigger("Attack");
        }
    }

    public void DoDamage()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if(playerHit != null) {
            playerHit.GetComponent<PlayerCombat>().TakeDamage(attackDamage, transform.position);
        }
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        currentHealth -= damage;
        rigidbody.velocity = Vector2.zero;

        if(currentHealth <= 0) {
            Die();
        }
        else {
            ApplyKnockback(damageSourcePosition);
            animator.SetTrigger("Hurt");
        }
    }

    private void ApplyKnockback(Vector2 damageSourcePosition)
    {
        Vector2 knockbackDirection = new Vector2(transform.position.x - damageSourcePosition.x, transform.position.y - damageSourcePosition.y).normalized;
        Vector2 knockbackVector = new Vector2(knockbackDirection.x * knockbackValueX, knockbackDirection.y * knockbackValueY);

        rigidbody.AddForce(knockbackVector);
    }

    private void Die()
    {
        // animator.SetTrigger("Death");

        Debug.Log("Enemy is dead");

        // Change layer of dead enemy to ignore collisions with player
        gameObject.layer = deadEnemyLayer;
        foreach(Transform child in transform) {
            child.gameObject.layer = deadEnemyLayer;
        }
        GetComponent<SpriteRenderer>().sortingLayerName = deadEnemySortingLayer;

        GetComponent<Enemy>().DisableEnemy();
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint != null) {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
