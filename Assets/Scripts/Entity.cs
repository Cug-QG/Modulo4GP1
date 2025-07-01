using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    float currentHealth;
    [SerializeField] private float respawnCooldown = 3f;

    public bool IsAlive => currentHealth > 0;
    public bool CanHeal => IsAlive && currentHealth < maxHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth - 50;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        //OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        //OnHealthChanged?.Invoke(currentHealth);
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnCooldown);

        //transform.position = respawnPoint.position;
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }
}
