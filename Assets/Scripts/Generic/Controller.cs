using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Rigidbody2D))]

public class Controller : MonoBehaviour {

    public bool isPlayer;
    [HideInInspector] public bool isAlive = true;

    public float moveSpeed = 7f;
    float saveSpeed;
    public bool canMove = true;

    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool patrolEnabled;
    [SerializeField] float patrolSpeed = 5f;

    Collider2D collider2d;

    Rigidbody2D rb2d;
    Animator animator;
    Vector2 moveInput;
    Attack attack;
    Health health;
    //CinemachineCamera cinemachine;

    [SerializeField] Vector2 deathKick = new Vector2(10, 10);
    [SerializeField] float deathSpin = 5f;

    void Awake() {
        //collider2d = GetComponent<Collider2D>();
        isAlive = true;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!isAlive) return;

        if (!isPlayer) {
            if (!patrolEnabled) {
                patrolSpeed = 0;
            }
            else {
                moveInput.x = Mathf.Sign(patrolSpeed);
            }
        }
    }

    void FixedUpdate() {

        Walk();
        FlipSprite();

    }

    void OnMove(InputValue value) {
        if (!isPlayer || !isAlive) return;
        moveInput = value.Get<Vector2>();
    }
    void OnAttack(InputValue value) {
        if (!isPlayer || !isAlive) return;
        if (!value.isPressed) return;

        var attack = GetComponent<Attack>();
        if (attack != null) {
            attack.AttackSequence();
        }
    }
    void OnJump(InputValue value) {
        if (!isPlayer || !isAlive) return;

        if (!IsGrounded()) return;

        if (value.isPressed) { //isPressed to detect any kind of change (from 0 to 1) for both button press and button release
            rb2d.linearVelocity += new Vector2(0f, jumpForce);
        }

    }
    bool IsGrounded() {

        return (true);
    }

    void Walk() {
        if (!canMove) return;       
        
        Vector2 vector = rb2d.linearVelocity;
        vector.x = moveInput.x * moveSpeed;
        rb2d.linearVelocity = vector;

        if (animator) {
            bool hasHorizontal = Mathf.Abs(vector.x) > Mathf.Epsilon;
            animator.SetBool("isWalking", hasHorizontal);
        }


    }
    void FlipSprite() {
        //Check if Entity is moving
        bool EntityHasHorizntalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        if (EntityHasHorizntalSpeed) {
            transform.localScale =
                new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.CompareTag("Weapon")) return;
        patrolSpeed = -patrolSpeed;
        //moveSpeed = saveSpeed;

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (isPlayer) return;
        if (!other.gameObject.CompareTag("Player")) return;
        //moveSpeed = 0;
        FlipSprite();
        attack.AttackSequence();

        //Face to other object
        transform.localScale = new Vector2(Mathf.Sign(other.transform.localScale.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

    }

    public void DeathSequence() {
        Debug.Log($"{gameObject.name} has died!");
        DeathEffects();

        if (!isPlayer) {
            Destroy(gameObject, 0.5f);
        }
        else {
            GameSession gameSession = FindFirstObjectByType<GameSession>();
            gameSession.ProcessPlayerDeath();

        }
    }

    void DeathEffects() {
        if (isPlayer) {
            //Stop Camera on death area
            FindAnyObjectByType<CinemachineCamera>().enabled = false;
        }

        //RedColorBlink
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetSpriteColor), 0.2f);
        //Disable Colliders
        Collider2D[] collider2Ds = GetComponents<Collider2D>();
        foreach (Collider2D col in collider2Ds) {
            col.enabled = false;
        }
        //DeathSpin
        //Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = deathKick;
        rb2d.freezeRotation = false;
        rb2d.AddTorque(deathSpin, ForceMode2D.Impulse);

        Invoke(nameof(StopSpin), 2f);

        Invoke(nameof(DestroyPlayer), 5f);
    }
    void ResetSpriteColor() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    void StopSpin() {
        rb2d.angularVelocity = 0f;
    }
    void DestroyPlayer() {
        Destroy(gameObject);
    }

}

