using UnityEngine;
using System.Collections;

public class WardobeController : MonoBehaviour
{
    [SerializeField] GameObject[] Drawers;
    [SerializeField] GameObject[] frontDrawers;
    [SerializeField] GameObject[] wardrobe1Items;
    [SerializeField] GameObject[] wardrobe2Items;
    [SerializeField] GameObject[] wardrobe3Items;

    [SerializeField] float drawerMoveDistance = 4.0f;
    [SerializeField] float itemMoveDistance = 2.0f;
    [SerializeField] float drawerMoveSpeed = 2.0f;
    [SerializeField] float itemMoveSpeed = 1.0f;
    [SerializeField] float holdDelay = 0.3f; // delay for open or close Drawer

    private GameObject currentDrawer;
    private GameObject[] currentItems;
    private bool isMoving = false;
    private bool isOpening = false;
    private GameObject targetDrawer;
    private GameObject[] targetItems;
    private Coroutine holdCoroutine;

    void Start()
    {
        // setting drawers and items as closed
        SetDrawerPosition(Drawers[0], -drawerMoveDistance);
        SetDrawerPosition(Drawers[1], -drawerMoveDistance);
        SetDrawerPosition(Drawers[2], -drawerMoveDistance);
        SetItemsPosition(wardrobe1Items, -itemMoveDistance);
        SetItemsPosition(wardrobe2Items, -itemMoveDistance);
        SetItemsPosition(wardrobe3Items, -itemMoveDistance);
    }

    void FixedUpdate()
    {
        CheckMouseOver();
    }

    // method check whether laser targets the drawer
    void CheckMouseOver()
    {
        if (isMoving) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == frontDrawers[0])
            {
                if (holdCoroutine == null)
                {
                    holdCoroutine = StartCoroutine(HoldToToggleDrawer(0));
                }
            }
            else if (hit.collider.gameObject == frontDrawers[1])
            {
                if (holdCoroutine == null)
                {
                    holdCoroutine = StartCoroutine(HoldToToggleDrawer(1));
                }
            }
            else if (hit.collider.gameObject == frontDrawers[2])
            {
                if (holdCoroutine == null)
                {
                    holdCoroutine = StartCoroutine(HoldToToggleDrawer(2));
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
    }
    //  handing delay for opening and closing drawers
    IEnumerator HoldToToggleDrawer(int drawerIndex)
    {
        yield return new WaitForSeconds(holdDelay);

        if (currentDrawer == Drawers[drawerIndex])
        {
            StartCoroutine(CloseCurrentDrawer());
        }
        else
        {
            targetDrawer = Drawers[drawerIndex];
            targetItems = GetWardrobeItems(drawerIndex);
            if (!isOpening)
            {
                StartCoroutine(ToggleDrawers());
            }
        }
        holdCoroutine = null;
    }

    GameObject[] GetWardrobeItems(int drawerIndex)
    {
        switch (drawerIndex)
        {
            case 0: return wardrobe1Items;
            case 1: return wardrobe2Items;
            case 2: return wardrobe3Items;
            default: return null;
        }
    }
    // handing  opening and closing items and drawers
    IEnumerator ToggleDrawers()
    {
        isMoving = true;
        isOpening = true;

        if (currentDrawer != null)
        {
            yield return StartCoroutine(MoveObjectsSimultaneously(currentDrawer, currentItems, -drawerMoveDistance, -itemMoveDistance, drawerMoveSpeed, itemMoveSpeed));
        }
        if (targetDrawer != null)
        {
            currentDrawer = targetDrawer;
            currentItems = targetItems;
            targetDrawer = null;
            targetItems = null;

            yield return StartCoroutine(MoveObjectsSimultaneously(currentDrawer, currentItems, drawerMoveDistance, itemMoveDistance, drawerMoveSpeed, itemMoveSpeed));
        }

        isMoving = false;
        isOpening = false;
    }

    IEnumerator CloseCurrentDrawer()
    {
        isMoving = true;

        if (currentDrawer != null)
        {
            yield return StartCoroutine(MoveObjectsSimultaneously(currentDrawer, currentItems, -drawerMoveDistance, -itemMoveDistance, drawerMoveSpeed, itemMoveSpeed));
            currentDrawer = null;
            currentItems = null;
        }

        isMoving = false;
    }

    //simulation of smooth movement for objects
    IEnumerator MoveObjectsSimultaneously(GameObject drawer, GameObject[] items, float drawerDistance, float itemDistance, float drawerSpeed, float itemSpeed)
    {   
        float elapsedTime = 0;
        Vector3 drawerInitialPosition = drawer.transform.localPosition;
        Vector3 drawerTargetPosition = drawerInitialPosition + new Vector3(drawerDistance, 0, 0);
        Vector3[] itemsInitialPositions = new Vector3[items.Length];
        Vector3[] itemsTargetPositions = new Vector3[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            itemsInitialPositions[i] = items[i].transform.localPosition;
            itemsTargetPositions[i] = itemsInitialPositions[i] + new Vector3(itemDistance, 0, 0);
        }

        while (elapsedTime < Mathf.Max(drawerSpeed, itemSpeed))
        {
            drawer.transform.localPosition = Vector3.Lerp(drawerInitialPosition, drawerTargetPosition, elapsedTime / drawerSpeed);

            for (int i = 0; i < items.Length; i++)
            {
                items[i].transform.localPosition = Vector3.Lerp(itemsInitialPositions[i], itemsTargetPositions[i], elapsedTime / itemSpeed);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        drawer.transform.localPosition = drawerTargetPosition;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.localPosition = itemsTargetPositions[i];
        }
    }

    void SetDrawerPosition(GameObject drawer, float distance)
    {
        drawer.transform.localPosition += new Vector3(distance, 0, 0);
    }

    void SetItemsPosition(GameObject[] items, float distance)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.localPosition += new Vector3(distance, 0, 0);
        }
    }
}
