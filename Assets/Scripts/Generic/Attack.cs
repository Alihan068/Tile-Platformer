using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackCooldown = 0.5f;

    [Header("Hit Source")]
    [SerializeField] Collider2D[] weaponTriggers;

    [Header("Hit Filtering")]
    [SerializeField] LayerMask damageableLayers;

    Controller controller;
    Animator animator;
    Rigidbody2D rb2d;

    bool canAttack = true;
    bool isPlayer;
    float lastAttackTime;

    void Start()
    {
        controller = GetComponent<Controller>();
        isPlayer = controller != null && controller.IsPlayer;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        Debug.Log($"[Attack:{name}] isPlayer={isPlayer}, weaponTriggers={(weaponTriggers == null ? 0 : weaponTriggers.Length)}, damageableMask={damageableLayers.value}");

        if (weaponTriggers != null && weaponTriggers.Length > 0)
        {
            var myCols = GetComponentsInParent<Collider2D>(true);
            foreach (var wt in weaponTriggers)
            {
                if (!wt) continue;
                for (int i = 0; i < myCols.Length; i++)
                {
                    var c = myCols[i];
                    if (!c || c == wt) continue;
                    Physics2D.IgnoreCollision(wt, c, true);
                }
            }
        }

    }

    public void OnAttack()
    {
        if (!isPlayer) return;
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        canAttack = false;
        if (animator) animator.SetTrigger("Attack");
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var otherCtrl = other.GetComponentInParent<Controller>();
        if (otherCtrl == null) return;

        if (controller != null && otherCtrl == controller) return;

        bool fromMyWeapon = false;
        if (weaponTriggers != null)
        {
            for (int i = 0; i < weaponTriggers.Length; i++)
            {
                var wt = weaponTriggers[i];
                if (!wt || !wt.isActiveAndEnabled) continue;
                if (wt.IsTouching(other)) { fromMyWeapon = true; break; }
            }
        }

        if (!fromMyWeapon) return;

        if (controller != null && otherCtrl.IsPlayer == controller.IsPlayer) return;

        var targetHealth = otherCtrl.GetComponent<Health>() ?? other.GetComponentInParent<Health>();
        if (targetHealth == null) return;

        targetHealth.TakeDamage(attackDamage, transform.position);
    }
}
