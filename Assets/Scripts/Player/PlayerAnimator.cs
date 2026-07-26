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

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private SpriteAnimator animator;
    [SerializeField] private Camera camera;

    private float _attackTimer;

    private void Awake()
    {
        inputReader.PrimaryFireEvent += OnFire;
    }

    private void OnDestroy()
    {
        inputReader.PrimaryFireEvent -= OnFire;
    }

    private void OnFire(bool pressed)
    {
        if (pressed)
        {
            _attackTimer = attackDuration;
        }
    }

    private void Update()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }

        Vector2 aimDirection = AimDirection();
        bool attacking = _attackTimer > 0f;

        Sprite[] clip = SelectClip(aimDirection, attacking, out bool flipX);

        animator.Play(clip);
        animator.SetFlip(flipX);
    }

    private Vector2 AimDirection()
    {
        Vector3 mouse = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return (Vector2)mouse - (Vector2)transform.position;
    }

    private Sprite[] SelectClip(Vector2 aim, bool attacking, out bool flipX)
    {
        flipX = false;

        if (Mathf.Abs(aim.x) >= Mathf.Abs(aim.y))
        {
            // BL: aim right by default, flip when aiming left
            flipX = aim.x < 0f;
            return attacking
                ? attackSide
                : moveSide;
        }

        if (aim.y > 0f)
        {
            return attacking
                ? attackUp
                : moveUp;
        }

        return attacking
            ? attackDown
            : moveDown;
    }
}
