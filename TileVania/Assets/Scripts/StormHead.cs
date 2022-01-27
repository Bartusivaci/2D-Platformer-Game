using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormHead : MonoBehaviour
{

    [SerializeField] Transform attackPoint;
    [SerializeField] Transform player;
    [SerializeField] float attackRange = 3.35f;
    [SerializeField] LayerMask playerLayer;


    bool isFlipped = false;
    

    void Start()
    {
        
    }

    
    void Update()
    {
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            
        }


    }

    public void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            //kill player
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}
