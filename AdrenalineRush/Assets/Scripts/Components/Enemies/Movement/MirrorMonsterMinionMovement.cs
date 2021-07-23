using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMonsterMinionMovement : EnemyMovement
{
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    private float groundCheckRadius;
    private LayerMask groundLayer;

    private MirrorMonsterMinionGFX gfx;
    private bool wasOnGround = true;

    protected override void Awake()
    {
        base.Awake();

        groundLayer = LayerMask.GetMask("Ground");
        groundCheckRadius = groundCheck.GetComponent<CircleCollider2D>().radius;

        gfx = GetComponent<MirrorMonsterMinionGFX>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        gfx.SetGrounded(isGrounded);
        gfx.SetYVelocity(rigidbody.velocity.y);

        if(currentDirection == Vector2.zero && isGrounded) {
            gfx.Idle();
        }

        if(!wasOnGround && isGrounded) {
            gfx.GroundImpacted();
        }
        wasOnGround = isGrounded;

    }

    public override void TryMove(Vector2 direction)
    {
        if(isGrounded) {
            currentDirection = direction;
            gfx.Run(currentDirection.x);
        }
    }

    public override void Move()
    {
        if(currentDirection != Vector2.zero) {
            rigidbody.AddForce(currentDirection * speed);
            currentDirection = Vector2.zero;
        }
    }
}
