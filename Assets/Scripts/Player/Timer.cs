using System;
using System.Collections;
using UnityEngine;


public class Timer : MonoBehaviour
{
    [SerializeField] private float startTime = 120f;
    [SerializeField] private float currentTime;
    
    public event Action TimerExpired;
    public event Action<float> TimerTick; 
    private Coroutine _timerCoroutine;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        TimerExpired += OnTimerExpired;
        currentTime = startTime;
    }

    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(TickTimer());
    }

    public void PauseTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
    }

    public void AddTime(float toAdd) =>  currentTime += toAdd;

    public float GetTime()
    {
        return currentTime;
    }

    private IEnumerator TickTimer()
    {
        while (currentTime <= 0f)
        {
            currentTime -= Time.deltaTime;
            TimerTick?.Invoke(currentTime);
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
