
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform firePoint;
    [SerializeField][Min(0f)] private float primaryCooldownSeconds;
    [SerializeField][Min(0f)] private float secondaryCooldownSeconds;
    [SerializeField] private float fireCost;
    
    private bool _canFire;
    
    private Coroutine _fireCoroutine;
    
    private void Awake()
    {
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
        inputReader.SecondaryFireEvent += HandleSecondaryFire;
    }

    private void OnDestroy()
    {
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
        inputReader.SecondaryFireEvent -= HandleSecondaryFire;
        if (_fireCoroutine != null)
        {
            StopCoroutine(_fireCoroutine);
        }
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        if (!shouldFire || !CanFireWeapon())
        {
            return;
        }

        PrimaryFireLogic();
        _fireCoroutine = StartCoroutine(
            StartFireTimer(primaryCooldownSeconds));
    }

    private void HandleSecondaryFire(bool shouldFire)
    {
        if (!shouldFire || !CanFireWeapon())
        {
            return;
        }

        SecondaryFireLogic();
        _fireCoroutine = StartCoroutine(
            StartFireTimer(secondaryCooldownSeconds));
    }

    private bool CanFireWeapon()
    {
        return (_fireCoroutine != null) && _canFire;
    }

    private IEnumerator StartFireTimer(float timerLength)
    {
        _canFire = false;
        yield return new WaitForSeconds(timerLength);
        _canFire = true;
        _fireCoroutine = null;
        
    }

    protected abstract void PrimaryFireLogic();

    protected abstract void SecondaryFireLogic();
}