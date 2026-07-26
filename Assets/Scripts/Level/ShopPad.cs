using UnityEngine;

public class ShopPad : MonoBehaviour
{
    [SerializeField] private float cost = 10f;
    [SerializeField] private int damageBonus = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || GameManager.Instance.Remaining <= cost)
        {
            return;
        }

        GameManager.Instance.SpendTime(cost);
        GameManager.Instance.AddDamage(damageBonus);
        gameObject.SetActive(false);
    }
}
