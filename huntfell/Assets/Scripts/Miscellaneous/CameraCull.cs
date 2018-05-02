using Hunter.Characters;
using System.Collections.Generic;
using UnityEngine;

/*    THE ROUTINE FOR CULLING AWAY OBJECTS IN FRONT OF THE CAMERA
 * 1) Linecast from camera to player 
 * 2) If linecast is false and items are still in cached list, turn all back to opaque, if false just return;
 * 3) If true, RaycastHitAll using same line from linecast 
 * 4) Compare new list to cached list, if in old list but not new list make opaque; if in new list but not in old make transparent
 * 5) Old cached list = new list
 */

namespace Hunter
{
    public class CameraCull : MonoBehaviour
    {
        #region Variables
        public Shader shaderToOverlay;
        [Space]
        public LayerMask cullingMask;

        private List<Material> playerMaterials = new List<Material>();
        private List<Shader> cachedShaders = new List<Shader>();
        private Player playerCharacter;
        #endregion

        #region Properties
        public Player PlayerCharacter
        {
            get
            {
                if (playerCharacter == null)
                {
                    var pcGO = GameObject.FindGameObjectWithTag("Player");
                    if (pcGO == null)
                    {
                        Debug.LogWarning("Could not find a GameObject tagged 'Player' in the scene.");
                        return null;
                    }

                    playerCharacter = pcGO.GetComponent<Player>();
                    if (playerCharacter == null)
                    {
                        Debug.LogWarning("The Player does not have the proper Character script attached to them.", pcGO);
                        return null;
                    }
                }
                return playerCharacter;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            var playerRenderer = PlayerCharacter.GetComponentInChildren<Renderer>().materials;

            for (var i = 0; i < playerRenderer.Length; i++)
            {
                playerMaterials.Add(playerRenderer[i]);
                cachedShaders.Add(playerRenderer[i].shader);
            }
        }

        private void FixedUpdate()
        {
            if (Physics.Linecast(transform.position, PlayerCharacter.EyeLineTransform.position, cullingMask))
            {
                for (var i = 0; i < playerMaterials.Count; i++)
                {
                    playerMaterials[i].shader = shaderToOverlay;
                }
            }
            else
            {
                for (var i = 0; i < playerMaterials.Count; i++)
                {
                    playerMaterials[i].shader = cachedShaders[i];
                }
            }
        }
        #endregion
    }
}
