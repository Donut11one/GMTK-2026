using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private MonoBehaviour[] disableOnDeath;
    [SerializeField] private Sprite[] deathFrames;

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
}
