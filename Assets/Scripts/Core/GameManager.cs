using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float startSeconds = 120f;
    [SerializeField] private float killReward = 1f;

    public event Action<float> TimeChanged;
    public event Action<int> ScoreChanged;
    public event Action GameOver;
    public event Action DamageTaken;
    public float Remaining { get; private set; }
    public int Score { get; private set; }
    public int Floor { get; private set; } = 1;
    public int DamageBonus { get; private set; }

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

    public void SetPaused(bool paused)
    {
        _running = !paused;
    }

    public void AddDamage(int amount)
    {
        DamageBonus += amount;
    }

    public void SpendTime(float seconds)
    {
        if (Remaining <= 0f)
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

    public void TimerDamage(float seconds)
    {
        DamageTaken?.Invoke();
        SpendTime(seconds);
    }

    public void EnemyKilled()
    {
        Score += Floor;
        ScoreChanged?.Invoke(Score);
        AddTime(killReward);
    }

    public void NextFloor()
    {
        Floor++;
    }
}
