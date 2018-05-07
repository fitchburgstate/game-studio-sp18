using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters
{
    public class ObjectDepthOverlay : MonoBehaviour
    {
        #region Variables
        public bool showDebugLine = false;

        [Space]
        public Shader shaderToOverlay;
        public LayerMask cullingMask;

        private List<Material> objectMaterials = new List<Material>();
        private List<Shader> cachedShaders = new List<Shader>();
        private Character characterInstance;

        private SkinnedMeshRenderer skinnedMeshToCheck;
        private MeshRenderer meshToCheck;
        #endregion

        #region Properties
        public Character CharacterInstance
        {
            get
            {
                if (characterInstance == null) { characterInstance = GetComponent<Character>(); }
                return characterInstance;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            var tempSkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            var tempMeshRenderers = GetComponentsInChildren<MeshRenderer>();

            if (tempSkinnedMeshRenderers.Length > 0)
            {
                for (var i = 0; i < tempSkinnedMeshRenderers.Length; i++)
                {
                    foreach (var mat in tempSkinnedMeshRenderers[i].materials)
                    {
                        objectMaterials.Add(mat);
                    }
                }
            }

            if (tempMeshRenderers.Length > 0)
            {
                for (var i = 0; i < tempMeshRenderers.Length; i++)
                {
                    foreach (var mat in tempMeshRenderers[i].materials)
                    {
                        objectMaterials.Add(mat);
                    }
                }
            }

            if (objectMaterials.Count > 0)
            {
                for (var i = 0; i < objectMaterials.Count; i++)
                {
                    cachedShaders.Add(objectMaterials[i].shader);
                }
            }

            if (tempSkinnedMeshRenderers.Length > 0)
            {
                skinnedMeshToCheck = tempSkinnedMeshRenderers[0];
            }
            else if (tempMeshRenderers.Length > 0)
            {
                meshToCheck = tempMeshRenderers[0];
            }
        }

        private void FixedUpdate()
        {
            if ((skinnedMeshToCheck != null && skinnedMeshToCheck.isVisible) || (meshToCheck != null && meshToCheck.isVisible))
            {
                var objectPosition = new Vector3();

                if (CharacterInstance != null)
                {
                    objectPosition = CharacterInstance.EyeLineTransform.position;
                }
                else
                {
                    objectPosition = transform.position;
                }

                if (showDebugLine) { Debug.DrawLine(objectPosition, Camera.main.transform.position, Color.blue, 3f); }
                if (Physics.Linecast(objectPosition, Camera.main.transform.position, cullingMask))
                {
                    for (var i = 0; i < objectMaterials.Count; i++)
                    {
                        if (objectMaterials[i].shader != shaderToOverlay)
                        {
                            objectMaterials[i].shader = shaderToOverlay;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < objectMaterials.Count; i++)
                    {
                        if (objectMaterials[i].shader != cachedShaders[i])
                        {
                            objectMaterials[i].shader = cachedShaders[i];
                        }
                    }
                }
            }
        }
        #endregion
    }
}
