
using UnityEngine;

public class PewPew: Attack
{
    [SerializeField] private Sprite bullet;
    public override void Fire(Transform firePoint)
    {
        if (!CanFire)
        {
            return;
        }

        GameObject projectile = new ()
        {
            transform =
            {
                position = firePoint.position,
                rotation = firePoint.rotation
            }
        };
        SpriteRenderer projSprite = projectile.AddComponent<SpriteRenderer>();
        projSprite.sprite = bullet;
        CooldownTimer.Start();
    }
}