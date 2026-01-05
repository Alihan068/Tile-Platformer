using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    Creature creature;
    Animator animator;
    [SerializeField] float attackDamage = 10f;

    void Awake() {
        creature = GetComponent<Creature>();
        animator = GetComponent<Animator>();

    }

    public void AttackMethod() {
        if (animator != null) animator.SetTrigger("isAttacking");

    }
}