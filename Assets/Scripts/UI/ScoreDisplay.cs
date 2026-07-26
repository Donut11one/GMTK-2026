using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        GameManager.Instance.ScoreChanged += UpdateLabel;
        UpdateLabel(GameManager.Instance.Score);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ScoreChanged -= UpdateLabel;
        }
    }

    private void UpdateLabel(int score)
    {
        label.text = score.ToString();
    }
}
