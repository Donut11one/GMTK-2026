using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.25f;

    private Color _defaultColor;
    private float _flashUntil;

    private void Start()
    {
        _defaultColor = label.color;
        GameManager.Instance.TimeChanged += UpdateLabel;
        GameManager.Instance.DamageTaken += Flash;
        UpdateLabel(GameManager.Instance.Remaining);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TimeChanged -= UpdateLabel;
            GameManager.Instance.DamageTaken -= Flash;
        }
    }

    private void UpdateLabel(float remaining)
    {
        int total = Mathf.CeilToInt(remaining);
        int minutes = total / 60;
        int seconds = total % 60;
        label.text = $"{minutes}:{seconds:00}";
        label.color = Time.time < _flashUntil 
            ? damageColor
            : _defaultColor;
    }

    private void Flash()
    {
        _flashUntil = Time.time + flashDuration;
    }
}
