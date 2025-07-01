using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] private float captureTime = 5f; // Time required to capture the zone
    float enemyCaptureProgress = 0f;
    float playerCaptureProgress = 0f;
    List<Transform> entities = new();

    private void Update()
    {
        RemoveNullTargets();
        if (entities.Count == 0) return;
        if (!CheckIfDominant()) return;
        CaptureZoneProgress();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Player))
        {
            entities.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Player))
        {
            entities.Remove(other.transform);
        }
    }

    private void RemoveNullTargets()
    {
        entities.RemoveAll(target => target == null);
    }

    private bool CheckIfDominant()
    {
        string firstTag = entities[0].tag;

        foreach (Transform entity in entities)
        {
            if (!entity.CompareTag(firstTag)) { return false; }
        }

        return true;
    }

    private void CaptureZoneProgress()
    {
        if (entities[0].CompareTag(Tags.Enemy))
        {
            enemyCaptureProgress += Time.deltaTime;
            if (enemyCaptureProgress >= captureTime)
            {
                Debug.Log("Enemy captured the zone!");
            }
        }
        else if (entities[0].CompareTag(Tags.Player))
        {
            playerCaptureProgress += Time.deltaTime;
            if (playerCaptureProgress >= captureTime)
            {
                Debug.Log("Player captured the zone!");
            }
        }
    }
}
