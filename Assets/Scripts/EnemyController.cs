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
    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        Walk();
        FlipEnemyFacing();
    }

    public void TurnEntity() {
         moveSpeed = -moveSpeed;
    }

    void FlipEnemyFacing() {
        transform.localScale = new Vector2((Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
    }

    void Walk() {
        rb2d.linearVelocity = new Vector2(moveSpeed, rb2d.linearVelocity.y);
    }
}