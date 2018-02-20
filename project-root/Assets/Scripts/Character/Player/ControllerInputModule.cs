using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;

public class ControllerInputModule : MonoBehaviour
{
    private Vector2 cameraPos;
    private Vector3 worldCameraPos;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private GameObject playerRoot;
    private DeviceManager myDeviceManager;
    private Camera mainCamera;
    private NavMeshAgent agent;

    private void Start()
    {
        playerRoot = gameObject.transform.GetChild(0).gameObject; // This will find the player-root gameobject, which means that the only child of this gameobject should be player-root

        mainCamera = GameObject.FindObjectOfType<Camera>();
        myDeviceManager = mainCamera.GetComponent<DeviceManager>();
        transform.forward = mainCamera.transform.forward;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        var moveCharacter = GetComponent<IMoveable>();
        var characterController = GetComponent<CharacterController>();
        var finalDirection = Vector3.zero;

        moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);

        if (!myDeviceManager.isController)
        {
            //cameraPos.x = Input.mousePosition.x;
            //cameraPos.y = Input.mousePosition.y;
            ////worldCameraPos = mainCamera.ScreenToWorldPoint(new Vector3(cameraPos.x, cameraPos.y, (playerRoot.transform.position.z + -(mainCamera.transform.position.z))));


            //finalDirection = worldCameraPos;
        }
        else
        {
            lookDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);

            // If the left stick is being used and the right stick is not, adjust the character body to align with the left 
            if (moveDirection != Vector3.zero && lookDirection == Vector3.zero)
            {
                finalDirection = moveDirection;
            }
            // If the right stick is being used, override the character body's rotation to align with the right stick
            else if (lookDirection != Vector3.zero)
            {
                finalDirection = lookDirection;
            }
        }
        moveCharacter.Move(characterController, moveDirection, finalDirection, playerRoot, agent);
    }
}
