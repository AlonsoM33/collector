using UnityEngine;

public class RoomCameraPerspectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomCameraSet
    {
        public string roomName;

        [Header("Cámaras de este cuarto")]
        public GameObject normalCamera;
        public GameObject alternateCamera;

        [Header("Objetos que se ocultan en vista alternativa")]
        public GameObject[] objectsToHideInAlternateView;
    }

    [Header("Cuartos")]
    public RoomCameraSet[] rooms;

    [Header("Fading Walls")]
    public GameObject[] fadingWalls;

    [Header("Alpha de paredes en vista alternativa")]
    [Range(0f, 1f)]
    public float alternateWallAlpha = 1f;

    private int currentRoomIndex = 0;
    private bool usingAlternateView = false;

    void Start()
    {
        ApplyCameraState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            usingAlternateView = !usingAlternateView;
            ApplyCameraState();
        }
    }

    public void SetCurrentRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Length) return;

        currentRoomIndex = roomIndex;

        // Cuando cambias de cuarto, puedes decidir si quieres volver a vista normal.
        // Si quieres conservar la vista alternativa entre cuartos, comenta esta línea.
        usingAlternateView = false;

        ApplyCameraState();
    }

    void ApplyCameraState()
    {
        // Apagar todas las cámaras primero
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].normalCamera != null)
                rooms[i].normalCamera.SetActive(false);

            if (rooms[i].alternateCamera != null)
                rooms[i].alternateCamera.SetActive(false);
        }

        // Encender solo la cámara correcta del cuarto actual
        RoomCameraSet currentRoom = rooms[currentRoomIndex];

        if (usingAlternateView)
        {
            if (currentRoom.alternateCamera != null)
                currentRoom.alternateCamera.SetActive(true);
        }
        else
        {
            if (currentRoom.normalCamera != null)
                currentRoom.normalCamera.SetActive(true);
        }

        // Controlar techos/objetos por cuarto
        for (int i = 0; i < rooms.Length; i++)
        {
            bool isCurrentRoom = i == currentRoomIndex;

            foreach (GameObject obj in rooms[i].objectsToHideInAlternateView)
            {
                if (obj == null) continue;

                // Solo ocultamos objetos del cuarto actual cuando está en vista alternativa.
                // Los demás cuartos se quedan visibles normal.
                if (isCurrentRoom && usingAlternateView)
                    obj.SetActive(false);
                else
                    obj.SetActive(true);
            }
        }

        // Controlar fading walls
        ApplyFadingWallsState();
    }

    void ApplyFadingWallsState()
    {
        foreach (GameObject wall in fadingWalls)
        {
            if (wall == null) continue;

            wall.SetActive(true);

            Renderer[] renderers = wall.GetComponentsInChildren<Renderer>(true);
            MonoBehaviour[] scripts = wall.GetComponentsInChildren<MonoBehaviour>(true);

            if (usingAlternateView)
            {
                // En vista alternativa: apagamos scripts de fading
                foreach (MonoBehaviour script in scripts)
                {
                    if (script == null) continue;

                    string scriptName = script.GetType().Name;

                    if (scriptName.Contains("Fade") || scriptName.Contains("TransparentWall"))
                    {
                        script.enabled = false;
                    }
                }

                // En vista alternativa: forzamos que se vean las paredes
                foreach (Renderer rend in renderers)
                {
                    if (rend == null) continue;

                    rend.enabled = true;
                    SetRendererAlpha(rend, alternateWallAlpha);
                }
            }
            else
            {
                // En vista normal: reactivamos scripts de fading
                foreach (MonoBehaviour script in scripts)
                {
                    if (script == null) continue;

                    string scriptName = script.GetType().Name;

                    if (scriptName.Contains("Fade") || scriptName.Contains("TransparentWall"))
                    {
                        script.enabled = true;
                    }
                }

                // En vista normal: apagamos render.
                // El script de fading lo prenderá cuando el jugador se acerque.
                foreach (Renderer rend in renderers)
                {
                    if (rend == null) continue;

                    rend.enabled = false;
                }
            }
        }
    }

    void SetRendererAlpha(Renderer rend, float alpha)
    {
        Material mat = rend.material;
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }
}