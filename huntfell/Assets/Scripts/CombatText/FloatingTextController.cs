using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Elements;

namespace Hunter.Character
{
    public class FloatingTextController : MonoBehaviour
    {
        private static FloatingText popUpText;
        private static GameObject popupCanvas;

        void Start()
        {
            Initialize();
        }

        public static void Initialize()
        {
            popupCanvas = GameObject.Find("CombatTextUI");
            if(!popUpText)
            {
                popUpText = Resources.Load<FloatingText>("Prefabs/PopupDamageParent");
            }
        }

        public static void CreateFloatingText(string text, Transform location, Element type)
        {
            var popupInstance = Instantiate(popUpText);
            var worldPos = Camera.main.WorldToScreenPoint(location.position);
            popupInstance.transform.SetParent(popupCanvas.transform, false);
            popupInstance.transform.position = worldPos;
            popupInstance.SetTextColor(type);
            popupInstance.SetDamageText(text);
        }
    }
}
