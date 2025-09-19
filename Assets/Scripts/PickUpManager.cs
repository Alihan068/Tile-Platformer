using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Key")) {
            FindAnyObjectByType<Door>().PlayerKey();
            Destroy(other);
        }
    }
}
