using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    [SerializeField] float HPLimit = 50f;
    [SerializeField] float probabilityToHeal = 0.5f;

    TagsEnum tagEnum = TagsEnum.Enemy;
    Transform player;
    NavMeshAgent agent;
    State currentState;
    CaptureZone captureZone;
    float radius;

    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.GetPlayer();
        agent = GetComponent<NavMeshAgent>();
        captureZone = GameManager.Instance.GetCaptureZone();
        radius = captureZone.GetCaptureRadius();
        currentState = new Goal(gameObject, agent, player);
    }

    protected override void Update()
    {
        base.Update();
        AlwaysToCheck();
        currentState = currentState.Process();
    }

    void AlwaysToCheck()
    {
        if (currentState.name != State.STATE.GOAL && !captureZone.IsThereAnEntityInZone(tagEnum))
        {
            currentState = new Goal(gameObject, agent, player);
        }
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (TryHeal()) { currentState = new Heal(gameObject, agent, player); return; }
        currentState = new Goal(gameObject, agent, player);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (TryHeal()) currentState = new Heal(gameObject, agent, player);
    }

    bool TryHeal()
    {
        if (captureZone.HowManyEntitiesInZone(tagEnum) < 2) return false;
        if (IsAlive && currentHealth < HPLimit && Random.value < probabilityToHeal)
        {
            return true;
        }
        return false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentState = new Goal(gameObject, agent, player);
    }
}