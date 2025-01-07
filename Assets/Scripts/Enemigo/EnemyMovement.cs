using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;

    public GameObject target; 

    private Rigidbody2D rb; 
    private SpriteRenderer spriteRenderer; 

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
    }
}
