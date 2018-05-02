using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters
{
    public class PlayerInputModule : MonoBehaviour
    {
        #region Variables
        private DeviceManager deviceManager;

        private bool inPauseMenu = false;

        private Player player;
        private IAttack attackCharacter;
        private IMoveable moveCharacter;
        #endregion

        private void Start ()
        {
            //Whoever this script is on is being controlled by the Player, so naturally they should be tagged as such
            transform.tag = "Player";

            deviceManager = GameManager.instance?.DeviceManager;
            attackCharacter = GetComponent<IAttack>();
            moveCharacter = GetComponent<IMoveable>();
            player = GetComponent<Player>();
        }

        private void Update ()
        {
            //Game Input Actions
            if (!inPauseMenu)
            {
                var moveDirection = new Vector3(deviceManager.Move.x, 0, deviceManager.Move.y);
                var lookDirection = moveDirection;

                if (moveCharacter != null)
                {
                    moveCharacter.Move(moveDirection, lookDirection);
                }

                if (deviceManager.PressedAttack && attackCharacter != null)
                {
                    attackCharacter.Attack();
                }
                else if (deviceManager.PressedDash && moveCharacter != null)
                {
                    moveCharacter.Dash();
                }
                else if (deviceManager.PressedInteract)
                {
                    moveCharacter.Interact();
                }
                else if(deviceManager.PressedPotion && player != null)
                {
                    player.UsePotion();
                }
                else if (deviceManager.PressedSwitchRanged || deviceManager.PressedSwitchMelee && attackCharacter != null)
                {
                    attackCharacter.SwitchWeaponType(deviceManager.PressedSwitchMelee);
                }
                else if ((deviceManager.PressedElementDown || deviceManager.PressedElementUp) && attackCharacter != null)
                {
                    attackCharacter.CycleElements(deviceManager.PressedElementUp);
                }
                else if ((deviceManager.PressedWeaponDown || deviceManager.PressedWeaponUp) && attackCharacter != null)
                {
                    attackCharacter.CycleWeapons(deviceManager.PressedWeaponUp);
                }


                if (deviceManager.PressedMenu || deviceManager.PressedJournal)
                {
                    if (PauseManager.instance == null)
                    {
                        Debug.LogWarning("Can't pause the game because the pause scene was never loaded.");
                    }
                    else
                    {
                        PauseManager.instance.PauseGame(GetComponent<Player>());
                        inPauseMenu = true;
                    }
                }
            }
            //UI Input Actions
            else
            {
                if (deviceManager.PressedMenu || deviceManager.PressedJournal || deviceManager.PressedCancel)
                {
                    PauseManager.instance.UnpauseGame();
                    inPauseMenu = false;
                }
                else if (deviceManager.PressedPageRight)
                {
                    PauseManager.instance.DisplayDiaries();
                }
                else if (deviceManager.PressedPageLeft)
                {
                    PauseManager.instance.DisplayJournals();
                }
            }
        }
    }
}
