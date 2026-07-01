using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class TransparentWallNearPlayer : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Distancia desde la pared")]
    public float appearDistance = 0.8f;

    [Header("Alpha")]
    [Range(0f, 1f)]
    public float hiddenAlpha = 0f;

    [Range(0f, 1f)]
    public float visibleAlpha = 0.15f;

    [Header("Suavizado")]
    public float fadeSpeed = 10f;

    private Renderer wallRenderer;
    private Collider wallCollider;
    private Material wallMaterial;
    private float currentAlpha;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallCollider = GetComponent<Collider>();

        wallMaterial = wallRenderer.material;

        currentAlpha = hiddenAlpha;
        SetAlpha(currentAlpha);

        // Inicia completamente invisible
        wallRenderer.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 closestPoint = wallCollider.ClosestPoint(player.position);
        float distanceToWall = Vector3.Distance(player.position, closestPoint);

        bool shouldShow = distanceToWall <= appearDistance;

        if (shouldShow && !wallRenderer.enabled)
        {
            wallRenderer.enabled = true;
        }

        float targetAlpha = shouldShow ? visibleAlpha : hiddenAlpha;

        currentAlpha = Mathf.Lerp(
            currentAlpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );

        SetAlpha(currentAlpha);

        // Cuando ya casi no se ve, apaga el render por completo
        if (!shouldShow && currentAlpha <= 0.01f)
        {
            wallRenderer.enabled = false;
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = wallMaterial.color;
        color.a = alpha;
        wallMaterial.color = color;
    }
}