using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private SpriteAnimator animator;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] deathFrames;

    private Transform _player;
    private bool _dead;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            _player = player.transform;
        }

        animator.Play(idleFrames);
    }

    public void PlayDeath()
    {
        _dead = true;
        animator.Play(deathFrames);
    }

    private void Update()
    {
        if (_dead || _player == null)
        {
            return;
        }

        animator.SetFlip(_player.position.x < transform.position.x);
    }
}