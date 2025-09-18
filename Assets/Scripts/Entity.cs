using UnityEngine;

public class Entity : MonoBehaviour {


    [Header("Stats")]
    //[SerializeField] float baseDamage = 10f;
    [SerializeField] float baseArmor = 10f;
    float extraArmor = 0f;
    float totalArmor;

    [SerializeField] float baseHealth = 100f;
    float extraHealth = 0f;
    float maxHealth = 100f;
    float currentHealth = 100f;

    public bool isPlayer = false;

    void Start() {
        maxHealth = baseHealth + extraHealth;
        currentHealth = maxHealth;
        totalArmor = baseArmor + extraArmor;


    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(float amount) {
        if (currentHealth < amount) {
            Death();
        }
        else {
            currentHealth =- CalculateResistance(amount);
            Debug.Log("Current hp = " + currentHealth);
        }
    }
    void Death() {

    }
    float CalculateResistance(float incomingDamageAmount) {
        float flatDamageRes = totalArmor / 10;
        Debug.Log("Damage Reduce by = " + flatDamageRes);
        float finalDamage = incomingDamageAmount - flatDamageRes;
        Debug.Log("Damage taken = " +  finalDamage);
        return finalDamage;
    }
}
