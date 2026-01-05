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

    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float contactDamage = 10f;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathSpin = 20f;

    Coroutine coroutine;
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

    void FixedUpdate() {
        if (isAlive) {
            DetectHazardDamage();
        }
    }

    public void TakeDamage(float amount) {
        if (!isAlive || !canTakeDamage) return;

        StartCoroutine(ImmunityFrame());

        float finalDamage = CalculateFinalDamage(amount);

        currentHealth -= finalDamage;

        Debug.Log($"Damage Taken: {finalDamage}, Current HP: {currentHealth}");

        if (currentHealth <= 0) {
            Death();
        }
        else {
            ApplyKnockback();
            DamageEffects();
        }
    }
    IEnumerator ImmunityFrame() {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

    void DetectHazardDamage() {
        if (!isPlayer) return;

        foreach (Collider2D col in colliders) {
            if (col.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
                TakeDamage(contactDamage);
                return;
            }
        }
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

    void ResetSpriteColor() {
        spriteRenderer.color = Color.white;
    }

    void StopSpin() {
        if (rb2d != null) rb2d.angularVelocity = 0f;
    }

    void DestroyCreature() {
        Destroy(gameObject);
    }

    float CalculateFinalDamage(float incomingDamageAmount) {
        return Mathf.Max(0, incomingDamageAmount - (incomingDamageAmount / damageReduction));
    }

    void ApplyKnockback() {
        if (rb2d == null) return;

        Vector2 randomDir = new Vector2(Random.Range(-0.5f, 0.5f), 0.5f).normalized;
        rb2d.AddForce(randomDir * knockbackForce, ForceMode2D.Impulse);
    }

    void DamageEffects() {
        if (hurtParticle != null) hurtParticle.Play();
    }
}