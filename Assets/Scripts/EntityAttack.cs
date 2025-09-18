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
    bool isPlayer;


    void Start() {
        isPlayer = GetComponent<Creature>().isPlayer;
        
        if (isPlayer) {
            weaponCollider = GetComponentInChildren<Collider2D>();           
        }
        else {
            weaponCollider = GetComponent<Collider2D>();
        }
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {



    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isPlayer) {
            other.GetComponent<Creature>().TakeDamage(attackDamage);
        }
        else if (isPlayer) {
            OnAttack(other);
        }

    }

    void OnAttack(Collider2D other) {
        other.GetComponent<Creature>().TakeDamage(attackDamage);
    }

}
