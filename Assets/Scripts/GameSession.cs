using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] int pointsPerPickup = 100;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI hpText;

    [SerializeField] List<GameObject> checkPoints = new List<GameObject>();
    PlayerController playerController;


    Vector3 checkpointPos;

    private void Awake() {
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if (numGameSessions > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start() {
        playerController = FindFirstObjectByType<PlayerController>();
        hpText.text = "x" + playerLives.ToString();
        scoreText.text = "x" + score.ToString();
    }

    void Update() {

    }

    public void ProcessPlayerDeath() {
        if (playerLives > 1) {
            TakeLife();
        }
        else {
            ResetGameSession();
        }
    }

    void TakeLife() {
        playerLives--;
        hpText.text = "x" + playerLives.ToString();
        Invoke(nameof(ReloadLevel), 2f);
    }
    void ResetGameSession() {
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        //main menü vb var ise scene variable değiş
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    void ReloadLevel() {
        //Restart current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void AddToScore(int pointsToAdd) {
        score += pointsToAdd;
        scoreText.text = "x" + score.ToString();
    }
    public void SetPlayerSpawn() {
        playerController.transform.position = checkpointPos;
        //playerController.transform.position = value;
        //Debug.Log("Coordinates Set to = " + value);
    }


}
