using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityAttack : MonoBehaviour {

    Collider2D weaponCollider;
    Creature creature;
    Animator animator;


    [SerializeField] float attackDamage = 10f;

    IEnumerator attackCoroutine;
    bool isPlayer = false;


    void Start() {
        isPlayer = GetComponent<Creature>().isPlayer;

        //if (animator != null) {
            Debug.Log("Animator Set");
            animator = GetComponent<Animator>();
        //}


        weaponCollider = GetComponentInChildren<Collider2D>();

    }

    // Update is called once per frame
    void Update() {



    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (!isPlayer && other.CompareTag("Player")) {

            other.GetComponent<Creature>().TakeDamage(attackDamage);
        }
        else if (isPlayer && other.CompareTag("Enemy")) {
            other.GetComponent<Creature>().TakeDamage(attackDamage);
        }

    }

    void OnAttack() {
        if (!isPlayer) return;
        Debug.Log("Attack");
        animator.SetTrigger("isAttacking");
    }

}
