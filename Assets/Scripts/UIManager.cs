using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
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

    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI enemyScoreText;
    [SerializeField] TextMeshProUGUI captureInfo;

    public void SetCaptureScore(float score, TagsEnum tag)
    {
        switch (tag)
        {
            case TagsEnum.Player:
                playerScoreText.text = (int)(score * 100) + "%";
                break;
            case TagsEnum.Enemy:
                enemyScoreText.text = (int)(score * 100) + "%";
                break;
        }
    }

    public void SetCaptureInfo(string name, float info)
    {
        captureInfo.text = name + " " + (int)(info * 100) + "%";
    }
}