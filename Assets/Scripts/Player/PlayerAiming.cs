using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private Transform aimPosition;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Camera playerCamera;

    private void LateUpdate()
    {
        Vector2 worldAimPosition =
            playerCamera.ViewportToWorldPoint(inputReader.AimPosition);
        
        aimPosition.up = new Vector2(
            worldAimPosition.x - aimPosition.position.x, 
            worldAimPosition.y - aimPosition.position.y
        );
    }
}
