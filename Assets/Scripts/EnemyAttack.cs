using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    Animator animator;

    [SerializeField] float damage = 15f;
    void Start() {
        animator = GetComponentInChildren<Animator>();
    }
    public void AttackAnim() {
        if (animator != null) animator.SetTrigger("isAttacking");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // this.transform yerine this.transform.position gönderiyoruz
            collision.GetComponentInParent<Creature>().TakeDamage(damage, this.transform.position);
        }
    }
}