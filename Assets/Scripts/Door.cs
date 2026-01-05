using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
    public bool playerHasKey;
    EdgeCollider2D edgeCollider;
    [SerializeField] SpriteRenderer openDoorSprite;
    [SerializeField] SpriteRenderer closeDoorSprite;

    void Start() {
        edgeCollider = GetComponent<EdgeCollider2D>();
        closeDoorSprite.enabled = true;
        openDoorSprite.enabled = false;
    }

    // Update is called once per frame
    void Update() {


    }
    public void PlayerKey() {
        playerHasKey = true;
        Debug.Log("Key Acquired");
    }
    void OpenDoor(bool door) {
        Debug.Log("Open Door!");
        edgeCollider.enabled = !door;
        closeDoorSprite.enabled = !door;
        openDoorSprite.enabled = door;
        playerHasKey = false;

    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && playerHasKey) {
            OpenDoor(true);
        }
    }
}
