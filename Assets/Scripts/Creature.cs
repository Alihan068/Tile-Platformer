using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Creature : MonoBehaviour {

    Rigidbody2D rb2d;
    ParticleSystem hurtParticle;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D[] colliders;

    [Header("Stats")]
    public bool isPlayer = false;

    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    [SerializeField] float damageReduction = 10f;

    [HideInInspector] public bool isAlive = true;

    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float contactDamage = 10f;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathSpin = 20f;

    public bool IsKnockedBack { get; private set; }

    bool canTakeDamage = true;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        hurtParticle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
    }

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Transform sourcePos) {
        if (!isAlive || !canTakeDamage) return;

        ApplyKnockback(sourcePos);
        StartCoroutine(DamageEffects());

        float finalDamage = CalculateFinalDamage(amount);
        currentHealth -= finalDamage;

        Debug.Log($"Damage Taken: {finalDamage}, Current HP: {currentHealth}");

        if (currentHealth <= 0) {
            Death();
        }
    }

    IEnumerator DamageEffects() {
        canTakeDamage = false;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

    float CalculateFinalDamage(float incomingDamageAmount) {
        return Mathf.Max(0, incomingDamageAmount - (incomingDamageAmount / damageReduction));
    }

    void ApplyKnockback(Transform sourcePos) {
        if (rb2d == null) return;

        StopCoroutine(nameof(KnockbackRoutine));
        StartCoroutine(KnockbackRoutine());

        rb2d.linearVelocity = Vector2.zero;
        Vector2 direction = (transform.position - sourcePos.position).normalized;
        Vector2 knockbackDirection = new Vector2(direction.x, 0.5f).normalized;

        rb2d.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    IEnumerator KnockbackRoutine() {
        IsKnockedBack = true;
        yield return new WaitForSeconds(0.2f);
        IsKnockedBack = false;
    }

    void DeathEffects() {
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetSpriteColor), 0.2f);

        foreach (Collider2D col in colliders) {
            col.enabled = false;
        }

        if (rb2d != null) {
            rb2d.linearVelocity = deathKick;
            rb2d.freezeRotation = false;
            rb2d.AddTorque(deathSpin, ForceMode2D.Impulse);
            Invoke(nameof(StopSpin), 2f);
        }

        Invoke(nameof(DestroyCreature), 5f);
    }

    void Death() {
        isAlive = false;

        if (isPlayer) {
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null) pc.canMove = false;

            GameSession session = FindFirstObjectByType<GameSession>();
            if (session != null) session.ProcessPlayerDeath();

            var camera = FindAnyObjectByType<CinemachineCamera>();
            if (camera != null) camera.enabled = false;
        }

        if (animator != null) animator.SetTrigger("death");
        DeathEffects();
    }


    void ResetSpriteColor() {
        spriteRenderer.color = Color.white;
    }

    void StopSpin() {
        if (rb2d != null) rb2d.angularVelocity = 0f;
    }

    void DestroyCreature() {
        Destroy(gameObject);
    }
}