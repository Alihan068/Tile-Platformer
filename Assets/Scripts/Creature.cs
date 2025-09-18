using Unity.Cinemachine;
using UnityEngine;

public class Creature : MonoBehaviour {

    Rigidbody2D rb2d;
    ParticleSystem hurtParticle;
    Animator animator;

    [Header("Stats")]
    //[SerializeField] float baseDamage = 10f;
    [SerializeField] float baseArmor = 10f;
    float extraArmor = 0f;
    float totalArmor;

    [SerializeField] float baseHealth = 100f;
    float extraHealth = 0f;
    float maxHealth = 100f;
    float currentHealth = 100f;

    [HideInInspector] public bool isAlive = true;
    public bool isPlayer = false;
  

    public Vector2 knockbackAmount;
    [SerializeField] float knockbackForce = 1f;
    [SerializeField] float contactDamage = 10f;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathSpin = 20f;
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        hurtParticle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();

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
        FindFirstObjectByType<PlayerController>().canMove = false;
        isAlive = false;
        animator.SetTrigger("death");
        DeathEffects();

    }
    void DeathEffects() {
        rb2d.linearVelocity = deathKick;
        //RedColorBlink
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetSpriteColor), 0.2f);
        //Stop Camera on death area
        FindAnyObjectByType<CinemachineCamera>().enabled = false;
        //Disable Colliders
        Collider2D[] collider2Ds = GetComponents<Collider2D>();
        foreach (Collider2D col in collider2Ds) {
            col.enabled = false;
        }
        //DeathSpin
        GetComponent<Rigidbody2D>().freezeRotation = false;
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
        rb2d.AddForce(knockbackAmount, ForceMode2D.Impulse);
    }

    void DamageEffects() {
        hurtParticle.Play();
    }

}
