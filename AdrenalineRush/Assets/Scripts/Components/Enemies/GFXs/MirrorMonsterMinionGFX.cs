using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMonsterMinionGFX : EnemyGFX
{
    [SerializeField] private ParticleSystem groundImpactParticles;

    [SerializeField] private ParticleSystem dissolveParticles;
    private Material dissolveMaterial;
    private bool isDissolving = false;
    private float fadeValue = 1f;
    
    private void Start()
    {
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if(isDissolving) {
            fadeValue -= Time.deltaTime;

            if(fadeValue <= 0) {
                fadeValue = 0;
                isDissolving = false;
            }

            dissolveMaterial.SetFloat("_Fade", fadeValue);
        }
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void SetYVelocity(float yVelocity)
    {
        animator.SetFloat("YVelocity", yVelocity);
    }

    public void Idle()
    {
        animator.SetBool("FollowPlayer", false);
    }

    public void Run(float horizontalDirection)
    {
        if(horizontalDirection >= 0.1f) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        animator.SetBool("FollowPlayer", true);
    }

    public void GroundImpacted()
    {
        groundImpactParticles.gameObject.SetActive(true);
        groundImpactParticles.Play();
    }

    public override void Death()
    {
        base.Death();

        StartCoroutine(StartDissolving());
    }

    private IEnumerator StartDissolving()
    {
        yield return new WaitForSeconds(2f);
        isDissolving = true;

        yield return new WaitForSeconds(.2f);
        dissolveParticles.Play();
    }
}
