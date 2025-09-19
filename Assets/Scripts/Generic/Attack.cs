using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackCooldown = 0.5f;

    Collider2D weaponCollider;
    GenericCreature creature;
    Animator animator;

    bool canAttack = true;
    bool isPlayer;
    float lastAttackTime;

    void Start()
    {
        weaponCollider = GetComponentInChildren<Collider2D>();
        creature = GetComponent<GenericCreature>();
        isPlayer = creature != null && creature.isPlayer;
        animator = GetComponent<Animator>();
    }

    public void OnAttack()
    {
        if (!isPlayer) return;
        Debug.Log("OnAttack");
        TryStartAttack();
    }

    void TryStartAttack()
    {
        Debug.Log("TryStartAttack");
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        canAttack = false;

        if (animator != null)
            animator.SetTrigger("Attack");

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canAttack) return;

        var targetCreature = other.GetComponent<GenericCreature>();
        if (targetCreature == null) return;
        if (targetCreature.isPlayer == isPlayer) { Debug.Log("Target is the same isPlayer"); return; }
        var targetHealth = other.GetComponent<Health>();

        if (targetHealth == null) { Debug.Log("Target " + targetCreature + " Health Null"); return; }

        if (targetCreature.CompareTag("Player")) {
            animator.SetTrigger("Attack");
        }

        //targetHealth.TakeDamage(attackDamage, transform.position);
    }
}
