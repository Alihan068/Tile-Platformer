using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour {
    static Vector2 savedPosOfThePlayer; // store the position of the player 

    static bool hasCheckpoint;

    static PlayerController playerController;

    static CheckPoint() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        playerController = FindFirstObjectByType<PlayerController>();
        savedPosOfThePlayer = playerController.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        savedPosOfThePlayer = transform.position;
        hasCheckpoint = true;
        Debug.Log(" Checkpoint! = " + savedPosOfThePlayer);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!hasCheckpoint) return;

        var player = FindFirstObjectByType<PlayerController>();
        player.transform.position = savedPosOfThePlayer;
    }
}
