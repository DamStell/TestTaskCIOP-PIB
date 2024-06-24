using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] LayerMask interactableLayer;

    private Camera mainCamera;
    private GameObject highlightedObject;
    private Color originalColor;
    private bool originalColorSet = false;

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    void Update()
    {
        RotateLaser();
        DrawLaser();
    }

    void RotateLaser()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Vector3 targetPoint = hit.point;
            Vector3 direction = targetPoint - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(90 + rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        }
    }

    void DrawLaser()
    {
        lineRenderer.SetPosition(0, transform.position);

        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            lineRenderer.SetPosition(1, hit.point);
            // Highlight the object the laser is pointing at
            HighlightObject(hit.collider.gameObject);
        }
        else
        {
            lineRenderer.SetPosition(1, ray.GetPoint(maxDistance));
            RemoveHighlight();
        }
    }

    void HighlightObject(GameObject obj)
    {
        if (highlightedObject != null && highlightedObject != obj)
        {
            RemoveHighlight();
        }

        highlightedObject = obj;
        // Apply highlighting effect (e.g., change color)
        var renderer = highlightedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (!originalColorSet)
            {
                originalColor = renderer.material.color;
                originalColorSet = true;
            }
            renderer.material.color = Color.green;
        }
    }

    void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            var renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
            highlightedObject = null;
            originalColorSet = false;
        }
    }
}
