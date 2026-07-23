using UnityEngine;

public abstract class EnemyBehaviorSO : ScriptableObject
{
    // Every enemy AI script will implement this update method
    public abstract void ProcessAI(EnemyController enemy, Transform playerTransform);
}