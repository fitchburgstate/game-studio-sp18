using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;

public class ControllerInputModule : MonoBehaviour
{
    private int floorMask;
    private float cameraRayLength = 100f;
    private Vector3 cameraPos;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 headDirection = Vector3.zero;

    private GameObject player;
    private GameObject playerRoot;
    private DeviceManager myDeviceManager;
    private Camera mainCamera;



    private void Start()
    {
        player = gameObject.transform.GetChild(0).gameObject; // This will find the player-root gameobject, which means that the only child of this gameobject should be player-root
        playerRoot = gameObject;
        floorMask = LayerMask.GetMask("Floor");
        mainCamera = GameObject.FindObjectOfType<Camera>();
        myDeviceManager = mainCamera.GetComponent<DeviceManager>();
    }

    public void Update()
    {
        var moveCharacter = GetComponent<IMoveable>();
        var characterController = GetComponent<CharacterController>();
        var lookDirection = Vector3.zero;

        moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);

        if (!myDeviceManager.isController)
        {
            cameraPos.y = Input.mousePosition.y;
            cameraPos.x = Input.mousePosition.x;

            cameraPos = mainCamera.ScreenToWorldPoint(new Vector3(cameraPos.x, cameraPos.y, (player + -mainCamera.transform.position.z)));
            //var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //var floorHit = new RaycastHit();

            //if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
            //{
            //    var playerToMouse = floorHit.point - transform.position;
            //    playerToMouse.y = 0f;
            //    lookDirection = playerToMouse;
            //}
        }
        else
        {
            headDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);

            // If the left stick is being used and the right stick is not, adjust the character body to align with the left 
            if (moveDirection != Vector3.zero && headDirection == Vector3.zero)
            {
                lookDirection = moveDirection;
            }
            // If the right stick is being used, override the character body's rotation to align with the right stick
            else if (headDirection != Vector3.zero)
            {
                player.transform.parent = playerRoot.transform;
                lookDirection = headDirection;
            }
        }
        moveCharacter.Move(characterController, moveDirection, lookDirection, playerRoot);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
