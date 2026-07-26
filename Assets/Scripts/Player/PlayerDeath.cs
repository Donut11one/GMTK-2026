using UnityEngine;

public class PlayerDeath : MonoBehaviour, IDamageable
{
    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private MonoBehaviour[] disableOnDeath;
    [SerializeField] private Sprite[] deathFrames;
    [SerializeField] private float iFrameTime = 0.75f;

    private float _nextHitTime;

    private void Start()
    {
        GameManager.Instance.GameOver += Die;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver -= Die;
        }
    }

    private void Die()
    {
        playerAnimator.enabled = false;

        foreach (var behavior in disableOnDeath)
        {
            behavior.enabled = false;
        }

        spriteAnimator.Play(deathFrames);
    }

    public void TakeDamage(float seconds)
    {
        if (Time.time < _nextHitTime)
        {
            return;
        }

        _nextHitTime = Time.time + iFrameTime;
        GameManager.Instance.TimerDamage(seconds);
    }
}
