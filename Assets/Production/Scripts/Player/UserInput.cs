using UnityEngine;
using System.Collections;
using RTS;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{

    public Player player;
    public bool gamePaused = false;
    public bool alternateZoom = false;
    public Texture2D moveCameraCursor;

    [SerializeField]
    private LayerMask lootTargetLayer;

    [SerializeField]
    private Texture2D defaultCursor;
    [SerializeField]
    private Texture2D pickupLootCursor;

    private int excludeLayer1;
	private ButtonManager buttonManagerScr;


    void Awake()
    {
        ChooseLayer();
    }

    // Use this for initialization
    void Start()
    {
        //player = transform.root.GetComponent<Player>();
        player = transform.root.GetComponentInChildren<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player && player.humanPlayer)
        {
            LootRay();
            MoveCamera();
            RotateCamera();
            SetDefaultMouseCursor();
            //MouseActivity();
            PauseGame();
            ShowHealthBars();
        }
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        //horizontal camera movement
        if ((xpos >= 0) && (xpos < ResourceManager.ScrollWidth)&&(Input.GetMouseButton(1)))
        {
            Cursor.SetCursor(moveCameraCursor, Vector2.zero, CursorMode.Auto);
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if ((xpos <= Screen.width) && (xpos > Screen.width - ResourceManager.ScrollWidth) && (Input.GetMouseButton(1)))
        {
            Cursor.SetCursor(moveCameraCursor, Vector2.zero, CursorMode.Auto);
            movement.x += ResourceManager.ScrollSpeed;
        }

        //vertical camera movement
        if ((ypos >= 0) && (ypos < ResourceManager.ScrollWidth) && (Input.GetMouseButton(1)))
        {
            Cursor.SetCursor(moveCameraCursor, Vector2.zero, CursorMode.Auto);
            movement.z -= ResourceManager.ScrollSpeed;
        }
        else if ((ypos <= Screen.height) && (ypos > Screen.height - ResourceManager.ScrollWidth) && (Input.GetMouseButton(1)))
        {
            Cursor.SetCursor(moveCameraCursor, Vector2.zero, CursorMode.Auto);
            movement.z += ResourceManager.ScrollSpeed;
        }
       
        /**/
        //make sure movement is in the direction the camera is pointing
        //but ignore the vertical tilt of the camera to get sensible scrolling
        
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //away from ground movement
        if (alternateZoom)
        {
            float cameraFOV = Camera.main.fieldOfView;
            cameraFOV -= Input.GetAxis("Mouse ScrollWheel") * ResourceManager.ScrollSpeed;
            cameraFOV = Mathf.Clamp(cameraFOV, ResourceManager.MinCameraFOV, ResourceManager.MaxCameraFOV);
            Camera.main.fieldOfView = cameraFOV;
        }
        else
        {
            movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
        }

        //calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        if (destination.y > ResourceManager.MaxCameraHeight)
        {
            destination.y = ResourceManager.MaxCameraHeight;
        }
        else if (destination.y < ResourceManager.MinCameraHeight)
        {
            destination.y = ResourceManager.MinCameraHeight;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            //destination.x = Mathf.Clamp(destination.x, rotMinX, rotMaxX);
            //destination.y = Mathf.Clamp(destination.y, rotMinY, rotMaxY);
            
            if (destination.x >=55f)
            {
                destination.x = 55f;
            }
            if (destination.x <= 30f)
            {
                destination.x = 30f;
            }
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
            //Mathf.Clamp(Camera.main.transform.rotation.y, rotMinY, rotMaxY);
        }
    }

    

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                gamePaused = false;
                Time.timeScale = 1;
            }
            else if (!gamePaused)
            {
                gamePaused = true;
                Time.timeScale = 0;
            }
            
        }
    }

    private void SetDefaultMouseCursor()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void ShowHealthBars()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameMainManager.Instance.showHealthBars = !GameMainManager.Instance.showHealthBars;
        }
    }
    

    private void LootRay()
    {
		GetButtonManager();

		if(!buttonManagerScr.isItemSelected)
		{
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit Hit;
	        if (Physics.Raycast(ray, out Hit, 1000, lootTargetLayer))
	        {
	            GameObject lootItem = Hit.collider.gameObject;
	            if (lootItem.tag == "LootObject")
	            {
	                Cursor.SetCursor(pickupLootCursor, Vector2.zero, CursorMode.Auto);
	                if (Input.GetMouseButton(0))
	                {
	                    LootTrigger lootObject = lootItem.GetComponent<LootTrigger>();
	                    lootObject.AwardLoot();
	                }
	            }
	        }
	        else
	        {
	            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
	        }
		}
    }

    private void ChooseLayer()
    {
        excludeLayer1 = LayerMask.NameToLayer("LootLayer");
        lootTargetLayer = 1 << excludeLayer1;
    }

	private void GetButtonManager()
	{
		buttonManagerScr = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
	}
}