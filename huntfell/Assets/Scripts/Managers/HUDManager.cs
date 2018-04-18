using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class HUDManager : MonoBehaviour {

        [HideInInspector]
        public static HUDManager instance;
        public Canvas hudCanvas;

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

            if(hudCanvas == null)
            {
                Debug.LogWarning("There is no HUD Cavnas set in the HUD Manager inspector. Please make sure to set this field. Defaulting to finding any Canvas.");
                hudCanvas = FindObjectOfType<Canvas>();
            }
            if(promptText != null && promptIcon != null)
            {
                promptCanvasGroup = promptText.transform.parent.GetComponent<CanvasGroup>();
            }
            if(tutorialText != null && tutorialIcon != null)
            {
                tutorialCanvasGroup = tutorialText.transform.parent.GetComponent<CanvasGroup>();
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
            promptFadeCR = FadeInAndOut(promptCanvasGroup, 2, 3);
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
            promptFadeCR = FadeInAndOut(promptCanvasGroup, 2, 3);
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
            tutorialFadeCR = FadeInAndOut(tutorialCanvasGroup, 2, 3);
            StartCoroutine(tutorialFadeCR);
        }

        private IEnumerator FadeInAndOut(CanvasGroup canvasGroup, float fadeDuration, float stayDuration)
        {
            canvasGroup.gameObject.SetActive(true);
            yield return FadeCanvasGroup(canvasGroup, fadeDuration, FadeType.Out);
            yield return new WaitForSeconds(stayDuration);
            yield return FadeCanvasGroup(canvasGroup, fadeDuration, FadeType.In);
            canvasGroup.gameObject.SetActive(false);
        }

        private IEnumerator FadeCanvasGroup (CanvasGroup canvasGroup, float fadeDuration, FadeType fadeType)
        {
            canvasGroup.alpha = (float)fadeType;
            if (fadeDuration == 0)
            {
                canvasGroup.alpha = Mathf.Abs(canvasGroup.alpha - 1);
            }
            else
            {
                float curvePos = 0;
                while (curvePos < 1)
                {
                    curvePos += (Time.deltaTime / fadeDuration);
                    if (fadeType == FadeType.In)
                    {
                        canvasGroup.alpha = fadeCurve.Evaluate(1 - curvePos);
                    }
                    else
                    {
                        canvasGroup.alpha = fadeCurve.Evaluate(curvePos);
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
