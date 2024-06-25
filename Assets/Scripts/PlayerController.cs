using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] int lives = 3;
    private float ringSetting = 0.0f;
    private float startTime;

    [SerializeField] TextMeshPro ringSettingText;
    [SerializeField] DeviceControler measurementDevice;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float lookSensitivity = 2f;
    [SerializeField] ParticleSystem shotEffect;

    private CharacterController characterController;
    private Transform cameraTransform;
    public int totalRooms = 4;
    public int roomsNeutralized = 0;
    public LifeDisplayController lifeDisplayController;
    public ResultPanelController resultPanel;

    public float totalTime = 0.0f;
    public int doorsNeutralized = 0;
    public int correctShots = 0;

    public int correctShotsInRoom = 0;
    public int errorsNoMeasurement = 0;
    public int errorsHighRingSetting = 0;
    public int errorsLowRingSetting = 0;
    public int errorsWrongRingSettingRoom = 0;
    public int errorsSkippedWalls = 0;
    public int errorsMultipleHits = 0;
    public int exitRoomCounter = 0;
    public int OpenDoorCounter = 0;
    private bool IsPlay = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        startTime = Time.time;
        LockCursor();
        UpdateRingSettingText();
        UpdateLifeDisplay();
    }

    void Update()
    {

        UpdateLifeDisplay();
        if (IsPlay)
        { 
        
            HandleMovement();
            HandleRotation();
            HandleRingSetting();
            HandleInteraction();
            HandleShooting();
            HandleDeviceMeasurement();
           // totalTime = Time.time - startTime;
                 // if the player opens all doors and leaves each room, the game will end
            if (OpenDoorCounter>=totalRooms && exitRoomCounter>=totalRooms)
            {
                ShowResults();
            }
        }
   
    }

    private void HandleMovement()
    {
        float moveDirectionY = characterController.velocity.y;
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if (characterController.isGrounded)
        {
            // Allow gravity to apply when grounded
            moveDirectionY = 0;
        }
        else
        {
            // Apply gravity when not grounded
            moveDirectionY -= 9.81f * Time.deltaTime;
        }

        move.y = moveDirectionY;

        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.Rotate(Vector3.right * -mouseY);
        float xRotation = cameraTransform.localEulerAngles.x;
        if (xRotation > 180)
        {
            xRotation -= 360;
        }
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        cameraTransform.localEulerAngles = new Vector3(xRotation, cameraTransform.localEulerAngles.y, 0);
    }

       private void HandleRingSetting()
    {
        for (int i = 0; i <= 9; i++)
        {
            KeyCode alphaKey = (KeyCode)((int)KeyCode.Alpha0 + i);
            KeyCode keypadKey = (KeyCode)((int)KeyCode.Keypad0 + i);

            if (Input.GetKeyDown(alphaKey) || Input.GetKeyDown(keypadKey))
            {
                ringSetting = i;
                UpdateRingSettingText();
                Debug.Log("Ustawienie pierścienia: " + ringSetting);
                break;
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // Instantiate(shotEffect, hit.point, Quaternion.LookRotation(hit.normal));
                StartShot();


                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                   
                    if (ringSetting == door.GetRequiredSetting())
                    {
                        doorsNeutralized++;
                    }
                    else if (ringSetting > door.GetRequiredSetting())
                    {
                        errorsHighRingSetting++;
                    }
                    else if (ringSetting < door.GetRequiredSetting())
                    {
                        errorsLowRingSetting++;
                    }

                    door.Neutralize(ringSetting);
                }
                else
                {
                    RoomController room = hit.collider.GetComponentInParent<RoomController>();
                    if (room != null)
                    {
                        room.CheckHit(hit.collider.gameObject, ringSetting);
                        if (room.AllSurfacesHit())
                        {
                            roomsNeutralized++;
                            //correctShots++;
                            if (roomsNeutralized >= totalRooms)
                            {
                                ShowResults();
                            }
                        }
                    }
                }
            }
        }
    }

    private void HandleDeviceMeasurement()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            measurementDevice.TakeMeasurement();
            StartCoroutine(DisplayMeasurementForSeconds(3));
        }
    }

    private IEnumerator DisplayMeasurementForSeconds(float seconds)
    {
        measurementDevice.DisplayMeasurement(true);
        yield return new WaitForSeconds(seconds);
        measurementDevice.DisplayMeasurement(false);
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Used E");

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Door");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Raycast trafil w: " + hit.collider.gameObject.name);

                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    Debug.Log("DoorController found");
                    door.Interact();
                }
                else
                {
                    Debug.Log("DoorController null");
                }
            }
            else
            {
                Debug.Log("Raycast hit in empty");
            }
        }
    }

    public float GetRingSetting()
    {
        return ringSetting;
    }
    private void StartShot()
    {
        shotEffect.Play();
        StartCoroutine(StopShotAfterDelay(0.1f));
    }

    private IEnumerator StopShotAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shotEffect.Stop();
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log("Lose Life. life: " + lives);
        if (lives <= 0)
        {
            //resultPanel.DisplayResults();
            ShowResults();
            //resultPanel.DisplayResults();
        }
    }

    private void UpdateRingSettingText()
    {
        if (ringSettingText != null)
        {
            ringSettingText.text = "" + ringSetting.ToString();
        }
    }

    private void ShowResults()
    {
        IsPlay = false;    
        totalTime = Time.time - startTime;
        UnlockCursor();
        resultPanel.DisplayResults();
   
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateLifeDisplay()
    {
        if (lifeDisplayController != null)
        {
            lifeDisplayController.SetLives(lives);
        }
    }

        private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
