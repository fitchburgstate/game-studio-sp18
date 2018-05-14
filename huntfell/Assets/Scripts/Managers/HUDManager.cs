using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Hunter.Characters;

namespace Hunter
{
    public class HUDManager : MonoBehaviour
    {

        [Serializable]
        public struct DecanterInfo
        {
            public GameObject parent;
            public Image fillImage;
            public TextMeshProUGUI shardsText;
            public Image inputImage;
        }

        [HideInInspector]
        public static HUDManager instance;
        public CanvasGroup hudCanvasGroup;

        [Header("Health and Stamina")]
        public Image playerHealthBar;
        public Image playerWoundBar;
        public Image playerStaminaBar;
        public List<DecanterInfo> decanters;
        [Space]
        public Image bossHealthBar;
        public Image bossWoundBar;

        [Header("Weapons and Elements")]
        [SerializeField]
        private List<Image> weaponSockets;
        [SerializeField]
        private Animator weaponWheelController;
        [SerializeField]
        private List<Image> elementSockets;
        [SerializeField]
        private Animator elementWheelController;
        [SerializeField]
        private float wheelSpeed = 2;
        [SerializeField]
        private Sprite nullElementSprite;

        [Header("Prompts")]
        [SerializeField]
        private TextMeshProUGUI promptText;
        [SerializeField]
        private Image promptIcon;
        [SerializeField]
        private TextMeshProUGUI tutorialText;
        [SerializeField]
        private Image tutorialIcon;
        [SerializeField]
        private Sprite hintSprite;

        [Space]
        public AnimationCurve fadeCurve;

        private CanvasGroup promptCanvasGroup;
        private CanvasGroup tutorialCanvasGroup;
        private CanvasGroup bossHealthCanvasGroup;

        //private GameObject journalParent;

        private IEnumerator promptFadeAction;
        private IEnumerator tutorialFadeAction;

        private IEnumerator staminaBarAction;

        private IEnumerator elementWheelAction;
        private int currentElementWheelIndex = 0;
        private int targetElementWheelIndex = 0;

        private IEnumerator weaponWheelAction;
        private int currentWeaponWheelIndex = 0;
        private int targetWeaponWheelIndex = 0;

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

            if (promptText != null && promptIcon != null)
            {
                promptCanvasGroup = promptText.transform.parent.GetComponent<CanvasGroup>();
            }
            if (tutorialText != null && tutorialIcon != null)
            {
                tutorialCanvasGroup = tutorialText.transform.parent.GetComponent<CanvasGroup>();
            }
            if (hudCanvasGroup != null)
            {
                hudCanvasGroup.alpha = 0;
                StartCoroutine(Utility.FadeCanvasGroup(hudCanvasGroup, fadeCurve, 1, FadeType.Out));
            }
            if (elementSockets != null && elementSockets.Count > 0)
            {
                foreach (var socket in elementSockets)
                {
                    socket.enabled = false;
                }
            }
            if (weaponSockets != null && weaponSockets.Count > 0)
            {
                foreach (var socket in weaponSockets)
                {
                    socket.enabled = false;
                }
            }
            if (bossHealthBar != null)
            {
                bossHealthCanvasGroup = bossHealthBar.transform.parent.GetComponent<CanvasGroup>();
                bossHealthCanvasGroup.alpha = 0;
            }

            FindObjectOfType<Player>()?.InitPlayerUI();
        }

        #region Weapon Wheel
        public void AddNewElementToSocket (Sprite elementIcon)
        {
            if (elementSockets != null && elementSockets.Count > 0)
            {
                foreach (var socket in elementSockets)
                {
                    if (!socket.enabled)
                    {
                        socket.sprite = elementIcon;
                        socket.enabled = true;
                        break;
                    }
                }
            }
        }

        public void AddNewWeaponToSocket (Sprite weaponIcon)
        {
            if (weaponSockets != null && weaponSockets.Count > 0)
            {
                foreach (var socket in weaponSockets)
                {
                    if (!socket.enabled)
                    {
                        socket.sprite = weaponIcon;
                        socket.enabled = true;
                        break;
                    }
                }
            }
        }

