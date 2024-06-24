using UnityEngine;
using TMPro;
using System.Collections;

public class DoorController : MonoBehaviour
{
    private int requiredSetting;
    private bool isNeutralized = false;
    private bool isOpen = false;
    private bool isCheckedAfterNeutralization = false;

    [SerializeField] Animator doorAnimator;
    [SerializeField] ParticleSystem fireEffect;

    void Start()
    {
        requiredSetting = Random.Range(1, 10);
    }

    public void Neutralize(float ringSetting)
    {
        if (ringSetting == requiredSetting)
        {
            isNeutralized = true;
            requiredSetting = 0;
            //Debug.Log("Door neutralization correct.");
            doorAnimator.SetTrigger("NeurializeDoor");

        }
    }

    public void Interact()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            if (isNeutralized)
            {
                if (isCheckedAfterNeutralization)
                {
                    OpenDoor();
                    PlayerController.instance.OpenDoorCounter++;
                    


                }
                else
                {
                    //Debug.Log("The door was not re-checked after neutralization!");
                    StartFire();
                    PlayerController.instance.errorsNoMeasurement++;
                    PlayerController.instance.LoseLife();
                   
                }
            }
            else
            {
                // Debug.Log("The door is not neutralized!");
                StartFire();
                PlayerController.instance.errorsNoMeasurement++;
                PlayerController.instance.LoseLife();
               
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        doorAnimator.SetBool("isOpen", true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        doorAnimator.SetBool("isOpen", false);
    }

    private void StartFire()
    {
        fireEffect.Play();
        StartCoroutine(StopFireAfterDelay(3.5f));
    }

    private IEnumerator StopFireAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        fireEffect.Stop();
    }

    public int GetRequiredSetting()
    {
        return requiredSetting;
    }

    public bool IsNeutralized()
    {
        return isNeutralized;
    }

    public void CheckAfterNeutralization()
    {
        if (isNeutralized)
        {
            isCheckedAfterNeutralization = true;
            // Debug.Log("The doors were checked again after neutralization.");
        }
    }
}
