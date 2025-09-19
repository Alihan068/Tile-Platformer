using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackCooldown = 0.5f;

    GenericCreature creature;
    Animator animator;

    bool canAttack = true;
    bool isPlayer;
    float lastAttackTime;

    void Start()
    {
        creature = GetComponent<GenericCreature>();
        isPlayer = creature != null && creature.isPlayer;
        animator = GetComponent<Animator>();
    }

    public void OnAttack()
    {
        if (!isPlayer) return;
        TryStartAttack();
    }

    void TryStartAttack()
    {
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
        if (targetCreature.isPlayer == isPlayer) return;

        var targetHealth = other.GetComponent<Health>();
        if (targetHealth == null) return;

        targetHealth.TakeDamage(attackDamage, transform.position);
    }
}
