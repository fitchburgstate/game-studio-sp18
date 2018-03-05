using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;
using Hunter.Character;

public class ControllerInputModule : MonoBehaviour
{
    //private Vector2 cameraPos;
    //private Vector3 worldCameraPos;
    
    /// <summary>
    /// Represents which direction the character should move in. Think of it as the input of the left stick.
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;

    /// <summary>
    /// Represents which direction the character should look in. Think of it as the input of the right stick.
    /// </summary>
    private Vector3 lookDirection = Vector3.zero;

    /// <summary>
    /// The model's gameobject. This exists so the model can be turned independently of the parent.
    /// </summary>
    private GameObject playerModel;

    /// <summary>
    /// This is the main camera in the scene.
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// This is the navmesh agent attached to the parent. The navmesh is used to find walkable area.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// The character controller that controls the player's movement.
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// The final direction that the character will face that's calculated.
    /// </summary>
    private Vector3 finalDirection;

    private DeviceManager myDeviceManager;
    private Player player;

    private void Start()
    {
        playerModel = gameObject.transform.GetChild(0).gameObject;

        mainCamera = GameObject.FindObjectOfType<Camera>();
        transform.forward = mainCamera.transform.forward;
        agent = GetComponent<NavMeshAgent>();

        myDeviceManager = mainCamera.GetComponent<DeviceManager>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        var moveCharacter = GetComponent<IMoveable>();
        characterController = GetComponent<CharacterController>();
        finalDirection = Vector3.zero;

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
        moveCharacter.Move(characterController, moveDirection, finalDirection, playerModel, agent);

        if (myDeviceManager.Device.RightBumper.WasReleased)
        {
            player.Attack();
        }
        else if (myDeviceManager.Device.LeftBumper.WasReleased)
        {
            player.SwitchWeapon();
        }
    }
}
