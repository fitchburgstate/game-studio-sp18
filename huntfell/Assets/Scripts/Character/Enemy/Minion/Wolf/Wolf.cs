using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

namespace Hunter.Character
{

    public sealed class Wolf : Minion, IMoveable, IUtilityBasedAI
    {
//#if UNITY_EDITOR
//        //public class WolfDrawer : PropertyDrawer
//        //{
//        //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        //    {
//        //        EditorGUILayout.Space();
//        //        EditorGUILayout.LabelField(property.FindPropertyRelative("playerName") + " Specific Variables", EditorStyles.boldLabel);
//        //        EditorGUILayout.BeginVertical("Box");
//        //        EditorGUILayout.PropertyField(property.FindPropertyRelative("walkSpeed"));
//        //        EditorGUILayout.PropertyField(property.FindPropertyRelative("runSpeed"));
//        //        EditorGUILayout.PropertyField(property.FindPropertyRelative("biteValue"));
//        //        EditorGUILayout.EndVertical();
//        //    }
//        //}

//        //public override PropertyDrawer CustomInspectorPropertyDrawer
//        //{
//        //    get
//        //    {
//        //        return new WolfDrawer();
//        //    }

//        //}
//        public override List<string> SerialzedPropertyNamesList
//        {
//            get
//            {
//                return new List<string>
//                {
//                    "displayName",
//                    "walkSpeed",
//                    "runSpeed",
//                    "biteValue"
//                };
//            }
//        }
//#endif
        /// <summary>
        /// This is the wolf's animator controller.
        /// </summary>
        public Animator anim;

        public float biteValue = 5f;

        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent)
        {
            // This function is not being used.
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent, Transform target)
        {
            //anim.SetFloat("dirY", Mathf.Abs(moveDirection.magnitude), 0, 1);

            var finalTarget = new Vector3(target.transform.position.x, characterModel.transform.localPosition.y, target.transform.position.z);
            agent.speed = runSpeed;
            agent.destination = finalTarget;
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented.
        }

        public void Idle(CharacterController controller, NavMeshAgent agent)
        {
            // This feature has not yet been implemented.
        }

        public void Wander(CharacterController controller, Vector3 target, NavMeshAgent agent)
        {
            agent.speed = walkSpeed;
            agent.destination = target;
        }
    }
}
