using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    [SerializeField] int pointsPerPickup = 100;

    bool wasCollected = false;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !wasCollected) {
            wasCollected = true;
            if (coinPickupSFX != null)
                AudioSource.PlayClipAtPoint(coinPickupSFX, UnityEngine.Camera.main.transform.position);

            FindAnyObjectByType<GameSession>().AddToScore(pointsPerPickup);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
    }
}
