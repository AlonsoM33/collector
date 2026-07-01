using UnityEngine;

public class FollowCamera    : MonoBehaviour
{
    [Header("Jugador")]
    public Transform target;

    [Header("Offset base de la cámara")]
    public Vector3 offset = new Vector3(0f, 2.5f, -6f);

    [Header("Suavizado")]
    public float followSmoothness = 5f;

    [Header("Alejar si el jugador se acerca a la cámara")]
    public float minDistanceToCamera = 3f;
    public float pushBackAmount = 2f;

    [Header("Mirar al jugador")]
    public float lookHeight = 1.2f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;

        // Posición base de la cámara
        Vector3 desiredPosition = targetPosition + offset;

        // Distancia real entre cámara y jugador
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Si el jugador está muy cerca de la cámara, la cámara se aleja
        if (distance < minDistanceToCamera)
        {
            Vector3 awayFromPlayer = (transform.position - targetPosition).normalized;
            desiredPosition += awayFromPlayer * pushBackAmount;
        }

        // Movimiento suave
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSmoothness * Time.deltaTime
        );

        // La cámara mira un poco arriba del jugador, no a sus pies
        Vector3 lookTarget = targetPosition + Vector3.up * lookHeight;
        transform.LookAt(lookTarget);
    }
}