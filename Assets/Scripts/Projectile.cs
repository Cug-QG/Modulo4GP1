using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] TagsEnum targetTag;
    [SerializeField] float damage = 10f;

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignora altri proiettili
        if (other.GetComponent<Collider>().isTrigger)
            return;

        // Se colpisce il bersaglio corretto
        if ((targetTag == TagsEnum.Enemy && other.CompareTag("Enemy")) ||
            (targetTag == TagsEnum.Player && other.CompareTag("Player")))
        {
            if (other.TryGetComponent<Entity>(out var target))
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        // Altrimenti, distruggiti su tutto tranne:
        // - altri proiettili (già gestito sopra)
        // - entità "alleate" (cioè, NON il target)
        if ((targetTag == TagsEnum.Enemy && other.CompareTag("Player")) ||
            (targetTag == TagsEnum.Player && other.CompareTag("Enemy")))
        {
            // Non fare nulla (colpito alleato)
            return;
        }

        // Qualsiasi altro oggetto (es. muri, ambiente)
        Destroy(gameObject);
    }
}
