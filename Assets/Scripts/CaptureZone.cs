using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [Header("Capture Zone Settings")]
    [SerializeField] float captureRadius = 5f;
    [SerializeField] float captureTime = 5f;
    [SerializeField] float sphereRadius;

    float enemyCaptureProgress = 0f;
    float playerCaptureProgress = 0f;
    public List<Transform> entities = new();
    TagsEnum dominant;

    [Header("Visual settings")]
    [SerializeField] float planeY = 0.1f;
    [SerializeField] int segments = 64;

    private LineRenderer lr;

    void Start()
    {
        GetComponent<SphereCollider>().radius = captureRadius;

        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = true;
        lr.positionCount = segments;
        sphereRadius = GetComponent<SphereCollider>().radius;
    }

    private void Update()
    {
        Draw();

        RemoveNullTargets();
        if (entities.Count == 0) return;
        CaptureZoneProgress();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Player))
        {
            entities.Add(other.transform);
            CheckIfDominant();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Player))
        {
            entities.Remove(other.transform);
            CheckIfDominant();
        }
    }

    private void RemoveNullTargets()
    {
        entities.RemoveAll(target => !target.gameObject.activeSelf);
    }

    private void CheckIfDominant()
    {
        TagsEnum entityTag = entities[0].CompareTag("Player") ? TagsEnum.Player : TagsEnum.Enemy;

        if (entities.Count == 1) dominant = entityTag;

        string firstTag = entities[0].tag;

        foreach (Transform entity in entities)
        {
            if (!entity.CompareTag(firstTag)) { return; }
        }

        dominant = entityTag;
    }

    private void CaptureZoneProgress()
    {
        switch (dominant)
        {
            case TagsEnum.Enemy:
                HandleCapture(ref enemyCaptureProgress, TagsEnum.Enemy);
                break;
            case TagsEnum.Player:
                HandleCapture(ref playerCaptureProgress, TagsEnum.Player);
                break;
        }
    }

    private void HandleCapture(ref float progress, TagsEnum tag)
    {
        progress = Mathf.Min(progress + Time.deltaTime, captureTime);
        UIManager.Instance.SetCaptureScore(progress / captureTime, tag);

        if (progress >= captureTime)
        {
            Debug.Log(tag + " captured the zone!");
        }
    }

    void Draw()
    {
        Vector3 center = transform.position;
        float h = Mathf.Abs(center.y - planeY);

        if (h > sphereRadius)
        {
            lr.enabled = false;
            return;
        }

        float contactRadius = Mathf.Sqrt(sphereRadius * sphereRadius - h * h);
        Vector3 circleCenter = new Vector3(center.x, planeY, center.z);
        lr.enabled = true;

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2;
            Vector3 point = circleCenter + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * contactRadius;
            lr.SetPosition(i, point);
        }
    }
}
