using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D playerRigid;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravityScaleAtStart;

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
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
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
}
