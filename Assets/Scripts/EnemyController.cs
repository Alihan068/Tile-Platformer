using System.Drawing;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    //public float currentHp = 100f;
    //public float attackDamage = 5f;

    

    //ParticleSystem myParticleSystem;

    Rigidbody2D rb2d;
    void Start() {
       // myParticleSystem = GetComponent<ParticleSystem>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
            rb2d.linearVelocity = new Vector2(moveSpeed, rb2d.linearVelocity.y);      
    }

    private void OnTriggerExit2D(Collider2D collision) {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing() {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.linearVelocity.x)), 1f);
    }

    //public void TakeDamage(float damage) {
    //    currentHp -= damage;
    //    Debug.Log(damage + "Damage Taken");
    //    DamageEffects();
    //}
//    void DamageEffects() {
        
//        myParticleSystem.Play();
        
//    }
}

