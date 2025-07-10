using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Transform target;
    [SerializeField] Vector3 offset = new(0, 1, 0);
    [SerializeField] Image lifeBarImage; // Assuming you have a UI Image component for the life bar

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void UpdateLifeBar(float lifePercentage)
    {
        lifeBarImage.fillAmount = lifePercentage;
    }
}
