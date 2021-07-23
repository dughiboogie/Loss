using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    protected new Rigidbody2D rigidbody;

    public float speed = 400f;
    protected Vector2 currentDirection = Vector2.zero;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public abstract void TryMove(Vector2 direction);

    public abstract void Move();

}
