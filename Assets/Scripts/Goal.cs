using UnityEngine;
using UnityEngine.AI;

public class Goal : State
{
    CaptureZone captureZone;

    public Goal(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.GOAL;
    }
    public override void Enter()
    {
        base.Enter();
        captureZone = GameManager.Instance.GetCaptureZone();
        agent.SetDestination(captureZone.transform.position);
    }

    public override void Update()
    {
        base.Update();
        agent.SetDestination(GameManager.Instance.GetCaptureZone().transform.position);
        if (captureZone.IsThereSpecificEntityInZone(npc.transform))
        {
            
            //stage = EVENT.EXIT;
        }
    }
}
