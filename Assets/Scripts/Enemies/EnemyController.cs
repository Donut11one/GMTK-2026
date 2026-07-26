using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private float deathDuration = 0.625f;

    private Transform playerTransform;
    private float _health;
    private bool _dead;

    private void Start()
    {
        _health = enemyData.maxHealth + (GameManager.Instance.Floor - 1);

        // Find the player in the scene by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void TakeDamage(float amount)
    {
        if (_dead)
        {
            return;
        }

        _health -= amount;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _dead = true;

        GameManager.Instance.EnemyKilled();

        GetComponent<Rigidbody2D>().simulated = false;
        animator.PlayDeath();
        enabled = false;

        Destroy(gameObject, deathDuration);
    }

    private void Update()
    {
        // Run the AI behavior every single frame
        if (enemyData != null && enemyData.aiBehavior != null)
        {
            enemyData.aiBehavior.ProcessAI(this, playerTransform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (enemyData != null)
        {
            enemyData.OnHitTarget(collision.gameObject);
        }
    }
}
