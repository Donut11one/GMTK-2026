using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyDataSO enemyData;
    private Transform playerTransform;
    private float _health;

    private void Start()
    {
        _health = enemyData.maxHealth;

        // Find the player in the scene by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
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