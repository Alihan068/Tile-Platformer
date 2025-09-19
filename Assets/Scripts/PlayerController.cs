using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 5f;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathSpin = 20f;

    //public Vector2 checkpointPos = new Vector2 (0, 0);

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D boxCollider2D;

    bool isAlive = true;


    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //checkpointTransform = GetComponent<Transform>();


    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isAlive) return;

        Walk();
        FlipSprite();
        //Death();
    }


    void OnMove(InputValue value) { //W-D (1;1)
        if (!isAlive) return;

        moveInput = value.Get<Vector2>();
    }

    void Walk() {
        if (!isAlive) return;

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = playerVelocity;

        bool playerHasHorizntalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizntalSpeed);
    }
    void FlipSprite() {

        bool playerHasHorizntalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizntalSpeed) {
            transform.localScale =
                new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void OnJump(InputValue value) {
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        if (value.isPressed) { //isPressed to detect any kind of change (from 0 to 1) for both button press and button release
            rb2d.linearVelocity += new Vector2(0f, jumpForce);
        }
    }

    //void Death() {
    //    //if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) ||
    //    //    capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {

    //    //    FindFirstObjectByType<GameSession>().ProcessPlayerDeath();

    //    //    isAlive = false;
    //    //    animator.SetTrigger("death");
    //    //    DeathEffects();

    //    }
    //    void DeathEffects() {
    //    rb2d.linearVelocity = deathKick;
    //    //RedColorBlink
    //    GetComponent<SpriteRenderer>().color = Color.red;
    //    Invoke(nameof(ResetSpriteColor), 0.2f);
    //    //Stop Camera on death area
    //    FindAnyObjectByType<CinemachineCamera>().enabled = false;
    //    //Disable Colliders
    //    Collider2D[] collider2Ds = GetComponents<Collider2D>();
    //    foreach (Collider2D col in collider2Ds) {
    //        col.enabled = false;
    //    }
    //    //DeathSpin
    //    GetComponent<Rigidbody2D>().freezeRotation = false;
    //    rb2d.AddTorque(deathSpin, ForceMode2D.Impulse);

    //    Invoke(nameof(StopSpin), 2f);

    //    Invoke(nameof(DestroyPlayer), 5f);

    //}
    //void ResetSpriteColor() {
    //    GetComponent<SpriteRenderer>().color = Color.white;
    //}
    //void StopSpin() {
    //    rb2d.angularVelocity = 0f;
    //}
    //void DestroyPlayer() {
    //    Destroy(gameObject);
    //}
}

