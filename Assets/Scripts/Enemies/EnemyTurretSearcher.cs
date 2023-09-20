using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretSearcher : MonoBehaviour
{
    EnemyBrainController controller;
    public float detectionRadius;
    public LayerMask turretLayer;

    List<GameObject> turrets = new List<GameObject>();
    GameObject currentTurret;
    float closeDistance;

    private void Start()
    {
        controller = GetComponent<EnemyBrainController>();
        closeDistance = Mathf.Infinity;
    }

    private void Update()
    {
        // Detectar objetos en el radio.
        Collider2D[] turrets = Physics2D.OverlapCircleAll(transform.position, detectionRadius, turretLayer);

        foreach (Collider2D turretCollider in turrets)
        {
            // Obtener el GameObject asociado al Collider.
            GameObject turret = turretCollider.gameObject;

            // Verificar si la torreta est� activa antes de proceder.
            if (turret.activeSelf)
            {
                // Calcular la distancia entre el jugador y la torreta.
                float distance = Vector3.Distance(transform.position, turret.transform.position);

                // Comprobar si esta torreta est� m�s cerca que la anteriormente almacenada.
                if (distance < closeDistance)
                {
                    closeDistance = distance;
                    currentTurret = turret;
                    controller.turret = currentTurret;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

}
