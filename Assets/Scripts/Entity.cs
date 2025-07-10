using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private float respawnCooldown = 3f;

    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] private float fireRate = 3f;
    float fireCooldown;
    protected bool enableShoot = true;

    public bool IsAlive => currentHealth > 0;
    public bool CanHeal => IsAlive && currentHealth < maxHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        fireCooldown = 1 / fireRate;
        transform.position = GameManager.Instance.GetBaseByTag(tag).position;
    }

    protected virtual void Update()
    {
        Cooldown();
    }

    private void Cooldown()
    {
        if (enableShoot) return;
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            enableShoot = true;
            fireCooldown = 1 / fireRate;
        }
    }


    public virtual void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        //OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public virtual void Heal(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        //OnHealthChanged?.Invoke(currentHealth);
    }

    private void HandleDeath()
    {
        transform.position = GameManager.Instance.GetBaseByTag(tag).position;
        GameManager.Instance.Respawn(transform, respawnCooldown);
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }
}
