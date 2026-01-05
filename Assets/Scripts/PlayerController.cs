using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 5f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D boxCollider2D;
    Creature creature;

    [SerializeField] LayerMask jumpables;
    [SerializeField] int jumpCount;

    public bool canMove = true;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider2D = GetComponentInChildren<CapsuleCollider2D>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        creature = GetComponent<Creature>();
    }

    void FixedUpdate() {
        if (!canMove) return;
        if (creature != null && creature.IsKnockedBack) return;

        if (boxCollider2D.IsTouchingLayers(jumpables) || boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards"))) {
            animator.SetBool("isJumping", false);
        }

        Walk();
        FlipSprite();
    }
    void OnMove(InputValue value) {
        if (!canMove) return;
        moveInput = value.Get<Vector2>();
    }

    void Walk() {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizontalSpeed);
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void OnJump(InputValue value) {
        if (!canMove || !boxCollider2D.IsTouchingLayers(jumpables)) return;
        if (creature != null && creature.IsKnockedBack) return;

        if (value.isPressed) {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        } 
    }
    void OnAttack(InputValue value) {
        if (!canMove) return;
        if (creature != null && creature.IsKnockedBack) return;

        if (value.isPressed) {
            GetComponent<PlayerAttack>().AttackMethod();
        }
    }
}