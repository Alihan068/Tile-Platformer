using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [SerializeField] float baseArmor = 10f;
    [SerializeField] float baseHealth = 100f;
    [SerializeField] float knockbackForce = 1f;
    [SerializeField] float contactDamage = 10f;

    float extraArmor = 0f;
    float totalArmor;

    float extraHealth = 0f;
    float maxHealth = 100f;
    float currentHealth = 100f;

    //GenericCreature creature; //deleted
    Rigidbody2D rb2d;
    ParticleSystem hurtParticle;
    GameSession gameSession;
    Controller controller;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start() {
        controller = GetComponent<Controller>();

        rb2d = GetComponent<Rigidbody2D>();
        hurtParticle = GetComponent<ParticleSystem>();

        maxHealth = baseHealth + extraHealth;
        currentHealth = maxHealth;
        totalArmor = baseArmor + extraArmor;
    }

    void Update() {

    }

    public float GetHealthPercentage() {
        return currentHealth / maxHealth;
    }

    public void TakeDamage(float incomingDamage, Vector3 hitFromPosition) {
        float finalDamage = CalculateFinalDamage(incomingDamage);

        if (currentHealth <= finalDamage) {
            Debug.Log("Player Died!");
            controller.DeathSequence();
        }

        currentHealth -= finalDamage;
        Debug.Log($"{gameObject.name} - Current HP: {currentHealth}/{maxHealth}");

        KnockbackCalculator(finalDamage, hitFromPosition);
        DamageEffects();
    }

    void DetectHazardDamage() {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) {
            if (collider.IsTouchingLayers(LayerMask.GetMask("Hazards"))) {
                Debug.Log("ContactDamage");
                TakeDamage(contactDamage, transform.position);
                return;
            }
        }
    }

    float CalculateFinalDamage(float incomingDamageAmount) {
        float flatDamageReduction = totalArmor / 10f;
        float finalDamage = Mathf.Max(1f, incomingDamageAmount - flatDamageReduction);
        Debug.Log($"Incoming: {incomingDamageAmount}, Reduced by: {flatDamageReduction}, Final: {finalDamage}");
        return finalDamage;
    }

    void KnockbackCalculator(float damageAmount, Vector3 hitFromPosition) {
        //if (rb2d == null) return;

        float dynamicKnockback = knockbackForce + (damageAmount / 100f);

        Vector2 direction = (transform.position - hitFromPosition).sqrMagnitude > 0.0001f
            ? (transform.position - hitFromPosition).normalized
            : Vector2.up;

        rb2d.AddForce(direction * dynamicKnockback, ForceMode2D.Impulse);
    }

    void DamageEffects() {
        if (hurtParticle != null) {
            hurtParticle.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Weapon")) return;
        Vector3 otherPos = other.transform.position;
        float takenDamageRaw = other.GetComponentInParent<Attack>().TotalAttackCalculator();
        TakeDamage(takenDamageRaw, otherPos);


    }
}

