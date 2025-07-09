using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else { instance = this; }
    }

    [SerializeField] Transform playerBase;
    [SerializeField] Transform enemyBase;
    [SerializeField] CaptureZone captureZone;
    [SerializeField] Transform[] healPoints;

    public void Respawn(Transform item, float delay)
    {
        StartCoroutine(RespawnRoutine(item, delay));
    }

    private IEnumerator RespawnRoutine(Transform item, float delay)
    {
        yield return new WaitForSeconds(delay);

        item.gameObject.SetActive(true);
    }

    public Transform GetBaseByTag(string tag)
    {
        if (tag == "Player")
        {
            return playerBase;
        }
        else if (tag == "Enemy")
        {
            return enemyBase;
        }
        else
        {
            Debug.LogError("Invalid tag provided: " + tag);
            return null;
        }
    }

    public CaptureZone GetCaptureZone()
    {
        return captureZone;
    }

    public Transform GetNearestHealPoint(Vector3 position)
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;
        foreach (Transform healPoint in healPoints)
        {
            float distance = Vector3.Distance(position, healPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = healPoint;
            }
        }
        return nearest;
    }
}
