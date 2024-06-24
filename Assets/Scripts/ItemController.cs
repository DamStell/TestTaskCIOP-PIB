using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform avatarTarget;
    [SerializeField] Transform drawerTarget; 

    private bool isHeld = false;
   // private bool isOverTarget = false;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Coroutine holdCoroutine;
    private GameObject currentItem;
    private float holdDelay = 0.5f; // deleay for start hold item

    private Plane movementPlane; // The plane of movement of an object
    private static bool isAnyItemHeld = false; 

    void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isHeld)
        {
            FollowLaser();
        }
        else
        {
            CheckLaserHover();
        }
    }

    void CheckLaserHover()
    {
        if (isAnyItemHeld) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            if (hit.collider.gameObject == gameObject && !isHeld)
            {
                if (holdCoroutine == null)
                {
                    holdCoroutine = StartCoroutine(StartHolding(hit.point));
                }
            }
        }
        else
        {
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
                holdCoroutine = null;
            }
        }
    }

    IEnumerator StartHolding(Vector3 hitPoint)
    {
        yield return new WaitForSeconds(holdDelay);

        if (isAnyItemHeld) yield break; // If an item is held, it cannot be added

        isHeld = true;
        isAnyItemHeld = true;
        currentItem = gameObject;
        currentItem.transform.SetParent(null);

        Vector3 newPosition = transform.position + Vector3.up * 2.0f; // change height item hold
        movementPlane = new Plane(Camera.main.transform.forward, hitPoint);
        hitPoint = newPosition;

    }

    void FollowLaser()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (movementPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPosition = ray.GetPoint(enter);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10.0f); // Adjust speed as needed
        }

        CheckForTarget();
    }

    void CheckForTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform == avatarTarget && !IsTargetOccupied(avatarTarget))
            {
                PlaceItem(avatarTarget);
            }
            else if (hit.collider.transform == drawerTarget)
            {
                PlaceItem(drawerTarget);
            }
        }
    }

    bool IsTargetOccupied(Transform target)
    {
        // Check if the target is already occupied by another item
        foreach (Transform child in target)
        {
            if (child != transform)
            {
                return true;
            }
        }
        return false;
    }

    // Method place item
    void PlaceItem(Transform target)
    {
        isHeld = false;
        isAnyItemHeld = false;
        holdCoroutine = null;

        if (target == avatarTarget)
        {
            transform.SetParent(avatarTarget);
            transform.localPosition = Vector3.zero;
        }
        else if (target == drawerTarget)
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalPosition;
        }
    }

    void ResetItemPosition()
    {
        isHeld = false;
        isAnyItemHeld = false;
        holdCoroutine = null;
        transform.SetParent(originalParent);
        transform.localPosition = originalPosition;
    }
}
