
using System;
using System.Timers;
using UnityEngine;

[Serializable]
public abstract class Attack: MonoBehaviour
{
    [SerializeField] private float cooldownSeconds;
    [SerializeField] public float damage;
    [SerializeField] private float cost;

    protected Timer CooldownTimer;
    
    public bool CanFire => !CooldownTimer.Enabled;

    private void Awake()
    {
        const int milliseconds = 1000;
        CooldownTimer = new Timer(cooldownSeconds * milliseconds);
        CooldownTimer.AutoReset = false;
        CooldownTimer.Enabled = false;
        CooldownTimer.Elapsed += ResetTimer;
    }

    private void ResetTimer(object sender, ElapsedEventArgs e)
    {
        CooldownTimer.Stop();
    }

    public abstract void Fire(Transform firePoint);
}
