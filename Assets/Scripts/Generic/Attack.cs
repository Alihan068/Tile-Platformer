using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour {
    [Header("Attack Settings")]
    [HideInInspector] public float totalAttack;
    [SerializeField] float baseAttackDamage = 10f;
    [SerializeField] float extraAttackDamage = 0f;
    [SerializeField] float attackMultiplier = 1f;
    [SerializeField] float attackCooldown = 0.5f;

    Collider2D weaponCollider;
    Animator animator;

    Controller controller;

    bool canAttack = true;
    bool isPlayer;
    float lastAttackTime;
    float saveMoveSpeed;

    void Awake() {
        controller = GetComponent<Controller>();
        weaponCollider = GetComponentInChildren<Collider2D>();
        isPlayer = controller.isPlayer;
        animator = GetComponent<Animator>();
    }

    public void AttackSequence() {
        //if (!isPlayer) return;
        //Debug.Log(gameObject + " AttackSequence");
        TryStartAttack();

    }

    void TryStartAttack() {
        if (Time.time - lastAttackTime < attackCooldown) {
            return;
        }
        lastAttackTime = Time.time;
        canAttack = false;
        animator.SetTrigger("Attack");
        if (isPlayer) return;
        saveMoveSpeed = controller.moveSpeed;
        controller.moveSpeed = 0f;


        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        if (!isPlayer) {
            controller.moveSpeed = saveMoveSpeed;
        }

    }

    public float TotalAttackCalculator() {
        totalAttack = ((baseAttackDamage + extraAttackDamage) * attackMultiplier);
        Debug.Log(gameObject + "Attack power = " + totalAttack);
        return totalAttack;
    }
    //void OnTriggerEnter2D(Collider2D other) {

    //    var targetCreature = other.GetComponent<Controller>();
    //    if (targetCreature == null) return;
    //    if (targetCreature.isPlayer == isPlayer) { Debug.Log("Target is the same isPlayer"); return; }
    //    var targetHealth = other.GetComponent<Health>();

    //    if (targetHealth == null) { Debug.Log("Target " + targetCreature + " Health Null"); return; }

    //    //if (targetCreature.CompareTag("Player")) {
    //    //    animator.SetTrigger("Attack");
    //    //}

    //    //targetHealth.TakeDamage(attackDamage, transform.position);
    //}
}
