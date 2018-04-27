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

        private IEnumerator promptFadeAction;
        private IEnumerator tutorialFadeAction;

        private IEnumerator staminaBarAction;

        private IEnumerator healthBarAction;
        private float targetHealthBarFill;

        private IEnumerator woundBarAction;
        private float targetWoundBarFill;


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

            if (promptFadeAction != null)
            {
                StopCoroutine(promptFadeAction);
            }
            promptFadeAction = FadePromptInAndOut(promptCanvasGroup, 2, 3);
            StartCoroutine(promptFadeAction);
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

            if(promptFadeAction != null)
            {
                StopCoroutine(promptFadeAction);
            }
            promptFadeAction = FadePromptInAndOut(promptCanvasGroup, 2, 3);
            StartCoroutine(promptFadeAction);
        }

        public void ShowTutorialPrompt (string text, Sprite controlIcon)
        {
            if(tutorialText == null || tutorialIcon == null) {
                Debug.LogWarning("Could not show the tutorial prompt because the elements haven't been set in the inspector.", gameObject);
                return;
            }

            tutorialText.text = text;
            tutorialIcon.sprite = controlIcon;

            if (tutorialFadeAction != null)
            {
                StopCoroutine(tutorialFadeAction);
            }
            tutorialFadeAction = FadePromptInAndOut(tutorialCanvasGroup, 2, 3);
            StartCoroutine(tutorialFadeAction);
        }

        private IEnumerator FadePromptInAndOut(CanvasGroup canvasGroup, float fadeDuration, float stayDuration)
        {
            canvasGroup.gameObject.SetActive(true);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.Out);
            yield return new WaitForSeconds(stayDuration);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.In);
            canvasGroup.gameObject.SetActive(false);
        }

        /// <summary>
        /// Smoothly moves the red health bar up or down based on a speed value for consistency with different amounts of damage being dealt
        /// </summary>
        /// <param name="targetFill"></param>
        /// <param name="fillSpeed"></param>
        public void SetHealthBar(float targetFill, float fillSpeed)
        {
            targetHealthBarFill = targetFill;
            if(healthBarAction != null)
            {
                return;
            }
            healthBarAction = SmoothHealthBarFill(fillSpeed);
            StartCoroutine(healthBarAction);
        }

        /// <summary>
        /// Stops any other health bar actions and instantly moves the health bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetHealthBar (float targetFill)
        {
            if (healthBarAction != null)
            {
                StopCoroutine(healthBarAction);
                healthBarAction = null;
            }
            targetHealthBarFill = targetFill;
            healthBar.fillAmount = targetHealthBarFill;
        }

        /// <summary>
        /// Smoothly moves the orange health bar up or down based on a speed value for consistency with different amounts of damage being dealt
        /// </summary>
        /// <param name="targetFill"></param>
        /// <param name="fillSpeed"></param>
        public void SetWoundBar(float targetFill, float fillSpeed)
        {
            targetWoundBarFill = targetFill;
            if(woundBarAction != null)
            {
                return;
            }
            //Want to make sure everytime we restart the woundbar that it is exactly where the health bar was before it got lowered to represent the target health, since wound bar represents actual health
            woundBar.fillAmount = healthBar.fillAmount;
            woundBarAction = SmoothWoundBarFill(fillSpeed);
            StartCoroutine(woundBarAction);
        }

        /// <summary>
        /// Stops any other wound bar actions and instantly moves the wound bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetWoundBar (float targetFill)
        {
            if (woundBarAction != null)
            {
                StopCoroutine(woundBarAction);
                woundBarAction = null;
            }
            targetWoundBarFill = targetFill;
            woundBar.fillAmount = targetWoundBarFill;
        }

        /// <summary>
        /// Smoothly moves the yellow stamina bar up or down based on a totalTime value for how long the fill should take
        /// </summary>
        /// <param name="targetFill"></param>
        /// <param name="totalTime"></param>
        public void SetStaminaBar(float targetFill, float totalTime)
        {
            if(staminaBarAction != null)
            {
                StopCoroutine(staminaBarAction);
            }
            staminaBarAction = SmoothStaminaBarFill(targetFill, totalTime);
            StartCoroutine(staminaBarAction);
        }

        private IEnumerator SmoothHealthBarFill(float fillSpeed)
        {
            fillSpeed = Mathf.Abs(fillSpeed);

            while(healthBar.fillAmount != targetHealthBarFill)
            {
                var step = Time.deltaTime * fillSpeed;
                if (healthBar.fillAmount < targetHealthBarFill)
                {
                    healthBar.fillAmount = Mathf.Clamp(healthBar.fillAmount + step, healthBar.fillAmount, targetHealthBarFill);
                }
                else
                {
                    healthBar.fillAmount = Mathf.Clamp(healthBar.fillAmount - step, targetHealthBarFill, healthBar.fillAmount);
                }
                yield return null;
            }

            healthBarAction = null;
        }

        private IEnumerator SmoothWoundBarFill (float fillSpeed)
        {
            fillSpeed = Mathf.Abs(fillSpeed);

            while(woundBar.fillAmount != targetWoundBarFill && woundBar.fillAmount > healthBar.fillAmount)
            {
                var step = Time.deltaTime * fillSpeed;
                if(woundBar.fillAmount < targetWoundBarFill)
                {
                    woundBar.fillAmount = Mathf.Clamp(woundBar.fillAmount + step, woundBar.fillAmount, targetWoundBarFill);
                }
                else
                {
                    woundBar.fillAmount = Mathf.Clamp(woundBar.fillAmount - step, targetWoundBarFill, woundBar.fillAmount);
                }
                yield return null;
            }

            woundBarAction = null;
        }

        private IEnumerator SmoothStaminaBarFill(float targetFill, float totalTime)
        {
            if (totalTime <= 0)
            {
                staminaBar.fillAmount = targetFill;
            }
            else
            {
                var startFill = staminaBar.fillAmount;
                var startTime = Time.time;
                var percentComplete = 0f;
                while (percentComplete < 1)
                {
                    var elapsedTime = Time.time - startTime;
                    percentComplete = Mathf.Clamp01(elapsedTime / totalTime);
                    staminaBar.fillAmount = Mathf.Lerp(startFill, targetFill, percentComplete);
                    yield return null;
                }
            }

            staminaBarAction = null;
        }
    }
}
