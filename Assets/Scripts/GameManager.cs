using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool playing;

    private void Start()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        playing = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        playing = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        PauseGame();
        UIManager.Instance.ToggleGameOverMenu(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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

    public Vector3 GetNearestHealPoint(Vector3 position)
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;
        foreach (Transform healPoint in healPoints)
        {
            if (!healPoint.gameObject.activeSelf) continue;
            float distance = Vector3.Distance(position, healPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = healPoint;
            }
        }
        return nearest == null ? position : nearest.position;
    }

    public Transform GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }
}
