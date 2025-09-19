using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Attack))]
public class Controller : MonoBehaviour
{
    [Header("Role")]
    [SerializeField] bool isPlayer = true;
    public bool IsPlayer => isPlayer;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] string groundLayerName = "Ground";
    [SerializeField] bool useBoxForGroundCheck = true;

    [Header("Enemy Patrol")]
    [SerializeField] bool patrolEnabled = true;
    [SerializeField] float patrolSpeed = 2f;

    Vector2 moveInput;
    bool wantJump;

    Rigidbody2D rb2d;
    Animator animator;
    BoxCollider2D boxCol;
    CapsuleCollider2D capsuleCol;
    
    int groundMask;
    [HideInInspector] public bool isAlive = true;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        groundMask = LayerMask.GetMask(groundLayerName);
    }

    void Update()
    {
        if (!isAlive) return;

        if (!isPlayer && patrolEnabled)
        {
            moveInput.x = Mathf.Sign(patrolSpeed);
        }
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        Walk();
        FlipSprite();
        TryJump();
    }

    void OnMove(InputValue value)
    {
        if (!isPlayer || !isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isPlayer || !isAlive) return;
        if (value.isPressed) wantJump = true;
    }

    void OnAttack(InputValue value)
    {
        if (!isPlayer || !isAlive) return;
        if (!value.isPressed) return;

        var attack = GetComponent<Attack>();
        if (attack != null)
        {
            attack.OnAttack();
        }
    }

    void Walk()
    {
        Vector2 v = rb2d.linearVelocity;
        v.x = moveInput.x * moveSpeed;
        rb2d.linearVelocity = v;

        if (animator)
        {
            bool hasHorizontal = Mathf.Abs(v.x) > Mathf.Epsilon;
            animator.SetBool("isWalking", hasHorizontal);
        }
    }

    void FlipSprite()
    {
        float vx = rb2d.linearVelocity.x;
        if (Mathf.Abs(vx) > Mathf.Epsilon)
        {
            var s = transform.localScale;
            s.x = Mathf.Sign(vx) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    void TryJump()
    {
        if (!wantJump) return;
        wantJump = false;

        if (!IsGrounded()) return;

        rb2d.linearVelocity += new Vector2(0f, jumpForce);
    }

    bool IsGrounded()
    {
        if (useBoxForGroundCheck && boxCol != null)
            return boxCol.IsTouchingLayers(groundMask);

        if (!useBoxForGroundCheck && capsuleCol != null)
            return capsuleCol.IsTouchingLayers(groundMask);

        return false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isPlayer || !patrolEnabled) return;
        patrolSpeed = -patrolSpeed;
    }
}
