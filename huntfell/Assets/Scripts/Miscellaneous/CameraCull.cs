using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hunter.Character;

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
        #region Properties
        public Character.Character PlayerCharacter
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

                    playerCharacter = pcGO.GetComponent<Character.Character>();
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

        #region Variables
        [Tooltip("The amount the object will fade by.")]
        [Range(0, 1)]
        public float transparencyAmount = 0.3f;
        [Tooltip("The amount the object will return to.")]
        [Range(0, 1)]
        public float opaqueAmount = 1f;

        public LayerMask cullingMask;

        private List<GameObject> cachedList;
        private Shader cachedShader;
        private Character.Character playerCharacter;
        #endregion

        private void Start()
        {
            cachedList = new List<GameObject>();
            cachedShader = new Shader();
        }

        private void FixedUpdate()
        {
            if (Physics.Linecast(transform.position, PlayerCharacter.eyeLine.position, cullingMask.value))
            {
                var hits = Physics.RaycastAll(transform.position, (PlayerCharacter.eyeLine.position - transform.position), Vector3.Distance(PlayerCharacter.eyeLine.position, transform.position), cullingMask.value);
                Debug.DrawLine(transform.position, PlayerCharacter.eyeLine.position, Color.blue, 5);

                if (hits.Length > 0)
                {
                    var newList = new List<GameObject>();
                    foreach (var hit in hits)
                    {
                        newList.Add(hit.transform.gameObject);
                    }

                    var makeTransparentList = newList.Except(cachedList).ToList();
                    var makeOpaqueList = cachedList.Except(newList).ToList();

                    for (var i = 0; i < makeTransparentList.Count; i++)
                    {
                        var objectToChange = makeTransparentList[i];
                        var rend = objectToChange.transform.GetComponentInChildren<Renderer>();
                        cachedShader = rend.material.shader;
                        MakeTransparent(rend);
                    }

                    for (var i = 0; i < makeOpaqueList.Count; i++)
                    {
                        var objectToChange = makeOpaqueList[i];
                        var rend = objectToChange.transform.GetComponentInChildren<Renderer>();
                        MakeOpaque(rend);
                    }
                    cachedList = makeTransparentList;
                }
            }
            else if (cachedList.Count > 0)
            {
                for (var i = 0; i < cachedList.Count; i++)
                {
                    var objectToChange = cachedList[i];
                    var rend = objectToChange.transform.GetComponentInChildren<Renderer>();
                    MakeOpaque(rend);
                }
                cachedList.Clear();
            }
        }

        private void MakeOpaque(Renderer rend)
        {
            if (rend)
            {
                rend.material.shader = cachedShader;
                var tempColor = rend.material.color;
                tempColor.a = opaqueAmount;
                rend.material.color = tempColor;
            }
        }

        private void MakeTransparent(Renderer rend)
        {
            if (rend)
            {
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                var tempColor = rend.material.color;
                tempColor.a = transparencyAmount;
                rend.material.color = tempColor;
            }
        }
    }
}
