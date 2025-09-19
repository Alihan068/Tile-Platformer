using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour {
    [Header("Attack Settings")]
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackCooldown = 0.5f;

    Collider2D weaponCollider;
    GenericCreature creature;
    Animator animator;

    bool canAttack = true;
    bool isPlayer;
    float lastAttackTime;

    void Awake() {
        weaponCollider = GetComponentInChildren<Collider2D>();
        creature = GetComponent<GenericCreature>();
        isPlayer = creature.isPlayer;
        animator = GetComponent<Animator>();
    }

    public void AttackSequence() {
        //if (!isPlayer) return;
        Debug.Log(gameObject + " AttackSequence");
        TryStartAttack();
    }

    void TryStartAttack() {
        Debug.Log(gameObject + "TryStartAttack");
        if (Time.time - lastAttackTime < attackCooldown) {
            Debug.Log(gameObject + "AttackOnCooldown");
            return;
        }
        lastAttackTime = Time.time;
        canAttack = false;
        //GetComponent<Controller>().enabled = false;

        //if (animator != null) {
            //Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            //rb2d.linearVelocity = Vector2.zero;
            Debug.Log(gameObject + "AttackAnim");
            animator.SetTrigger("Attack");
       // }

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown() {
        yield return new WaitForSeconds(attackCooldown);
        //Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        //rb2d.constraints = RigidbodyConstraints2D.None;
        //GetComponent<Controller>().enabled = true;
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!canAttack) return;

        var targetCreature = other.GetComponent<GenericCreature>();
        if (targetCreature == null) return;
        if (targetCreature.isPlayer == isPlayer) { Debug.Log("Target is the same isPlayer"); return; }
        var targetHealth = other.GetComponent<Health>();

        if (targetHealth == null) { Debug.Log("Target " + targetCreature + " Health Null"); return; }

        //if (targetCreature.CompareTag("Player")) {
        //    animator.SetTrigger("Attack");
        //}

        //targetHealth.TakeDamage(attackDamage, transform.position);
    }
}
