using UnityEngine;

public class CheckPoint : MonoBehaviour {
    Transform checkPointTransform;

    void Start() {
        checkPointTransform = FindFirstObjectByType<PlayerController>().checkpointTransform;

    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log(" Checkpoint! = " + transform.position);
            checkPointTransform.position = transform.position;

        }

    }
}
