using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LİttleEnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float maxHealth = 100f;

    float currentHealth;
    Rigidbody2D littleEnemyRigidbody;


    void Start()
    {
        littleEnemyRigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        littleEnemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(littleEnemyRigidbody.velocity.x)), 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            TakeDamage(arrow.GetDamage());
        }
    }

}
