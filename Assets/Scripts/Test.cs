using UnityEngine;

public class Test : MonoBehaviour
{
    Transform player;
    [SerializeField] Vector3 offset = new Vector3(0, 1, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
