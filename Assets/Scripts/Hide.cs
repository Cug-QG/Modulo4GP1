using UnityEngine;
using UnityEngine.AI;

public class Hide : State
{
    CaptureZone captureZone;
    float raycastIntervalAngle = 5f;
    float rayDistance = 10f;
    public Hide(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.HIDE;
    }

    public override void Enter()
    {
        base.Enter();
        captureZone = GameManager.Instance.GetCaptureZone();
    }

    public override void Update()
    {
        base.Update();
        TryHide();
        if (!captureZone.IsThereSpecificEntityInZone(player))
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public void TryHide()
    {
        Vector3 toEnemy = npc.transform.position - player.position;
        float baseAngle = Mathf.Atan2(toEnemy.z, toEnemy.x) * Mathf.Rad2Deg;

        for (float offset = 0; offset < 180f; offset += raycastIntervalAngle)
        {
            foreach (int sign in new int[] { 1, -1 })
            {
                float angle = baseAngle + sign * offset;
                Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector3 targetPoint = player.position + dir * rayDistance;

                if (Physics.Raycast(player.position + Vector3.up, dir, out RaycastHit hit, rayDistance, 1 << LayerMask.NameToLayer("Obstacle")))
                {
                    Collider obstacle = hit.collider;
                    Vector3 obstacleCenter = obstacle.bounds.center;

                    // Direzione opposta rispetto al player
                    Vector3 throughDirection = (obstacleCenter - player.position).normalized;

                    // Ray dall’ostacolo verso lato opposto
                    if (Physics.Raycast(obstacleCenter, throughDirection, out RaycastHit backHit, rayDistance, 1 << LayerMask.NameToLayer("Obstacle")) == false)
                    {
                        // Niente altro ostacolo: punto potenziale dove nascondersi
                        Vector3 hideSpot = obstacleCenter + throughDirection * 1.5f;

                        if (Vector3.Distance(hideSpot, captureZone.transform.position) > captureZone.GetCaptureRadius()) continue;

                        NavMeshPath path = new NavMeshPath();
                        if (NavMesh.CalculatePath(npc.transform.position, hideSpot, NavMesh.AllAreas, path)
                            && path.status == NavMeshPathStatus.PathComplete)
                        {
                            agent.SetDestination(hideSpot);

                            Debug.DrawLine(player.position + Vector3.up, hit.point, Color.yellow, 1f);
                            Debug.DrawLine(obstacleCenter, hideSpot, Color.green, 1f);
                            return;
                        }
                    }
                }
            }
        }
    }

}
