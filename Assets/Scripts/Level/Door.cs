using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject barrier;

    public void OpenState(bool open)
    {
        barrier.SetActive(!open);
    }
}
