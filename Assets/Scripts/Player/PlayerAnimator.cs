using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Moving/Idling")]
    [SerializeField] private Sprite[] moveDown;
    [SerializeField] private Sprite[] moveUp;
    [SerializeField] private Sprite[] moveSide;

    [Header("Attacking")]
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private Sprite[] attackDown;
    [SerializeField] private Sprite[] attackUp;
    [SerializeField] private Sprite[] attackSide;

    [Header("Taking Damage")]
    [SerializeField] private float damageDuration = 0.3f;
    [SerializeField] private Sprite[] damageDown;
    [SerializeField] private Sprite[] damageUp;
    [SerializeField] private Sprite[] damageSide;

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private SpriteAnimator animator;
    [SerializeField] private Camera camera;

    private float _attackTimer;
    private float _damageTimer;

    private void Awake()
    {
        inputReader.PrimaryFireEvent += OnFire;
    }

    private void Start()
    {
        GameManager.Instance.DamageTaken += OnDamaged;
    }

    private void OnDestroy()
    {
        inputReader.PrimaryFireEvent -= OnFire;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.DamageTaken -= OnDamaged;
        }
    }

    private void OnFire(bool pressed)
    {
        if (pressed)
        {
            _attackTimer = attackDuration;
        }
    }

    private void OnDamaged()
    {
        _damageTimer = damageDuration;
    }

    private void Update()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }

        if (_damageTimer > 0f)
        {
            _damageTimer -= Time.deltaTime;
        }

        Sprite[] clip = SelectClip(AimDirection(), out bool flipX);

        animator.Play(clip);
        animator.SetFlip(flipX);
    }

    private Vector2 AimDirection()
    {
        Vector3 mouse = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return (Vector2)mouse - (Vector2)transform.position;
    }

    private Sprite[] SelectClip(Vector2 aim, out bool flipX)
    {
        flipX = false;

        if (Mathf.Abs(aim.x) >= Mathf.Abs(aim.y))
        {
            // BL: aim right by default, flip when aiming left
            flipX = aim.x < 0f;
            return Pick(damageSide, attackSide, moveSide);
        }

        if (aim.y > 0f)
        {
            return Pick(damageUp, attackUp, moveUp);
        }

        return Pick(damageDown, attackDown, moveDown);
    }

    private Sprite[] Pick(Sprite[] damaged, Sprite[] attacking, Sprite[] moving)
    {
        if (_damageTimer > 0f)
        {
            return damaged;
        }

        if (_attackTimer > 0f)
        {
            return attacking;
        }

        return moving;
    }
}