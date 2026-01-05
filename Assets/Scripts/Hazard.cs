using UnityEngine;

public class Hazard : MonoBehaviour {
    [SerializeField] int damageAmount = 10;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Creature playerCreature = collision.gameObject.GetComponentInParent<Creature>();
            if (playerCreature != null) {

                Vector3 hitPoint = collision.GetContact(0).point;

                playerCreature.TakeDamage(damageAmount, hitPoint);
            }
        }
    }
}