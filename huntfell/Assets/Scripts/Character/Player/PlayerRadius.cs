using Hunter.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class PlayerRadius : MonoBehaviour
    {
        #region Variables
        public List<Transform> interactableInRadiusTransform = new List<Transform>();

        private List<GameObject> enemiesInRadius = new List<GameObject>();
        private string tempSoundEvent;
        #endregion

        #region Unity Functions
        private void OnTriggerEnter(Collider other)
        {
            var musicSoundEvent = "";

            var interactable = other.GetComponent<IInteractable>();
            var enemy = other.GetComponent<Enemy>();

            if (interactable != null && interactable.IsImportant && !interactableInRadiusTransform.Contains(other.transform))
            {
                interactableInRadiusTransform.Add(other.transform);
            }
            else if (enemy != null && !enemiesInRadius.Contains(enemy.gameObject))
            {
                musicSoundEvent = "Music - Expo to Regular Combat";
                enemiesInRadius.Add(enemy.gameObject);
            }

            // Update the list to make sure it does not have any null gameobject's in it
            UpdateList(enemiesInRadius);

            if (enemiesInRadius.Count < 1)
            {
                musicSoundEvent = "Music - Regular Combat to Expo";
            }

            if (musicSoundEvent != tempSoundEvent) { Fabric.EventManager.Instance?.PostEvent(musicSoundEvent); }
            tempSoundEvent = musicSoundEvent;
        }

        private void OnTriggerExit(Collider other)
        {
            var musicSoundEvent = "";

            var enemy = other.GetComponent<Enemy>();
            var interactable = other.GetComponent<IInteractable>();

            if (interactable != null && interactable.IsImportant && interactableInRadiusTransform.Contains(other.transform))
            {
                interactableInRadiusTransform.Remove(other.transform);
            }
            else if (enemy != null && enemiesInRadius.Contains(enemy.gameObject))
            {
                enemiesInRadius.Remove(enemy.gameObject);
            }

            // Update the list to make sure it does not have any null gameobject's in it
            UpdateList(enemiesInRadius);

            if (enemiesInRadius.Count < 1)
            {
                musicSoundEvent = "Music - Regular Combat to Expo";
            }

            if (musicSoundEvent != tempSoundEvent) { Fabric.EventManager.Instance?.PostEvent(musicSoundEvent); }
            tempSoundEvent = musicSoundEvent;
        }
        #endregion

        #region Helper Functions
        private void UpdateList(List<GameObject> gameobjectList)
        {
            foreach (var listItem in gameobjectList)
            {
                if (listItem == null)
                {
                    gameobjectList.Remove(listItem);
                }
            }
        }
        #endregion
    }
}
