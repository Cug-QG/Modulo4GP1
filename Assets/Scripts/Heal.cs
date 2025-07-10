using UnityEngine;
using UnityEngine.AI;

public class Heal : State
{
    Vector3 targetPosition;

    public Heal(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.HEAL;
    }

    public override void Update()
    {
        base.Update();
        targetPosition = GameManager.Instance.GetNearestHealPoint(npc.transform.position);
        agent.SetDestination(targetPosition);
    }
}
