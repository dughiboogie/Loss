using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCombat : MonoBehaviour {
    // Components references
    private Animator animator;
    private PlayerController2D playerController;
    private new Rigidbody2D rigidbody;

    // Animations
    private int attackAnimation = 0;    // Current attack animation to play
    [SerializeField] private float attackAnimationResetTime = .8f;  // Time for the attack combo to reset
    private float attackAnimationCount; // Counter that keeps track of the time from the last attack

    // Hitbox variables
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = .5f;
    [SerializeField] private LayerMask enemyLayers;
    private List<Collider2D> enemiesHit;

    // Player stats
    [SerializeField] private int attackDamage = 1;
    public int maxHealth = 3;
    public int currentHealth;
    [SerializeField] private PlayerHealth playerHealth;

    // Enemy aggro variables
    [SerializeField] private float aggroRange = 5f;
    [SerializeField] private float aggroCheckWaitTime = .5f;
    private List<Collider2D> lastEnemiesAggroed;
    private List<Collider2D> currentEnemiesAggroed;

    // Variables for when player gets hit
    [SerializeField] private float knockbackValueX = 3000f;
    [SerializeField] private float knockbackValueY = 400f;
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float invincibilityTimeAfterHit = 1f;
    private float invincibilityTimeCount;

    [SerializeField] ParticleSystem playerHurtParticles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        enemiesHit = new List<Collider2D>();
        currentEnemiesAggroed = new List<Collider2D>();

        currentHealth = maxHealth;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    #region EnemyAggro

    private void Start()
    {
        StartCoroutine(CheckEnemyAggro(aggroCheckWaitTime));
    }

    // Observer pattern to check if an enemy enters the player's aggro range
    private IEnumerator CheckEnemyAggro(float aggroCheckWaitTime)
    {
        while(true) {   // TODO change true with GameManager.instance.enemiesAreInteractable

            yield return new WaitForSeconds(aggroCheckWaitTime);

            lastEnemiesAggroed = new List<Collider2D>(currentEnemiesAggroed);
            // Check for new enemies aggros
            currentEnemiesAggroed = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, aggroRange, enemyLayers));

            // Remove from current enemies aggroed all objects layered as enemies that don't have an Enemy script.
            // If the current object has an enemy script and was not checked in the previous frame, update its path to follow player.
            for(int i = currentEnemiesAggroed.Count - 1; i >= 0; i--) {
                Enemy currentEnemyScript = currentEnemiesAggroed[i].GetComponent<Enemy>();

                if(currentEnemyScript == null) {
                    currentEnemiesAggroed.RemoveAt(i);
                }
                else if(!lastEnemiesAggroed.Contains(currentEnemiesAggroed[i])) {
                    // Debug.Log("New enemy aggroed!");

                    currentEnemiesAggroed[i].GetComponent<EnemyPathfinding>().FollowPlayer();
                }
            }

            // Check for enemies in lastEnemiesAggroed that are not in the currentEnemiesAggroed list (player lost their aggro)
            foreach(Collider2D enemy in lastEnemiesAggroed) {

                // If player lost enemy aggro call enemy method to stop searching for player
                if(!currentEnemiesAggroed.Contains(enemy)) {

                    // Debug.Log("Player lost enemy aggro!");

                    enemy.GetComponent<EnemyPathfinding>().StopFollowingPlayer();

                    // TODO function to make enemy search near player's last knwown position
                }
            }

            /**
             * TODO set timeout to keep enemy aggro for a few seconds after loss of line of sight
             * 
             * TODO cast a ray or a line to check if enemy has line of sight with player
             * in particular, check for ground colliders and such (a LayerMask may be necessary)
             */

            /*
            // Raycast to every enemy to check if there is line of sight with the player
            for(int i = lastEnemiesAggroed.Count - 1; i >= 0 ; i--) {

                Collider2D currentEnemy = lastEnemiesAggroed[i];
                RaycastHit2D rayHit = Physics2D.Raycast(transform.position, currentEnemy.transform.position);

                if(rayHit.collider != null) {
                    Debug.Log("Linecast object found: " + rayHit.collider.name);
                    Enemy currentEnemyScript = currentEnemy.GetComponent<Enemy>();

                    if(currentEnemyScript == null) {
                        lastEnemiesAggroed.RemoveAt(i);
                    }
                    else {
                        currentEnemy.GetComponent<EnemyAI>().UpdatePath();
                    }

                }

            }
            */

        }
    }

    #endregion

    /**
     * If player is in a combo reduce attack combo reset counter time
     */
    void Update()
    {
        if(invincibilityTimeCount > 0) {
            invincibilityTimeCount -= Time.deltaTime;
        }

        if(attackAnimationCount > 0) {
            attackAnimationCount -= Time.deltaTime;
        }
        else {
            attackAnimation = 0;
        }
    }

    public void TakeDamage(int damage, Vector2 enemyPosition)
    {
        // Prevent damage when player just got hit
        if(invincibilityTimeCount > 0) {
            return;
        }

        currentHealth -= damage;
        playerHealth.RemoveHealth();

        invincibilityTimeCount = invincibilityTimeAfterHit;

        // Shake camera when player is hit
        impulseSource.GenerateImpulse();

        playerHurtParticles.gameObject.SetActive(true);
        playerHurtParticles.Play();

        if(currentHealth <= 0) {
            Die();
        }
        else {
            ApplyKnockback(enemyPosition);
            animator.SetTrigger("Hurt");
        }
    }

    private void ApplyKnockback(Vector2 enemyPosition)
    {
        Vector2 knockbackDirection = new Vector2(transform.position.x - enemyPosition.x, transform.position.y - enemyPosition.y).normalized;
        Vector2 knockbackVector = new Vector2(knockbackDirection.x * knockbackValueX, knockbackDirection.y * knockbackValueY);

        rigidbody.AddForce(knockbackVector);
    }

    private void Die()
    {
        animator.SetTrigger("Death");

        playerController.enabled = false;
        this.enabled = false;
    }

    /**
     * Get attack input and start the correct attack animation based on the fact that the player is doing a combo or not
     */
    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed && playerController.isGrounded) {

            if(attackAnimationCount > 0) {
                attackAnimation = attackAnimation == 3 ? 1 : ++attackAnimation; // Cycle through attack animation 1, 2, 3
            }
            else {
                attackAnimation = 1;
            }

            attackAnimationCount = attackAnimationResetTime;
            animator.SetTrigger("Attack" + attackAnimation);
        }
    }

    /**
     * This method is called as an AnimatorEvent.
     * Get all the enemies in the player's attack hitbox and add them to the enemiesHit list.
     * If an enemy is already in the list, then the duplicate is removed.
     */
    public void HitEnemy()
    {
        List<Collider2D> currentEnemies = new List<Collider2D>(Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers));

        if(currentEnemies.Count > 0) {

            // Remove gameObjects without an Enemy script (sensors, etc. that are layered as enemies)
            for(int i = currentEnemies.Count - 1; i >= 0; i--) {
                Enemy currentEnemyScript = currentEnemies[i].GetComponent<Enemy>();

                if(currentEnemyScript == null) {
                    currentEnemies.RemoveAt(i);
                }
            }

            // Remove duplicates
            foreach(Collider2D enemy in enemiesHit) {
                bool isDuplicate = false;

                for(int j = 0; j < currentEnemies.Count && !isDuplicate; j++) {

                    if(currentEnemies[j] == enemy) {
                        isDuplicate = true;
                        currentEnemies.RemoveAt(j);
                    }
                }
            }

            enemiesHit.AddRange(currentEnemies);
        }
    }

    /**
     * This method is called as an AnimatorEvent.
     * At the end of the current attack animation, cycle through the enemiesHit list and apply damage to all of them.
     */
    public void DoDamage()
    {
        if(enemiesHit.Count > 0) {
            foreach(Collider2D enemy in enemiesHit) {
                if(enemy != null) {
                    enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage, attackPoint.position);
                    impulseSource.GenerateImpulse();
                }
            }

            enemiesHit.Clear();
        }
    }

    #region Combat SFX

    public void PlayLightAttackSound()
    {
        AudioManager.instance.Play("Player_Attack1");
    }

    public void PlayHeavyAttackSound()
    {
        AudioManager.instance.Play("Player_Attack2");
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
