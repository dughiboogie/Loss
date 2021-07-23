using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] private ParticleSystem enemyHurtParticles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hurt(Vector2 damageSourcePosition)
    {
        animator.SetTrigger("Hurt");

        enemyHurtParticles.gameObject.SetActive(true);
        enemyHurtParticles.Play();
    }

    public virtual void Death()
    {
        animator.SetTrigger("Death");
    }
}
