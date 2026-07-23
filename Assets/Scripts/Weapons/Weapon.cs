using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Attack primaryAttack;
    [SerializeField] private Attack secondaryAttack;

    private void Start()
    {
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
        inputReader.SecondaryFireEvent += HandleSecondaryFire;
    }

    private void OnDestroy()
    {
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
        inputReader.SecondaryFireEvent -= HandleSecondaryFire;
    }

    protected virtual void HandlePrimaryFire(bool shouldFire)
    {
        if (shouldFire)
        {
            primaryAttack?.Fire(firePoint);
        }
    }

    protected virtual void HandleSecondaryFire(bool shouldFire)
    {
        if (shouldFire)
        {
            secondaryAttack?.Fire(firePoint);
        }
    }
}