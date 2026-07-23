using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName = "Basic Enemy";
    public float damageAmount = 5f;

    [Header("AI Behavior")]
    public EnemyBehaviorSO aiBehavior;

    public void OnHitTarget(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log(enemyName + " hit " + target.name + "!");
        }
    }
}