using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    [Header("Configuración de puerta")]
    public float openAngle = 90f;
    public float openSpeed = 4f;

    [Header("Tecla de prueba")]
    public KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, 0f, openAngle);
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }
}