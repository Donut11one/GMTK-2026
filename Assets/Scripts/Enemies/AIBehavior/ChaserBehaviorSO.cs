using UnityEngine;

[CreateAssetMenu(fileName = "AI_Chaser", menuName = "Enemy/Type/Chaser")]
public class ChaserBehaviorSO : EnemyBehaviorSO
{
    [SerializeField] private float moveSpeed = 3.5f;

    public override void ProcessAI(EnemyController enemy, Transform playerTransform)
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector2 direction = (playerTransform.position - enemy.transform.position).normalized;
        enemy.transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}