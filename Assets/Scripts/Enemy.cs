using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : Entity
{
    [SerializeField] float hpLimitToHeal = 20f; // Limite di salute per decidere se curarsi
    [SerializeField] float probabilityToGoHeal = 0.1f; // Probabilità di andare a curarsi

    CaptureZone captureZone;
    NavMeshAgent agent;
    Vector3 target;

    protected override void Start()
    {
        base.Start();
        captureZone = GameManager.Instance.GetCaptureZone();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
        AlwaysToCheck();
        Move();
    }

    private void Move()
    {
        if (target == null) return;
        agent.SetDestination(target);
    }

    void AlwaysToCheck()
    {
        if (!captureZone.IsThereAnEntityInZone(TagsEnum.Enemy))
        {
            target = captureZone.transform.position;
        }
        else if (currentHealth <= hpLimitToHeal && Random.value < probabilityToGoHeal)
        {
            target = GameManager.Instance.GetNearestHealPoint(transform.position).position;
        }
    }
}
