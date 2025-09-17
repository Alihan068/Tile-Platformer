using UnityEngine;

public class CheckPoint : MonoBehaviour {
    Transform checkpointTransform;
    Vector2 transformValues;

    void Start() {
        transformValues = FindFirstObjectByType<GameSession>().checkpointPos;
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        transformValues = transform.position;
        Debug.Log(" Checkpoint! = " + transformValues);
    }
}
