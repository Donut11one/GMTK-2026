using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RoomSizeCamera : MonoBehaviour
{
    private void Start()
    {
        var cam = GetComponent<Camera>();
        float aspectRatio = (float)Screen.width / Screen.height;

        var rect = new Rect(0f, 0f, 1f, 1f);

        if (aspectRatio > 1f)
        {
            rect.width = 1f / aspectRatio;
            rect.x = (1f - rect.width) / 2f;
        }
        else
        {
            rect.height = aspectRatio;
            rect.y = (1f - rect.height) / 2f;
        }

        cam.rect = rect;
    }
}
