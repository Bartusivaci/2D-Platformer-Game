using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float maxHealth = 200f;

    float currentHealth;
    Rigidbody2D skeletonRigidbody;

    Animator animator;


    void Start()
    {
        skeletonRigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        skeletonRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        skeletonRigidbody.velocity = new Vector2(0, 0);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(skeletonRigidbody.velocity.x)), 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            TakeDamage(arrow.GetDamage());
        }
    }

}
