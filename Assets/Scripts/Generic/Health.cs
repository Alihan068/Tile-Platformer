using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Base Values")]
    [SerializeField] float baseArmor = 10f;
    [SerializeField] float baseHealth = 100f;

    [Header("Damage Effects")]
    [SerializeField] float knockbackForce = 1f;
    [SerializeField] float contactDamage = 10f;

    [Header("Hazard Cooldown")]
    [SerializeField] float hazardTickRate = 1;

    [Header("Death Effects")]
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathSpinImpulse = 20f;
    float _nextHazardTime;

    float extraArmor = 0f;
    float totalArmor;

    float extraHealth = 0f;
    float maxHealth = 100f;
    float currentHealth = 100f;

    Controller controller;
    Rigidbody2D rb2d;
    ParticleSystem hurtParticle;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start()
    {
        controller = GetComponent<Controller>();
        rb2d = GetComponent<Rigidbody2D>();
        hurtParticle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHealth = baseHealth + extraHealth;
        currentHealth = maxHealth;
        totalArmor = baseArmor + extraArmor;
    }

    void Update()
    {
        // Player-only hazard tick
        if (controller != null && controller.IsPlayer)
        {
            DetectHazardDamage();
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public void TakeDamage(float incomingDamage, Vector3 hitFromPosition)
    {
        float finalDamage = CalculateFinalDamage(incomingDamage);

        if (currentHealth <= finalDamage)
        {
            Death();
            return;
        }

        currentHealth -= finalDamage;
        Debug.Log($"{gameObject.name} - Current HP: {currentHealth}/{maxHealth}");

        KnockbackCalculator(finalDamage, hitFromPosition);
        DamageEffects();
    }

    void DetectHazardDamage()
    {
        if (Time.time < _nextHazardTime) return;

        var cols = GetComponents<Collider2D>();
        for (int i = 0; i < cols.Length; i++)
        {
            var c = cols[i];
            if (!c) continue;
            if (c.IsTouchingLayers(LayerMask.GetMask("Hazards")))
            {
                _nextHazardTime = Time.time + hazardTickRate;
                TakeDamage(contactDamage, transform.position);
                return;
            }
        }
    }


    float CalculateFinalDamage(float incomingDamageAmount)
    {
        float flatDamageReduction = totalArmor / 10f;
        float finalDamage = Mathf.Max(1f, incomingDamageAmount - flatDamageReduction);
        Debug.Log($"Incoming: {incomingDamageAmount}, \nReduced by: {flatDamageReduction}, \nFinal: {finalDamage}");
        return finalDamage;
    }

    void KnockbackCalculator(float damageAmount, Vector3 hitFromPosition)
    {
        if (rb2d == null) return;

        float dynamicKnockback = knockbackForce + (damageAmount / 100f);

        Vector2 direction = (transform.position - hitFromPosition).sqrMagnitude > 0.0001f
            ? (transform.position - hitFromPosition).normalized
            : Vector2.up;

        rb2d.AddForce(direction * dynamicKnockback, ForceMode2D.Impulse);
    }

    void DamageEffects()
    {
        if (hurtParticle != null)
        {
            hurtParticle.Play();
        }
    }

    void Death()
    {
        Debug.Log($"{gameObject.name} has died!");

        if (controller != null && !controller.IsPlayer)
        {
            Destroy(gameObject, 0.5f);
        }
        else if (controller != null && controller.IsPlayer)
        {
            PlayDeathEffectsAndDisable();
            GameSession gameSession = FindFirstObjectByType<GameSession>();
            gameSession.ProcessPlayerDeath();
        }
    }

    public void PlayDeathEffectsAndDisable()
    {
        if (!controller.isAlive) return;
        controller.isAlive = false;

        if (animator) animator.SetTrigger("death");

        rb2d.linearVelocity = deathKick;

        if (spriteRenderer)
        {
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetSpriteColor), 0.2f);
        }

        rb2d.freezeRotation = false;
        rb2d.AddTorque(deathSpinImpulse, ForceMode2D.Impulse);

        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        Invoke(nameof(StopSpin), 2f);
    }

    void ResetSpriteColor()
    {
        if (spriteRenderer)
            spriteRenderer.color = Color.white;
    }

    void StopSpin()
    {
        rb2d.angularVelocity = 0f;
    }
}
