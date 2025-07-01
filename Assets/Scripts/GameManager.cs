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

    public void Respawn(Transform item, float delay)
    {
        StartCoroutine(RespawnRoutine(item, delay));
    }

    private IEnumerator RespawnRoutine(Transform item, float delay)
    {
        yield return new WaitForSeconds(delay);

        item.gameObject.SetActive(true);
    }
}
