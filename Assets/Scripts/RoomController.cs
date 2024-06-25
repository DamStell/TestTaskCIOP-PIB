using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    private Dictionary<GameObject, bool> surfacesHit = new Dictionary<GameObject, bool>();
    private bool isPlayerInRoom = false;
    private HashSet<GameObject> wallsWithMultipleHits = new HashSet<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Wall") || child.CompareTag("Floor") || child.CompareTag("Ceiling") || child.CompareTag("Walldoor"))
            {
                surfacesHit.Add(child.gameObject, false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = true;
            //Debug.Log("Player entered the room");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            Debug.Log("Player  left the room");
            // handing counter of missed walls in the room
            PlayerController.instance.errorsSkippedWalls = PlayerController.instance.errorsSkippedWalls + (6-PlayerController.instance.correctShotsInRoom);
            PlayerController.instance.exitRoomCounter++;


        }
    }

    public void CheckHit(GameObject hitObject, float ringSetting)
    {
        if (!isPlayerInRoom)
        {
            Debug.Log("Player nie jest w pokoju");
            return;
        }

        if (hitObject.CompareTag("Walldoor"))
        {
            HandleWalldoorHit(ringSetting);
        }
        else if (surfacesHit.ContainsKey(hitObject))
        {
            if (ringSetting == 0)
            {
                if (!surfacesHit[hitObject]) //correct neutralization wall
                {
                    StartCoroutine(ChangeColor(hitObject, Color.green));
                    surfacesHit[hitObject] = true;
                    PlayerController.instance.correctShots++;
                    PlayerController.instance.correctShotsInRoom++;
                }
                else
                {   //walls with excess shot
                    StartCoroutine(ChangeColor(hitObject, Color.red)); 
                    if (!wallsWithMultipleHits.Contains(hitObject))
                    {
                        wallsWithMultipleHits.Add(hitObject);
                        PlayerController.instance.errorsMultipleHits++;
                    }
                }
            }
            else
            {   //incorrectly adjusted ring
                StartCoroutine(ChangeColor(hitObject, Color.red));
                PlayerController.instance.errorsWrongRingSettingRoom++;
            }
        }
        else
        {
            Debug.Log("Hit object not found in surfacesHit dictionary: " + hitObject.name);
        }
    }

    private void HandleWalldoorHit(float ringSetting)
    {
        bool anyUnhitWalldoor = false;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Walldoor") && !surfacesHit[child.gameObject])
            {
                anyUnhitWalldoor = true;
                break;
            }
        }

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Walldoor"))
            {
                if (ringSetting == 0 && anyUnhitWalldoor)
                {
                    StartCoroutine(ChangeColor(child.gameObject, Color.green));
                    surfacesHit[child.gameObject] = true;
                }
                else
                {
                    StartCoroutine(ChangeColor(child.gameObject, Color.red));
                }
            }
        }
    }

    private IEnumerator ChangeColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        renderer.material.color = color;
        yield return new WaitForSeconds(0.13f);
        renderer.material.color = originalColor;
    }

    public bool AllSurfacesHit()
    {
        foreach (bool hit in surfacesHit.Values)
        {
            if (!hit)
            {
                
                return false;
            }
        }
        return true;
    }
}
