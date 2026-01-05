using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Key")) {
            FindAnyObjectByType<Door>().PlayerKey();
            Destroy(other.gameObject);
        }
    }
}
