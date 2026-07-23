using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    // BL: gave the player a rigidbody instead of transform so it collides properly
    [SerializeField] private Rigidbody2D body;

    [Header("Settings")] 
    [SerializeField] private float moveSpeed = 5f;

    // BL: MoveEvent fires when input changes, so cache it and apply continuously 
    // applying in the handler made the player move one frame and then stop
    private Vector2 _moveInput;

    private void Awake()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        inputReader.MoveEvent += OnMove;
    }

    private void OnDestroy()
    {
        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 movementInput)
    {
        Debug.Log($"Move: {movementInput}");
        _moveInput = movementInput.normalized;
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + _moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
