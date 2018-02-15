using System;
using UnityEngine;
using System.Linq;
using InControl;
using Hunter;

public interface IMoveable
{
    void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection);

    void Dash(CharacterController controller);

}

public class ControllerInputModule : MonoBehaviour
{
    private DeviceManager myDeviceManager;
    private int floorMask;
    private float cameraRayLength = 100f;

    private void Start ()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

    public void Update ()
    {
        var moveBitch = GetComponent<IMoveable>();
        var cc = GetComponent<CharacterController>();
        var moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);
        Vector3 lookDirection = Vector3.zero;
        if (!myDeviceManager.isController)
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var floorHit = new RaycastHit();

            if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
            {
                var playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                lookDirection = playerToMouse;
            }
        }
        else
        {
            var headDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);

            // If the left stick is being used and the right stick is not, adjust the character body to align with the left 
            if (moveDirection != Vector3.zero && headDirection == Vector3.zero)
            {
                lookDirection = moveDirection;
            }
            // If the right stick is being used, override the character body's rotation to align with the right stick
            else if (headDirection != Vector3.zero)
            {
                lookDirection = headDirection;
            }
        }
        moveBitch.Move(cc, moveDirection, lookDirection);
    }
}

public class AIInputModule : MonoBehaviour
{
    private DeviceManager myDeviceManager;

    public void Update ()
    {
        var moveBitch = GetComponent<IMoveable>();
        var cc = GetComponent<CharacterController>();
        moveBitch.Move(cc, myDeviceManager.Device.inputs)
    }
}

//public interface IDamageable<T, V>
//{
//    void TakeDamage(T healthValue, V amount);

//    void DealDamage(T targetHealthValue, V amount);
//}

//public interface IHealth<T>
//{
//    void SetMaxHealth(T amount);

//    void SetStartingHealth(T amount);

//    void SetCurrentHealth(T amount);

//}
