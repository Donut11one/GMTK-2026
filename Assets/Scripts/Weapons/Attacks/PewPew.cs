using UnityEngine;
using UnityEngine.InputSystem;

public class PewPew: Attack
{
    [SerializeField] private Projectile projectile;

    public override void Fire(Transform firePoint)
    {
        if (!CanFire)
        {
            return;
        }

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = ((Vector2)mouse - (Vector2)firePoint.position).normalized;

        var bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
        bullet.Launch(direction, damage);
        Debug.Log("bullet fired");

        CooldownTimer.Start();
    }
}