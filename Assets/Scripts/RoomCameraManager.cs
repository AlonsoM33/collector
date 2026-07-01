using UnityEngine;

public class RoomCameraManager : MonoBehaviour
{
    [Header("Cámaras de cuartos")]
    public GameObject[] roomCameras;

    public void SwitchToCamera(GameObject cameraToActivate)
    {
        foreach (GameObject cam in roomCameras)
        {
            if (cam != null)
                cam.SetActive(false);
        }

        if (cameraToActivate != null)
            cameraToActivate.SetActive(true);
    }
}