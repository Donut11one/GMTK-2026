using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")] 
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        inputReader.MoveEvent += OnMove;
    }

    private void OnDestroy()
    {
        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 movementInput)
    {
        float moveY = movementInput.y;
        float moveX = movementInput.x;
        Vector2 moveDirection =  new Vector2(moveX, moveY).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
