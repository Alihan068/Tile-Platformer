using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] float attackDamage = 15f;
    [SerializeField] float timeBetweenAttack = 1f;
    public Vector2 knockbackAmount;
    [SerializeField] float knockbackForce = 1f;
    float startTimeBetweenAttack = 1f;


    public Transform attackPos;
    public float attackRangeX;
    public float attackRangeY;
    CircleCollider2D circleCollider;
    bool isAttacked;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        PlayerAttackMove();

    }

    void PlayerAttackMove() {
        if (timeBetweenAttack <= 0) {
            if (Input.GetKey(KeyCode.E)) {
                //then attack
                ExecuteAttack();
                timeBetweenAttack = startTimeBetweenAttack;
            }
        }
        else {
            timeBetweenAttack -= Time.deltaTime;
        }
    }
    public void ExecuteAttack() {
        Debug.Log("Attack!");
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPos.transform.position, new Vector2 (attackRangeX, attackRangeY), LayerMask.GetMask("Enemies"));
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].GetComponent<EnemyController>().TakeDamage(attackDamage);
            Rigidbody2D rb2d = enemies[i].GetComponent<EnemyController>().GetComponent<Rigidbody2D>();
            KnockbackCalculator(rb2d);
            Debug.Log("Hit Enemy");
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3 (attackRangeX, attackRangeY)); 
    }
    void KnockbackCalculator(Rigidbody2D target) {
        knockbackForce += attackDamage / 100;
        Vector2 direction = (target.transform.position - this.transform.position).normalized;
        knockbackAmount = direction * knockbackForce;
        target.AddForce(knockbackAmount, ForceMode2D.Impulse);
    }
    void ResetAttack() {

    }

}
