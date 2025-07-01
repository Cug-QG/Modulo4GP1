using System.Collections;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] float healthAmount = 20f;
    [SerializeField] float respawnTime = 5f;
    public bool isAvailable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAvailable) return;

        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.CanHeal)
        {
            isAvailable = false;
            entity.Heal(healthAmount);
            GameManager.Instance.Respawn(transform, respawnTime);
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        isAvailable = true;
    }
}
