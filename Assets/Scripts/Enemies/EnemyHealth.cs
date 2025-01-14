using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] EnemyStats stats;
    [SerializeField] AudioClip[] hurtClips;
    [SerializeField] AudioClip deathClip;
    private AudioSource source;
    private SpriteRenderer render;
    private Animator anim;
    private Rigidbody2D rb; 

    [Header("Timers")]
    [SerializeField] private float redTime;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float knockbackForce; 

    // Variables
    private float currentHealth;
    private bool canBeDamaged = true;
    private bool isDead = false;

    //Evento
    public UnityEvent OnDamageTaken;

    private void Start()
    {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); 
        ResetHealth();
    }

    public void TakeDamage(float damage)
    {
        if (!canBeDamaged) return;

        PlayHurtSound();
        currentHealth -= damage;
        CheckHealth();

        if (isDead) return;

        StartCoroutine(ChangeColor());
        StartCoroutine(InvincibleMode());

        OnDamageTaken?.Invoke();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
            Die();
    }

    public void DisableEnemy()
    {
        EnemyObjectPool.Instance.ReturnEnemyToPool(gameObject);
    }

    private AudioClip TakeRandomStepSound()
    {
        return hurtClips[Random.Range(0, hurtClips.Length)];
    }

    public void PlayHurtSound()
    {
        source.Stop();
        source.PlayOneShot(TakeRandomStepSound());
    }

    private IEnumerator ChangeColor()
    {
        Color damagedColor = Color.red;
        render.color = damagedColor;
        yield return new WaitForSeconds(redTime);
        render.color = Color.white;
    }

    private IEnumerator InvincibleMode()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(invincibleTime);
        canBeDamaged = true;
    }

    private void ResetHealth()
    {
        currentHealth = stats.MaxHealth;
        canBeDamaged = true;
        isDead = false;
    }

    private void OnEnable()
    {
        DayCycle.DayStartRelease += Die;
        ResetHealth();
    }

    private void OnDisable()
    {
        DayCycle.DayStartRelease -= Die;
    }

    private void Die()
    {
        isDead = true;
        canBeDamaged = false;
        anim.SetTrigger("Death");
        source.Stop();
        source.PlayOneShot(deathClip);
        Invoke(nameof(DisableEnemy), 4f);
    }

    public bool IsAlive()
    {
        throw new System.NotImplementedException();
    }
}
