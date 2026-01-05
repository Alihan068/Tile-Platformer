using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Animator animator;

    void Start() {
        animator = GetComponentInChildren<Animator>();

    }
    public void AttackAnim()
    {
        if (animator != null) animator.SetTrigger("isAttacking");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<Creature>().TakeDamage(1, this.transform);
        }
    }
}
