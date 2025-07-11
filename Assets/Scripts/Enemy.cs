using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI.Table;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    [SerializeField] float HPLimit = 50f;
    [SerializeField] float probabilityToHeal = 0.5f;
    [SerializeField] float visionRange = 10f;

    TagsEnum tagEnum = TagsEnum.Enemy;
    Transform player;
    NavMeshAgent agent;
    State currentState;
    CaptureZone captureZone;

    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.GetPlayer();
        agent = GetComponent<NavMeshAgent>();
        captureZone = GameManager.Instance.GetCaptureZone();
        currentState = new Goal(gameObject, agent, player);
    }

    protected override void Update()
    {
        base.Update();
        See();
        AlwaysToCheck();
        currentState = currentState.Process();
    }

    void See()
    {
        if (Vector3.Distance(player.position, transform.position) < visionRange && CanSeePlayer())
        {
            Rotate();
            Shoot();
            return;
        }
        agent.updateRotation = true;
    }

    void Shoot()
    {
        if (!enableShoot || !IsAlive) return;
        
        /*Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float angle = Vector3.Angle(firePoint.up, directionToPlayer);
        if (angle > 15f) return;*/
        
        enableShoot = false;
        Instantiate(projectilePrefab, firePoint.position, gun.rotation);
    }

    private void Rotate()
    {
        agent.updateRotation = false;
        RotateTransform(player.position, transform);
        RotateTransform(player.position, gun);
    }

    private void RotateTransform(Vector3 hitPoint, Transform toRotate)
    {
        Vector3 direction = (hitPoint - toRotate.position).normalized;
        direction.y = 0f; // mantieni solo rotazione orizzontale

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            toRotate.rotation = Quaternion.Slerp(toRotate.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    bool CanSeePlayer()
    {
        if (Physics.Raycast(firePoint.position, (player.position - firePoint.position).normalized, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
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