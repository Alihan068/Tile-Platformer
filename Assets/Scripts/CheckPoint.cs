using UnityEngine;

public class CheckPoint : MonoBehaviour {
    Transform checkpointTransform;
    Vector3 checkpointTransformValues; //store checkpoint gameobject position
    Vector3 savedPosOfPlayer; //store position of player
    Controller controller;

    void Start() {
        controller = FindFirstObjectByType<Controller>();
        savedPosOfPlayer = controller.transform.position;
        Debug.Log("SavedPosOfPlayer = " + savedPosOfPlayer);
        checkpointTransformValues = transform.position;
    }

    // Update is called once per frame
    void Update() {

        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        savedPosOfPlayer = checkpointTransformValues;
        Debug.Log(" Checkpoint! = " + checkpointTransformValues);
    }
}
