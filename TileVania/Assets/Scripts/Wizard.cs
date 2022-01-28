using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
	Transform player;
	public Transform attackPoint;
	public float attackRange = 1.46f;
	public LayerMask playerLayer;
	public HealthBar healtBar;
	public AudioClip fireSFX;

	[SerializeField] float maxHealth = 500f;

	float currentHealth;

	public bool isFlipped = false;

	Animator animator;

	public Transform portalPlace;
	public GameObject portal;

    void Start()
    {
		currentHealth = maxHealth;
		healtBar.SetMaxHealth(maxHealth);
		player = FindObjectOfType<PlayerMovement>().GetTransform();
		animator = GetComponent<Animator>();
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		healtBar.SetHealth(currentHealth);
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		animator.SetTrigger("Death");
		GetComponent<BoxCollider2D>().enabled = false;
		this.enabled = false;
		Instantiate(portal, portalPlace.position, Quaternion.identity);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Arrow")
		{
			Arrow arrow = collision.gameObject.GetComponent<Arrow>();
			TakeDamage(arrow.GetDamage());
		}
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
		AudioSource.PlayClipAtPoint(fireSFX, transform.position);
		
		Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

		foreach(Collider2D players in hitPlayer)
        {
			players.GetComponent<PlayerMovement>().DieAnotherWay();
        }
    }


    void OnDrawGizmosSelected()
    {
		if(attackPoint == null) { return; }
		
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
