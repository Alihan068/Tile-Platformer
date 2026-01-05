using System.Collections;
using UnityEngine;

public enum EnemyType {
    Walker,
    Flyer,
    Shooter
}

public class EnemyController : MonoBehaviour {

    public enum EnemyState {
        Walking,
        Attacking,
        Dead
    }

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float attackCooldown = 1f;

    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;
    EnemyAttack enemyAttack;
    Creature creature;

    EnemyState currentState = EnemyState.Walking;
    float lastTargetSeenTime;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        creature = GetComponent<Creature>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    void FixedUpdate() {
        if (currentState == EnemyState.Dead || (creature != null && creature.IsKnockedBack)) return;

        if (currentState == EnemyState.Walking) {
            Walk();
            FlipEnemyFacing();
        }
    }

    public void TurnEntity() {
        if (currentState == EnemyState.Attacking) return;
        moveSpeed = -moveSpeed;
    }

    void FlipEnemyFacing() {
        if (Mathf.Abs(rb2d.linearVelocity.x) > 0.1f) {
            transform.localScale = new Vector2((Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
        }
    }

    void Walk() {
        rb2d.linearVelocity = new Vector2(moveSpeed, rb2d.linearVelocity.y);
    }

    public void StartAttackCoroutine(GameObject target) {
        lastTargetSeenTime = Time.time;

        if (currentState != EnemyState.Attacking) {
            StartCoroutine(AttackSequenceCoroutine());
        }
    }

    IEnumerator AttackSequenceCoroutine() {
        currentState = EnemyState.Attacking;
        rb2d.linearVelocity = Vector2.zero;

        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        do {
            enemyAttack.AttackAnim();
            yield return new WaitForSeconds(attackCooldown);

        } while (Time.time - lastTargetSeenTime < 0.5f);

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        currentState = EnemyState.Walking;
    }
}