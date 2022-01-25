using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] float arrowDamage = 40f;

    Rigidbody2D arrowRigidbody;
    CapsuleCollider2D arrowCollider;
    PlayerMovement player;
    float xSpeed;


    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    
    void Update()
    {
        arrowRigidbody.velocity = new Vector2(xSpeed, 0f);
        FlipSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            FindObjectOfType<LÝttleEnemyMovement>().TakeDamage(arrowDamage);
        }
        Destroy(gameObject, 2f);
        
    }


    void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(arrowRigidbody.velocity.x), 1f);
   
    }
}
