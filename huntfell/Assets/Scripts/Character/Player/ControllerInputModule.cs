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
    private Vector3 animLookDirection = Vector3.zero;

    private DeviceManager myDeviceManager;
    private Player player;
    private IMoveable moveCharacter;

    private void Start()
    {
        var mainCamera = Camera.main;
        transform.forward = mainCamera.transform.forward;
        myDeviceManager = mainCamera.GetComponent<DeviceManager>();

        player = GetComponent<Player>();
        moveCharacter = GetComponent<IMoveable>();
    }

    private void Update()
    {
        moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);

        if (!myDeviceManager.isController)
        {
            //Working DONT DELETE
            //RaycastHit hit;
            //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //var test = 32f;
            //if (Physics.Raycast(ray, out hit, test))
            //{
            //    lookDirection = new Vector3(hit.point.x, playerRoot.transform.position.y, hit.point.z);
            //    playerRoot.transform.LookAt(new Vector3(hit.point.x, playerRoot.transform.position.y, hit.point.z));
            //}
            //else
            //{
            //    var rayvect = ray.getpoint(test);
            //    lookdirection = new vector3(rayvect.x, playerroot.transform.position.y, rayvect.z);
            //    playerroot.transform.lookat(new vector3(rayvect.x, playerroot.transform.position.y, rayvect.z));
            //}
        }
        else
        {
            lookDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);
            animLookDirection = lookDirection;
            // If the left stick is being used and the right stick is not, adjust the character body to align with the left 
            if (moveDirection != Vector3.zero && lookDirection == Vector3.zero)
            {
                lookDirection = moveDirection;
            }

        }
        moveCharacter.Move(moveDirection, lookDirection, animLookDirection);

        var device = myDeviceManager.Device;
        if (device == null) {
            Debug.LogWarning("No devices are attatched to the Device Manager.");
            return;
        }

        if (device.RightBumper.WasReleased)
        {
            player.Attack();
        }
        else if (device.LeftBumper.WasReleased)
        {
            player.SwitchWeapon();
        }
        else if (device.Action1.WasPressed)
        {
            player.Dash();
        }
    }
}
