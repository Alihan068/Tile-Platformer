using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
    public bool playerHasKey;
    EdgeCollider2D edgeCollider;
    [SerializeField] SpriteRenderer openDoorSprite;
    [SerializeField] SpriteRenderer closeDoorSprite;

    void Start() {
        edgeCollider = GetComponent<EdgeCollider2D>();

    }

    // Update is called once per frame
    void Update() {


    }
    public void PlayerKey() {
        playerHasKey = true;
        Debug.Log("Key Acquired");
    }
    void OpenDoor(bool open) {
        Debug.Log("Open Door!");
        edgeCollider.enabled = !open;
        closeDoorSprite.enabled = !open;
        openDoorSprite.enabled = open;
        playerHasKey = false;

    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && playerHasKey) {
            OpenDoor(true);
        }
    }
}
