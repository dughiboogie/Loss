using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private PlayerControls controls;

    private int attackAnimation = 0;    // Current attack animation to play
    [SerializeField] private float attackAnimationResetTime = .8f;
    private float attackAnimationCount;

    [SerializeField] private Transform attackPoint;
    private float attackRange = .5f;
    private LayerMask enemyLayers;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
    }

    void Update()
    {
        if(attackAnimationCount > 0) {
            attackAnimationCount -= Time.deltaTime;
        }
        else {
            attackAnimation = 0;
        }
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed) {

            if(attackAnimationCount > 0) { 
                attackAnimation = attackAnimation == 3 ? 1 : ++attackAnimation; // Cycle through attack animation 1, 2, 3
            }
            else {
                attackAnimation = 1;
            }

            attackAnimationCount = attackAnimationResetTime;
            animator.SetTrigger("Attack" + attackAnimation);

            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in enemiesHit) {
                Debug.Log("Enemy " + enemy.name + " hit!");
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
