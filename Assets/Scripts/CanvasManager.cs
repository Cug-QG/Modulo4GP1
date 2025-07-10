using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    public static CanvasManager Instance
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

    [SerializeField] Canvas mainCanvas; // Reference to the main canvas

    public LifeBar SpawnLifeBar(Transform target, LifeBar lifeBarPrefab)
    {
        GameObject lifeBarObject = Instantiate(lifeBarPrefab.gameObject, target.position, Quaternion.Euler(90,0,0), mainCanvas.transform);
        LifeBar lifeBar = lifeBarObject.GetComponent<LifeBar>();
        lifeBar.SetTarget(target);
        return lifeBar;
    }
}
