using Unity.Cinemachine;
using UnityEngine;

public class CreatureDeath : MonoBehaviour {
    GenericCreature creature;
    Rigidbody2D rb2d;
    GameSession gameSession;
    Controller controller;

    bool isPlayer;
    [SerializeField] Vector2 deathKick = new Vector2(10, 10);
    [SerializeField] float deathSpin = 5f;

    void Start() {
        isPlayer = creature.isPlayer;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void DeathSequence() {
        Debug.Log($"{gameObject.name} has died!");

        if (creature != null && !creature.isPlayer) {
            DeathEffects();
            Destroy(gameObject, 0.5f);
        }
        else {
            DeathEffects();
            gameSession.ProcessPlayerDeath();
        }
    }
    void DeathEffects() {
        if (isPlayer) {
            //Stop Camera on death area
            FindAnyObjectByType<CinemachineCamera>().enabled = false;
        }

        //RedColorBlink
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetSpriteColor), 0.2f);
        //Disable Colliders
        Collider2D[] collider2Ds = GetComponents<Collider2D>();
        foreach (Collider2D col in collider2Ds) {
            col.enabled = false;
        }
        //DeathSpin
        //Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = deathKick;
        rb2d.freezeRotation = false;
        rb2d.AddTorque(deathSpin, ForceMode2D.Impulse);

        Invoke(nameof(StopSpin), 2f);

        Invoke(nameof(DestroyPlayer), 5f);
    }
    void ResetSpriteColor() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    void StopSpin() {
        rb2d.angularVelocity = 0f;
    }
    void DestroyPlayer() {
        Destroy(gameObject);
    }
}
