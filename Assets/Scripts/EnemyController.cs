using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb2d;
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        rb2d.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing() {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.linearVelocity.x)), 1f);
    }
}
