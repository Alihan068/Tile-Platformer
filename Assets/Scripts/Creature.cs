using UnityEngine;

public class Creature : MonoBehaviour {

    Rigidbody rb2d;
    ParticleSystem hurtParticle;

    [Header("Stats")]
    //[SerializeField] float baseDamage = 10f;
    [SerializeField] float baseArmor = 10f;
    float extraArmor = 0f;
    float totalArmor;

    [SerializeField] float baseHealth = 100f;
    float extraHealth = 0f;
    float maxHealth = 100f;
    float currentHealth = 100f;

    public bool isPlayer = false;

    public Vector2 knockbackAmount;
    [SerializeField] float knockbackForce = 1f;
    [SerializeField] float contactDamage = 10f;

    void Start() {
        rb2d = GetComponent<Rigidbody>();
        hurtParticle = GetComponent<ParticleSystem>();

        maxHealth = baseHealth + extraHealth;
        currentHealth = maxHealth;
        totalArmor = baseArmor + extraArmor;
    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(float amount) {
        if (currentHealth < amount) {
            Death();
        }
        else {
            float finalDamage = CalculateFinalDamage(amount);
            KnockbackCalculator(finalDamage);
            currentHealth = -finalDamage;
            Debug.Log("Current hp = " + currentHealth);
            DamageEffects();
        }
    }
    void DetectHazardDamage() {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) {

            if (collider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) && isPlayer) {
                TakeDamage(contactDamage);
            }
        }
    }
    void Death() {

        FindFirstObjectByType<GameSession>().ProcessPlayerDeath();

        isAlive = false;
        animator.SetTrigger("death");
        DeathEffects();

    }




    float CalculateFinalDamage(float incomingDamageAmount) {
        float flatDamageRes = totalArmor / 10;
        Debug.Log("Damage Reduce by = " + flatDamageRes);
        float finalDamage = incomingDamageAmount - flatDamageRes;
        Debug.Log("Damage taken = " + finalDamage);
        return finalDamage;
    }

    void KnockbackCalculator(float amount) {
        knockbackForce += amount / 100;
        Vector2 direction = (rb2d.transform.position - this.transform.position).normalized;
        knockbackAmount = direction * knockbackForce;
        rb2d.AddForce(knockbackAmount, (ForceMode)ForceMode2D.Impulse);
    }

    void DamageEffects() {
        hurtParticle.Play();
    }

}