        public void DimElementSockets (List<int> indexList)
        {
            //Start at one to not include null element since all weapons can equip it
            for (var i = 1; i < elementSockets.Count; i++)
            {
                if (indexList.Contains(i))
                {
                    elementSockets[i].color = Color.gray;
                }
                else
                {
                    elementSockets[i].color = Color.white;
                }
            }
        }

        public void MoveElementWheel (int elementIndex)
        {
            targetElementWheelIndex = elementIndex;
            if (elementWheelAction != null) { return; }

            elementWheelAction = ElementWheelAnimation();
            StartCoroutine(elementWheelAction);
        }

        private IEnumerator ElementWheelAnimation ()
        {
            DisableElementSocketHighlight(currentElementWheelIndex);

            //This gets the total amount of element slots with respect to index notation and then converts it to a 'divide by zero' safe fraction
            var eMod = 1.0f / (elementSockets.Count - 1.0f);

            // Converts the index values to blend tree percentage values
            var targetWheelAmount = targetElementWheelIndex * eMod;
            var currentWheelAmount = currentElementWheelIndex * eMod;

            while (targetWheelAmount != currentWheelAmount)
            {
                var step = Time.deltaTime * wheelSpeed;
                targetWheelAmount = targetElementWheelIndex * eMod;

                if (currentWheelAmount < targetWheelAmount)
                {
                    currentWheelAmount = Mathf.Clamp(currentWheelAmount + step, 0, targetWheelAmount);
                }
                else if (currentWheelAmount > targetWheelAmount)
                {
                    currentWheelAmount = Mathf.Clamp(currentWheelAmount - step, targetWheelAmount, 1);
                }

                elementWheelController.SetFloat("wheelAmount", currentWheelAmount);
                yield return null;
            }

            currentElementWheelIndex = targetElementWheelIndex;
            EnableElementSocketHighlight(currentElementWheelIndex);
            elementWheelAction = null;
        }

        public void MoveWeaponWheel (int weaponIndex)
        {
            targetWeaponWheelIndex = weaponIndex;
            if (weaponWheelAction != null) { return; }

            weaponWheelAction = WeaponWheelAnimation(weaponIndex);
            StartCoroutine(weaponWheelAction);
        }

        private IEnumerator WeaponWheelAnimation (int elementIndex)
        {

            //This gets the total amount of weapon slots with respect to index notation and then converts it to a 'divide by zero' safe fraction
            var wMod = 1.0f / (weaponSockets.Count - 1.0f);

            // Converts the index values to blend tree percentage values
            var targetWheelAmount = targetWeaponWheelIndex * wMod;
            var currentWheelAmount = currentWeaponWheelIndex * wMod;

            while (targetWheelAmount != currentWheelAmount)
            {
                var step = Time.deltaTime * wheelSpeed;
                targetWheelAmount = targetWeaponWheelIndex * wMod;

                if (currentWheelAmount < targetWheelAmount)
                {
                    currentWheelAmount = Mathf.Clamp(currentWheelAmount + step, 0, targetWheelAmount);
                }
                else if (currentWheelAmount > targetWheelAmount)
                {
                    currentWheelAmount = Mathf.Clamp(currentWheelAmount - step, targetWheelAmount, 1);
                }

                weaponWheelController.SetFloat("wheelAmount", currentWheelAmount);
                yield return null;
            }

            currentWeaponWheelIndex = elementIndex;
            weaponWheelAction = null;
        }

        public void EnableElementSocketHighlight (int index)
        {
            elementSockets[index].transform.GetChild(0).gameObject.SetActive(true);
        }

        public void DisableElementSocketHighlight (int index)
        {
            elementSockets[index].transform.GetChild(0).gameObject.SetActive(false);
        }
        #endregion

        #region Prompts
        public void ShowItemPickupPrompt (string itemName, Sprite itemIcon)
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

