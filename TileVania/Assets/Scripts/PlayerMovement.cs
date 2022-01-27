using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float shootRate = 2f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    

    Vector2 moveInput;
    Rigidbody2D playerRigid;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    float nextShootTime = 0f;


    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = playerRigid.gravityScale;
        playerFeetCollider = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        if(Time.time >= nextShootTime)
        {
            playerAnimator.SetTrigger("isShooting");
            Instantiate(arrow, bow.position, transform.rotation);

            nextShootTime = Time.time + 1f / shootRate;
        }
        

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            playerRigid.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed , playerRigid.velocity.y);
        playerRigid.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigid.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigid.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigid.velocity.x), 1f);
        }
        
    }

    void ClimbLadder()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            playerRigid.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("isClimbing", false);
            return; 
        }
        Vector2 climbVelocity = new Vector2(playerRigid.velocity.x, moveInput.y * climbSpeed);
        
        playerRigid.velocity = climbVelocity;
        playerRigid.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRigid.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards","Water")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Death");
            playerRigid.velocity = deathKick;
            StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());
        }
    }

    public void DieAnotherWay()
    {
        
        isAlive = false;
        playerAnimator.SetTrigger("Death");
        playerRigid.velocity = deathKick;
        StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());
        
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
