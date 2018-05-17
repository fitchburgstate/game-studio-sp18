using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters
{
    public class PlayerInputModule : MonoBehaviour
    {
        #region Variables
        private DeviceManager deviceManager;

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
            var moveDirection = new Vector3(deviceManager.Move.x, 0, deviceManager.Move.y);
            var lookDirection = moveDirection;

            if (moveCharacter != null) { moveCharacter.Move(moveDirection, lookDirection); }

            if (deviceManager.PressedAttack && attackCharacter != null) { attackCharacter.Attack(); }

            else if (deviceManager.PressedDash && moveCharacter != null) { moveCharacter.Dash(); }

            else if (deviceManager.PressedInteract) { moveCharacter.Interact(); }

            else if (deviceManager.PressedPotion && player != null) { player.UsePotion(); }

            else if ((deviceManager.PressedElementDown || deviceManager.PressedElementUp) && attackCharacter != null)
            {
                attackCharacter.CycleElements(deviceManager.PressedElementUp);
            }

            else if ((deviceManager.PressedWeaponDown || deviceManager.PressedWeaponUp) && attackCharacter != null)
            {
                attackCharacter.CycleWeapons(deviceManager.PressedWeaponUp);
            }

            //Pause Game
            if (PauseManager.instance != null)
            {
                if (PauseManager.instance.IsGamePaused)
                {
                    if (deviceManager.PressedMenu || deviceManager.PressedJournal)
                    {
                        PauseManager.instance.UnpauseGame();
                    }

                    else if (deviceManager.PressedCancel)
                    {
                        if (PauseManager.instance.IsControlsOpen) { PauseManager.instance.CloseControls(); }
                        else { PauseManager.instance.UnpauseGame(); }
                    }

                    else if (deviceManager.PressedPageRight) { PauseManager.instance.SwitchPage(true); }

                    else if (deviceManager.PressedPageLeft) { PauseManager.instance.SwitchPage(false); }

                    else if (deviceManager.PressedTabRight) { PauseManager.instance.CurrentTabIndex++; }

                    else if (deviceManager.PressedTabLeft) { PauseManager.instance.CurrentTabIndex--; }
                }
                else
                {
                    if (deviceManager.PressedMenu) { PauseManager.instance.PauseGame(player, 0); }

                    else if (deviceManager.PressedJournal) { PauseManager.instance.PauseGame(player, 1); }
                }
            }
        }
    }
}
