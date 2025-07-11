using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    CaptureZone captureZone;
    float radius = 5f;
    Vector3 target;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.PATROL;
    }

    public override void Enter()
    {
        base.Enter();
        captureZone = GameManager.Instance.GetCaptureZone();
        radius = captureZone.GetCaptureRadius();
        target = GetRandomPointInZone();
        agent.SetDestination(target);
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(npc.transform.position, target) < 1.2f)
        {
            target = GetRandomPointInZone();
            agent.SetDestination(target);
        }

        //if player is in capture zone: chase / hide
        if (captureZone.IsThereSpecificEntityInZone(player))
        {
            if (captureZone.GetDominant() == TagsEnum.Enemy)
            {
                nextState = new Hide(npc, agent, player);
                stage = EVENT.EXIT;
                return;
            }
            nextState = new Chase(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    Vector3 GetRandomPointInZone()
    {
        Vector3 randomPoint;
        NavMeshPath path = new NavMeshPath();
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            randomPoint = captureZone.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        } while (agent.CalculatePath(randomPoint, path) && path.status != NavMeshPathStatus.PathComplete);

        return randomPoint; // fallback
    }
}
