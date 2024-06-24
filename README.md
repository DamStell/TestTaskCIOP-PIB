# TestTaskCIOP-PIB
 
Scene1
![scene1](https://github.com/DamStell/TestTaskCIOP-PIB/blob/main/scene1.png)
Scene2
![scene2](https://github.com/DamStell/TestTaskCIOP-PIB/blob/main/scene2.png)

Scripts Documentation 

Script: LaserController

The LaserController script is responsible for controlling a laser pointer in the game. The laser automatically adjusts its rotation to the point the camera is looking at, drawing a line and highlighting objects it points to. During interactions with interactive objects, the laser changes the object's color to green to indicate selection.

Fields:
LineRenderer lineRenderer: LineRenderer component used for drawing the laser.
float maxDistance = 100f: Maximum distance the laser can reach.
LayerMask interactableLayer: Layer of objects that can be interactive.
Camera mainCamera: Main camera in the scene.
GameObject highlightedObject: Currently highlighted object.
Color originalColor: Original color of the highlighted object.
bool originalColorSet = false: Flag indicating whether the original color has been set.

Methods:
void Start(): Initializes the camera and sets the laser width.
void Update(): Calls the RotateLaser and DrawLaser methods.
void RotateLaser(): Rotates the laser to point at where the camera is looking.
void DrawLaser(): Draws the laser and highlights the objects it points to.
void HighlightObject(GameObject obj): Highlights an object.
void RemoveHighlight(): Removes the highlight from an object.


Script: WardrobeController
This script is responsible for controlling drawers in a wardrobe, allowing opening and closing of drawers and moving items within the drawers.

Fields:
GameObject[] Drawers: Array of drawers.
GameObject[] frontDrawers: Array of drawer fronts.
GameObject[] wardrobe1Items: Array of items in drawer 1.
GameObject[] wardrobe2Items: Array of items in drawer 2.
GameObject[] wardrobe3Items: Array of items in drawer 3.
float drawerMoveDistance = 4.0f: Distance the drawer moves.
float itemMoveDistance = 2.0f: Distance the items move.
float drawerMoveSpeed = 2.0f: Speed of drawer movement.
float itemMoveSpeed = 1.0f: Speed of item movement.
float holdDelay = 0.3f: Delay for opening/closing drawers.
GameObject currentDrawer: Currently opened drawer.
GameObject[] currentItems: Items in the currently opened drawer.
bool isMoving = false: Flag indicating whether a drawer is moving.
bool isOpening = false: Flag indicating whether a drawer is opening.
GameObject targetDrawer: Target drawer to be opened.
GameObject[] targetItems: Items in the target drawer.
Coroutine holdCoroutine: Coroutine handling the delay.

Methods:
void Start(): Sets the initial positions of drawers and items as closed.
void FixedUpdate(): Checks if the laser is pointing at a drawer.
void CheckMouseOver(): Checks if the laser is pointing at the front of a drawer.
IEnumerator HoldToToggleDrawer(int drawerIndex): Handles the delay for opening/closing a drawer.
GameObject[] GetWardrobeItems(int drawerIndex): Returns the items in the specified drawer.
IEnumerator ToggleDrawers(): Opens/closes drawers and moves items.
IEnumerator CloseCurrentDrawer(): Closes the current drawer.
IEnumerator MoveObjectsSimultaneously(GameObject drawer, GameObject[] items, float drawerDistance, float itemDistance, float drawerSpeed, float itemSpeed): Smoothly moves drawers and items.
void SetDrawerPosition(GameObject drawer, float distance): Sets the position of the drawer.
void SetItemsPosition(GameObject[] items, float distance): Sets the positions of the items.


Script: ItemController
This script allows interaction with items in the game. The user can pick up items using the laser to point and move them to designated targets, such as a character or drawer.

Fields:
LayerMask interactableLayer: Layer of interactive objects.
Transform avatarTarget: Target for the carried item.
Transform drawerTarget: Target for the item in the drawer.
bool isHeld = false: Flag indicating whether the item is held.
Transform originalParent: Original parent of the item.
Vector3 originalPosition: Original position of the item.
Coroutine holdCoroutine: Coroutine handling the holding delay.
GameObject currentItem: Currently held item.
float holdDelay = 0.5f: Delay for picking up the item.
Plane movementPlane: Plane for item movement.
static bool isAnyItemHeld = false: Static variable indicating if any item is held.

Methods:
void Start(): Sets the original parent and position of the item.
void Update(): Updates the state of the item (picking up/moving).
void CheckLaserHover(): Checks if the laser is pointing at the item.
IEnumerator StartHolding(Vector3 hitPoint): Handles the holding delay.
void FollowLaser(): Makes the item follow the laser.
void CheckForTarget(): Checks if the item is over a target.
bool IsTargetOccupied(Transform target): Checks if the target is occupied.
void PlaceItem(Transform target): Places the item on the target.
void ResetItemPosition(): Resets the item's position.


Script: AvatarItemChecker
Monitors whether all required items have been placed on the character. When all targets on the character are occupied by items, the script uses a screen fading effect and loading to transition to the next game scene.

Fields:
Transform[] avatarTargets: Targets on the avatar.
string nextSceneName: Name of the next scene.
ScreenFader screenFader: Component responsible for the screen fade effect.

Methods:
void Update(): Checks if all items are in place and loads the next scene.
bool CheckAllItemsEquipped(): Checks if all targets on the avatar are occupied by items.


Script: ScreenFader
The ScreenFader script is responsible for the screen fading effect during scene transitions.

Fields:
public Image fadeImage: Image used as an overlay for the screen fade.
public float fadeDuration = 1.0f: Duration of the fade effect in seconds.

Methods:
public void FadeOutAndLoadScene(string sceneName): Starts the coroutine for fading out the screen and loading a new scene.
private IEnumerator FadeOut(string sceneName): Coroutine responsible for the screen fade.


Script: PlayerController

This script manages the main player functions, including movement, rotation, shooting, and interactions with doors and rooms. It also tracks game statistics such as lives, game time, and the number of correct and incorrect actions.
Fields:
static instance: Singleton instance of PlayerController.
int lives: Number of player lives, default is 3.
float ringSetting: Current ring setting, default is 0.
float startTime: Game start time.
TextMeshPro ringSettingText: Text displaying the current ring setting.
DeviceController measurementDevice: Measurement device.
float moveSpeed = 5f: Player movement speed.
float lookSensitivity = 2f: Camera sensitivity.
ParticleSystem shotEffect: Shooting particle effect.
CharacterController characterController: Character controller component.
Transform cameraTransform: Camera transform.
int totalRooms = 4: Total number of rooms to neutralize.
int roomsNeutralized = 0: Number of neutralized rooms.
LifeDisplayController lifeDisplayController: Life display controller.
ResultPanelController resultPanel: Result panel controller.
float totalTime: Total game time.
int doorsNeutralized: Number of neutralized doors.
int correctShots: Number of correct shots.
int correctShotsInRoom: Number of correct shots in the room.
int errorsNoMeasurement: Number of no measurement errors.
int errorsHighRingSetting: Number of high ring setting errors.
int errorsLowRingSetting: Number of low ring setting errors.
int errorsWrongRingSettingRoom: Number of wrong ring setting errors in the room.
int errorsSkippedWalls: Number of skipped walls.
int errorsMultipleHits: Number of walls hit more than once.
int exitRoomCounter: Number of exited rooms.
int OpenDoorCounter: Number of opened doors.
bool IsPlay = true: Game state flag, default is true.

Methods:
void Awake(): Sets the singleton instance.
void Start(): Initializes components and sets initial values.
void Update(): Updates game logic, movement, rotation, ring setting, interaction, shooting, measurements, and life display.
void HandleMovement(): Manages player movement.
void HandleRotation(): Manages camera rotation.
void HandleRingSetting(): Manages ring setting.
void HandleShooting(): Manages shooting and interactions with doors and rooms.
void HandleDeviceMeasurement(): Manages device measurements.
IEnumerator DisplayMeasurementForSeconds(float seconds): Coroutine to display the measurement for a specified time.
void HandleInteraction(): Manages interactions with doors.
float GetRingSetting(): Returns the current ring setting.
void StartShot(): Starts the shooting effect.
IEnumerator StopShotAfterDelay(float delay): Coroutine stopping the shooting effect after a specified time.
void LoseLife(): Manages life loss.
void DisplayLives(): Displays lives in the UI.
void NeutralizeRoom(): Manages room neutralization.
void DisplayResults(): Displays game results.


Script: DoorController

The DoorController script is responsible for the behavior of doors in the game, including their neutralization and interaction with the player. Doors can be neutralized by setting the appropriate ring value, and incorrect interactions result in the player losing a life.
Fields:
requiredSetting (int): The required ring value to neutralize the door.
isNeutralized (bool): Flag indicating whether the door is neutralized.
isOpen (bool): Flag indicating whether the door is open.
isCheckedAfterNeutralization (bool): Flag indicating whether the door has been checked after neutralization.
doorAnimator (Animator): The door's animator.
fireEffect (ParticleSystem): The fire effect.

Methods:
Start(): Initializes the required ring value.
Neutralize(float ringSetting): Neutralizes the door.
Interact(): Interacts with the door, opening or closing it.
OpenDoor(): Opens the door.
CloseDoor(): Closes the door.
StartFire(): Starts the fire effect.
StopFireAfterDelay(float delay): Coroutine that stops the fire effect after a specified time.
GetRequiredSetting(): Returns the required ring value to neutralize the door.
IsNeutralized(): Returns whether the door is neutralized.
CheckAfterNeutralization(): Marks the door as checked after neutralization.


Script: RoomController

The RoomController script manages the state of a room, tracking hits on various surfaces and checking whether all surfaces have been properly neutralized by the player.
Fields:
surfacesHit (Dictionary<GameObject, bool>): A dictionary of surfaces in the room and their hit status.
isPlayerInRoom (bool): Flag indicating whether the player is in the room.

Methods:
Start(): Initializes the dictionary of surfaces in the room.
OnTriggerEnter(Collider other): Handles the player entering the room.
OnTriggerExit(Collider other): Handles the player exiting the room.
CheckHit(GameObject hitObject, float ringSetting): Checks the hit on a surface in the room.
HandleWalldoorHit(float ringSetting): Handles a hit on the door surface.
ChangeColor(GameObject obj, Color color): Coroutine that changes the color of a hit surface.
AllSurfacesHit(): Checks if all surfaces in the room have been hit.


Script: LifeDisplayController

The LifeDisplayController script is responsible for displaying the player's lives in the form of hearts on the screen. It updates the number of hearts based on the player's current number of lives.
Fields:
heartPrefab: The heart prefab representing a life.
heartsParent: The parent for heart objects.
hearts: List of heart objects.

Methods:
SetLives(int lives): Sets the number of lives displayed.


Script: ResultPanelController
The ResultPanelController script controls the display of the result panel at the end of the game, presenting the game time, the number of properly neutralized doors and rooms, and detailed statistics of errors made by the player.
Fields:
resultsText: The text displaying the results.
resultPanel: The result panel.

Methods:
DisplayResults(): Displays the game results.


Script: GameManager

The GameManager script manages overall game functions, such as resetting the scene.
Methods:
ResetScene(): Resets the current scene.






