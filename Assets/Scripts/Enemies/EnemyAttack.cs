using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected float hitDamage;
    [SerializeField] protected float raycastLength;
    [SerializeField] protected LayerMask damageableLayer;


    protected float attackTimer;
    protected bool canAttack;
    [SerializeField] float attackDelay;

    EnemyBrainController controller;

    private void Start()
    {
        attackTimer = attackDelay;
        controller = GetComponent<EnemyBrainController>();
    }

    private void Update()
    {
        CheckAttackDelay();
    }

    public virtual void Attack()
    {
        if (!canAttack) return;

        // Lanzar un Raycast hacia la derecha desde la posici�n del objeto
        Vector2 raycastDirection = transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, damageableLayer);

        // Si el Raycast golpea algo
        if (hit.collider != null)
        {
            // Intenta obtener el componente IDamageable del objeto golpeado
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            
            // Si el objeto golpeado implementa la interfaz IDamageable

            // Llama al m�todo TakeDamage() en el objeto
            damageable?.TakeDamage(hitDamage);
        }

        canAttack = false;
        controller.TryUpdateTarget();
    }

    protected virtual void CheckAttackDelay()
    {
        // Comprueba si el temporizador de ataque ha llegado a cero y si el enemigo puede atacar nuevamente.
        if (attackTimer <= 0 && !canAttack)
        {
            attackTimer = Mathf.Max(0, attackDelay); // Evita valores negativos
            canAttack = true;
        }

        attackTimer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
