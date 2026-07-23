using System;
using System.Collections;
using UnityEngine;


public class Timer : MonoBehaviour
{
    [SerializeField] private float startTime = 120f;
    [SerializeField] private float remainingTime;
    
    public event Action TimerExpired;
    public event Action<float> TimerTick; 
    private Coroutine _timerCoroutine;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        TimerExpired += OnTimerExpired;
        remainingTime = startTime;
    }

    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(TickTimer());
    }

    public IEnumerator PauseTimer(float pauseTime)
    {
        float remainingPauseTime = remainingTime;
        StopTimer();
        while (remainingPauseTime > 0f)
        {
            remainingPauseTime -= Time.deltaTime;
            yield return null;
        }
        StartTimer();
    }
    
    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
    }

    public void AddTime(float toAdd) =>  remainingTime += toAdd;

    public float GetTime()
    {
        return remainingTime;
    }

    private IEnumerator TickTimer()
    {
        while (remainingTime <= 0f)
        {
            remainingTime -= Time.deltaTime;
            TimerTick?.Invoke(remainingTime);
            yield return null;
        }
        
        TimerExpired?.Invoke();
    }

    private void OnTimerExpired()
    {
        if (_timerCoroutine == null)
        {
            return;
        }

        StopCoroutine(_timerCoroutine);
        _timerCoroutine = null;
    }

}
