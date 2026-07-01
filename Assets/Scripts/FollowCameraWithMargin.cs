using UnityEngine;

public class FollowCameraClamped : MonoBehaviour
{
    [Header("Jugador")]
    public Transform target;

    [Header("Offset base de la cámara")]
    public Vector3 offset = new Vector3(4.33f, 1.25f, 0f);

    [Header("Suavizado")]
    public float followSmoothness = 10f;

    [Header("Límites de cámara")]
    public bool useLimits = true;

    public float minX = -10f;
    public float maxX = 10f;

    public float minY = 1f;
    public float maxY = 5f;

    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Mirar al jugador")]
    public float lookHeight = 1.2f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSmoothness * Time.deltaTime
        );

        Vector3 lookTarget = target.position + Vector3.up * lookHeight;
        transform.LookAt(lookTarget);
    }
}