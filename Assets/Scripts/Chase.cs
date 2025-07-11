using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{
    CaptureZone captureZone;

    public Chase(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.CHASE;
    }
    public override void Enter()
    {
        base.Enter();
        captureZone = GameManager.Instance.GetCaptureZone();
    }

    public override void Update()
    {
        base.Update();
        if (captureZone.IsThereSpecificEntityInZone(player))
        {
            agent.SetDestination(player.position);
        }
        else
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }
}
