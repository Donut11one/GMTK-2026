using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        GameManager.Instance.TimeChanged += UpdateLabel;
        UpdateLabel(GameManager.Instance.Remaining);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TimeChanged -= UpdateLabel;
        }
    }

    private void UpdateLabel(float remaining)
    {
        int total = Mathf.CeilToInt(remaining);
        int minutes = total / 60;
        int seconds = total % 60;
        label.text = $"{minutes}:{seconds:00}";
    }
}
