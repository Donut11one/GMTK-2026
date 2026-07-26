using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float startSeconds = 120f;

    public event Action<float> TimeChanged;
    public event Action GameOver;
    public float Remaining { get; private set; }

    private bool _running;

    private void Awake()
    {
        if (Instance != null && 
            Instance != this
        ){
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        Remaining = startSeconds;
        _running = true;
    }

    private void Update()
    {
        if (_running)
        {
            SpendTime(Time.deltaTime);
        }
    }

    public void SpendTime(float seconds)
    {
        if (!_running)
        {
            return;
        }
        
        Remaining -= seconds;
        TimeChanged?.Invoke(Remaining);

        if (Remaining <= 0f)
        {
            Remaining = 0f;
            _running = false;
            GameOver?.Invoke();
        }
    }

    public void AddTime(float seconds)
    {
        Remaining += seconds;
        TimeChanged?.Invoke(Remaining);
    }
}
