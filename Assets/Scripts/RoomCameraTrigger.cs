using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
    [Header("Manager de cámaras")]
    public RoomCameraPerspectiveManager cameraManager;

    [Header("Número de cuarto")]
    public int roomIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        cameraManager.SetCurrentRoom(roomIndex);
    }
}