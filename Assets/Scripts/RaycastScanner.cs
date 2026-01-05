using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastScanner : MonoBehaviour {
    [System.Serializable]
    public class RaycastConfig {
        public string configName = "Scanner";
        public float detectionRange = 5f;
        public float maxAngle = 45f;
        public float minAngle = -45f;
        public int rayCount = 5;
        public Vector2 rayOriginOffset = Vector2.zero;
        public LayerMask targetLayer;

        public bool inverted;
        public bool continuous;
        public float cooldown = 0.5f;

        public Color gizmoColor = Color.red;
        public UnityEvent<GameObject> onHit;

        [System.NonSerialized] public float nextFireTime;
        [System.NonSerialized] public bool hasFired;
    }

    [SerializeField] List<RaycastConfig> scanners;

    void Update() {
        if (scanners == null) return;

        foreach (var config in scanners) {
            CastRays(config);
        }
    }

    void CastRays(RaycastConfig config) {
        if (Time.time < config.nextFireTime) return;

        float facingDir = Mathf.Sign(transform.localScale.x);
        Vector3 calculatedOrigin = transform.position + new Vector3(config.rayOriginOffset.x * facingDir, config.rayOriginOffset.y, 0);
        float angleStep = config.rayCount > 1 ? (config.maxAngle - config.minAngle) / (config.rayCount - 1) : 0;

        GameObject detectedObject = null;
        bool anyHit = false;

        for (int i = 0; i < config.rayCount; i++) {
            float currentAngle = config.maxAngle - (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * Vector2.right * facingDir;

            RaycastHit2D hit = Physics2D.Raycast(calculatedOrigin, direction, config.detectionRange, config.targetLayer);

            if (hit.collider != null) {
                anyHit = true;
                detectedObject = hit.collider.gameObject;
                break;
            }
        }

        bool conditionMet = (config.inverted && !anyHit) || (!config.inverted && anyHit);

        if (conditionMet) {
            if (config.continuous) {
                config.onHit.Invoke(detectedObject);
                config.nextFireTime = Time.time + config.cooldown;
            }
            else {
                if (!config.hasFired) {
                    config.onHit.Invoke(detectedObject);
                    config.hasFired = true;
                    config.nextFireTime = Time.time + config.cooldown;
                }
            }
        }
        else {
            config.hasFired = false;
        }
    }

    void OnDrawGizmos() {
        if (scanners == null) return;

        foreach (var config in scanners) {
            Gizmos.color = config.gizmoColor;

            float facingDir = Mathf.Sign(transform.localScale.x);
            Vector3 calculatedOrigin = transform.position + new Vector3(config.rayOriginOffset.x * facingDir, config.rayOriginOffset.y, 0);

            float angleStep = config.rayCount > 1 ? (config.maxAngle - config.minAngle) / (config.rayCount - 1) : 0;

            for (int i = 0; i < config.rayCount; i++) {
                float currentAngle = config.maxAngle - (angleStep * i);
                Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.right * facingDir;

                Gizmos.DrawLine(calculatedOrigin, calculatedOrigin + direction * config.detectionRange);
            }
        }
    }
}