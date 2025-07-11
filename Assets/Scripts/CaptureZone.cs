using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [Header("Capture Zone Settings")]
    [SerializeField] float captureRadius = 5f;
    [SerializeField] float captureTime = 5f;
    [SerializeField] float dominiumTime = 5f;
    [SerializeField] float sphereRadius;

    float enemyCaptureProgress = 0f;
    float playerCaptureProgress = 0f;
    public List<Transform> entities = new();
    TagsEnum? contender;
    TagsEnum? dominant;
    float captureProgress = 0f;
    bool isPaused = false;

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
        //if (entities.Count == 0) return;
        CaptureZoneProgress();
        TakeControl();
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
        entities.RemoveAll(target => !target.gameObject.activeSelf);
        CheckIfDominant();
    }

    private void CheckIfDominant()
    {
        if (entities.Count == 0)
        {
            if (contender != null) previousContender = contender;
            contender = null;
            return;
        }

        TagsEnum entityTag = entities[0].CompareTag("Player") ? TagsEnum.Player : TagsEnum.Enemy;

        string firstTag = entities[0].tag;

        foreach (Transform entity in entities)
        {
            if (!entity.CompareTag(firstTag)) { isPaused = true; return; }
        }

        //contender = entityTag;
        SetDominant(entityTag);
    }

    TagsEnum? previousContender;

    void SetDominant(TagsEnum input)
    {
        isPaused = false;
        if (dominant == input) return;
        if (contender == input) return;
        contender = input;
    }

    void TakeControl()
    {
        if (contender != null && !entities.Any(entity => entity.CompareTag(contender.ToString())))
        {
            captureProgress = Mathf.Max(0, captureProgress - Time.deltaTime);
            UIManager.Instance.SetCaptureInfo(contender.ToString(), captureProgress / dominiumTime);
            return;
        }
        if (captureProgress > 0 && previousContender != contender)
        {
            if (isPaused)
            {
                return;
            }
            captureProgress = Mathf.Max(0, captureProgress - Time.deltaTime);
            UIManager.Instance.SetCaptureInfo(previousContender.ToString(), captureProgress / dominiumTime);
            return;
        }
        if (contender != null) previousContender = contender;
        if (0 <= captureProgress && captureProgress < dominiumTime)
        {
            if (isPaused)
            {
                return;
            }
            captureProgress = contender == null ? Mathf.Max(0, captureProgress - Time.deltaTime) : captureProgress + Time.deltaTime;
            string contenderString = contender == null ? previousContender.ToString() : contender.ToString();
            UIManager.Instance.SetCaptureInfo(contenderString, captureProgress / dominiumTime);
            return;
        }
        if (captureProgress > dominiumTime) { dominant = contender; captureProgress = 0; contender = null; }
    }

    private void CaptureZoneProgress()
    {
        if (entities.Count == 0) { return; }
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
        if (!entities.Any(entity => entity.CompareTag(tag.ToString())))
        {
            return;
        }

        progress = Mathf.Min(progress + Time.deltaTime, captureTime);
        UIManager.Instance.SetCaptureScore(progress / captureTime, tag);

        if (progress >= captureTime)
        {
            GameManager.Instance.GameOver();
            string message = tag == TagsEnum.Enemy ? "You Lose!" : "You Win!";
            UIManager.Instance.SetGameOverText(message);
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

    public bool IsThereAnEntityInZone(TagsEnum tag)
    {
        return entities.Any(entity => entity.CompareTag(tag.ToString()));
    }

    public bool IsThereSpecificEntityInZone(Transform entity)
    {
        return entities.Any(e => e == entity);
    }

    public float HowManyEntitiesInZone(TagsEnum tag)
    {
        return entities.Count(entity => entity.CompareTag(tag.ToString()));
    }

    public float GetCaptureRadius()
    {
        return captureRadius;
    }

    public TagsEnum? GetDominant()
    {
        return dominant;
    }
}
