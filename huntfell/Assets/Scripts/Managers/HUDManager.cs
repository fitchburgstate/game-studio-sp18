using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class HUDManager : MonoBehaviour {

        [HideInInspector]
        public static HUDManager instance;
        public CanvasGroup hudCanvasGroup;

        [SerializeField]
        public Image healthBar;
        [SerializeField]
        public Image woundBar;
        [SerializeField]
        public Image staminaBar;
        [SerializeField]
        private Image activeWeapon;
        [SerializeField]
        private Image inactiveWeapon;
        [SerializeField]
        private Image activeElement;
        [SerializeField]
        private TextMeshProUGUI promptText;
        [SerializeField]
        private Image promptIcon;
        [SerializeField]
        private TextMeshProUGUI tutorialText;
        [SerializeField]
        private Image tutorialIcon;

        public AnimationCurve fadeCurve;
        [SerializeField]
        private Sprite nullElementSprite;
        [SerializeField]
        private Sprite hintSprite;

        public GameObject damagePopUpPrefab;
        private CanvasGroup promptCanvasGroup;
        private CanvasGroup tutorialCanvasGroup;

        //private GameObject journalParent;

        private IEnumerator promptFadeCR;
        private IEnumerator tutorialFadeCR;

        public void Awake ()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if(promptText != null && promptIcon != null)
            {
                promptCanvasGroup = promptText.transform.parent.GetComponent<CanvasGroup>();
            }
            if(tutorialText != null && tutorialIcon != null)
            {
                tutorialCanvasGroup = tutorialText.transform.parent.GetComponent<CanvasGroup>();
            }
            if(hudCanvasGroup != null)
            {
                hudCanvasGroup.alpha = 0;
                StartCoroutine(Utility.FadeCanvasGroup(hudCanvasGroup, fadeCurve, 1, FadeType.Out));
            }
        }

        public void UpdateWeaponImage(Sprite newSprite)
        {
            inactiveWeapon.sprite = activeWeapon.sprite;
            activeWeapon.sprite = newSprite;
        }

        public void UpdateElementImage (Sprite newSprite)
        {
            if(newSprite == null)
            {
                activeElement.sprite = nullElementSprite;
                return;
            }
            activeElement.sprite = newSprite;
        }

        public void ShowItemPickupPrompt(string itemName, Sprite itemIcon)
        {
            if (promptText == null || promptIcon == null)
            {
                Debug.LogWarning("Could not show the prompt because the elements haven't been set in the inspector.", gameObject);
                return;
            }

            promptText.text = "Obtained the " + itemName;
            promptIcon.sprite = itemIcon;

            if (promptFadeCR != null)
            {
                StopCoroutine(promptFadeCR);
            }
            promptFadeCR = FadePromptInAndOut(promptCanvasGroup, 2, 3);
            StartCoroutine(promptFadeCR);
        }

        public void ShowHintPrompt (string text)
        {
            if (promptText == null || promptIcon == null)
            {
                Debug.LogWarning("Could not show the prompt because the elements haven't been set in the inspector.", gameObject);
                return;
            }

            promptText.text = text;
            promptIcon.sprite = hintSprite;

            if(promptFadeCR != null)
            {
                StopCoroutine(promptFadeCR);
            }
            promptFadeCR = FadePromptInAndOut(promptCanvasGroup, 2, 3);
            StartCoroutine(promptFadeCR);
        }

        public void ShowTutorialPrompt (string text, Sprite controlIcon)
        {
            if(tutorialText == null || tutorialIcon == null) {
                Debug.LogWarning("Could not show the tutorial prompt because the elements haven't been set in the inspector.", gameObject);
                return;
            }

            tutorialText.text = text;
            tutorialIcon.sprite = controlIcon;

            if (tutorialFadeCR != null)
            {
                StopCoroutine(tutorialFadeCR);
            }
            tutorialFadeCR = FadePromptInAndOut(tutorialCanvasGroup, 2, 3);
            StartCoroutine(tutorialFadeCR);
        }

        private IEnumerator FadePromptInAndOut(CanvasGroup canvasGroup, float fadeDuration, float stayDuration)
        {
            canvasGroup.gameObject.SetActive(true);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.Out);
            yield return new WaitForSeconds(stayDuration);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.In);
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