            if (promptFadeAction != null)
            {
                StopCoroutine(promptFadeAction);
            }
            promptFadeAction = FadePromptInAndOut(promptCanvasGroup, 2, 3);
            StartCoroutine(promptFadeAction);
        }

        public void ShowTutorialPrompt (string text, Sprite controlIcon)
        {
            if (tutorialText == null || tutorialIcon == null)
            {
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

        private IEnumerator FadePromptInAndOut (CanvasGroup canvasGroup, float fadeDuration, float stayDuration)
        {
            canvasGroup.gameObject.SetActive(true);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.Out);
            yield return new WaitForSeconds(stayDuration);
            yield return Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, FadeType.In);
            canvasGroup.gameObject.SetActive(false);
        }
        #endregion

        #region Player Health
        /// <summary>
        /// Stops any other health bar actions and instantly moves the health bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetPlayerTargetHealthBar (float targetFill)
        {
            playerHealthBar.fillAmount = targetFill;
        }

        /// <summary>
        /// Stops any other wound bar actions and instantly moves the wound bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetPlayerCurrentHealthBar (float targetFill)
        {
            playerWoundBar.fillAmount = targetFill;
        }

        public void EnableDecanter (int decanterIndex)
        {
            if (decanterIndex >= decanters.Count)
            {
                Debug.LogWarning($"You tried to enable a decanter at index {decanterIndex} but there are only {decanters.Count} assigned in the inspector.");
                return;
            }
            decanters[decanterIndex].parent.SetActive(true);
        }

        public void SetDecanterInfo (int decanterIndex, int currentShards, int maxShards)
        {
            if (decanterIndex >= decanters.Count)
            {
                Debug.LogWarning($"You tried to edit a decanter at index {decanterIndex} but there are only {decanters.Count} assigned in the inspector.");
                return;
            }
            var decanter = decanters[decanterIndex];
            var adjustedCurrentShards = Mathf.Clamp(currentShards - (maxShards * decanterIndex), 0, maxShards);
            var targetFill = (float)adjustedCurrentShards / maxShards;
            decanter.fillImage.fillAmount = targetFill;
            decanter.shardsText.text = $"{adjustedCurrentShards}/{maxShards}";

            if (targetFill == 1)
            {
                decanter.shardsText.gameObject.SetActive(false);
                decanter.inputImage.gameObject.SetActive(true);
            }
            else
            {
                decanter.inputImage.gameObject.SetActive(false);
                decanter.shardsText.gameObject.SetActive(true);
            }
        }
        #endregion

        #region Boss Health
        public void FadeBossHealth (FadeType fadeType)
        {
            StartCoroutine(Utility.FadeCanvasGroup(bossHealthCanvasGroup, fadeCurve, 1, fadeType));
        }

        /// <summary>
        /// Stops any other health bar actions and instantly moves the health bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetBossTargetHealthBar (float targetFill)
        {
            bossHealthBar.fillAmount = targetFill;
        }

        /// <summary>
        /// Stops any other wound bar actions and instantly moves the wound bar to the targetFill
        /// </summary>
        /// <param name="targetFill"></param>
        public void SetBossCurrentHealthBar (float targetFill)
        {
            bossWoundBar.fillAmount = targetFill;
        }
        #endregion

        #region Player Stamina
        /// <summary>
        /// Smoothly moves the yellow stamina bar up or down based on a totalTime value for how long the fill should take
        /// </summary>
        /// <param name="targetFill"></param>
        /// <param name="totalTime"></param>
        public void SetStaminaBar (float targetFill, float totalTime)
        {
            if (staminaBarAction != null)
            {
                StopCoroutine(staminaBarAction);
            }
            staminaBarAction = PlayerSmoothStaminaBarFill(targetFill, totalTime);
            StartCoroutine(staminaBarAction);
        }

        private IEnumerator PlayerSmoothStaminaBarFill (float targetFill, float totalTime)
        {
            if (totalTime <= 0)
            {
                playerStaminaBar.fillAmount = targetFill;
            }
            else
            {
                var startFill = playerStaminaBar.fillAmount;
                var startTime = Time.time;
                var percentComplete = 0f;
                while (percentComplete < 1)
                {
                    var elapsedTime = Time.time - startTime;
                    percentComplete = Mathf.Clamp01(elapsedTime / totalTime);
                    playerStaminaBar.fillAmount = Mathf.Lerp(startFill, targetFill, percentComplete);
                    yield return null;
                }
            }

            staminaBarAction = null;
        }
        #endregion
    }
}
