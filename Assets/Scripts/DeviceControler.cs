using UnityEngine;
using TMPro;

public class DeviceControler : MonoBehaviour
{
    [SerializeField] TextMeshPro measurementText;
    private float currentMeasurement = 0;
    [SerializeField] LayerMask ignoreLayerMask;

    public void TakeMeasurement()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayerMask))
        {
            DoorController door = hit.collider.GetComponent<DoorController>();
            if (door != null)
            {
                currentMeasurement = door.GetRequiredSetting();
                if (door.IsNeutralized())
                {
                    door.CheckAfterNeutralization();
                }
            }
            else
            {
                currentMeasurement = 0;
            }
        }

        measurementText.text = currentMeasurement.ToString();
        measurementText.gameObject.SetActive(true);
    }

    public void DisplayMeasurement(bool display)
    {
        measurementText.gameObject.SetActive(display);
    }
}
