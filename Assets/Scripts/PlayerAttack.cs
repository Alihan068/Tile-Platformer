using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float playerAttackCd;
    [SerializeField] float startAttackTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayerAttackSpeed() {
        if (playerAttackCd <= 0) { 
            //then attack
            playerAttackCd = startAttackTime;
        }
    }
}
