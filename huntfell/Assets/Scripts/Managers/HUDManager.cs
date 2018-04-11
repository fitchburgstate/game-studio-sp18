using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class HUDManager : MonoBehaviour {

        [HideInInspector]
        public static HUDManager instance;
        [SerializeField]
        private Canvas hudCanvas;
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
        private TextMeshProUGUI pickupText;
        [SerializeField]
        private Image pickupIcon;
        [SerializeField]
        private TextMeshProUGUI journalText;
        public AnimationCurve fadeCurve;
        [SerializeField]
        private Sprite nullElementSprite;

        public GameObject damagePopUpPrefab;
        private GameObject promptParent;
        //private GameObject journalParent;

        private IEnumerator promptFadeCR;
        private IEnumerator journalFadeCR;

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
            if(pickupText != null)
            {
                promptParent = pickupText.transform.parent.gameObject;
            }
            if(journalText != null)
            {
                //journalParent = journalText.transform.parent.gameObject;
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
            pickupText.text = "You have picked up " + itemName;
            pickupIcon.sprite = itemIcon;
            pickupIcon.enabled = true;
            var cg = promptParent.GetComponent<CanvasGroup>();
            if (promptFadeCR != null)
            {
                StopCoroutine(promptFadeCR);
            }
            promptFadeCR = FadeInAndOut(cg, 2, 3);
            StartCoroutine(promptFadeCR);
        }

        public void ShowPrompt (string text)
        {
            pickupText.text = text;
            pickupIcon.enabled = false;
            var cg = promptParent.GetComponent<CanvasGroup>();
            if(promptFadeCR != null)
            {
                StopCoroutine(promptFadeCR);
            }
            promptFadeCR = FadeInAndOut(cg, 2, 3);
            StartCoroutine(promptFadeCR);
        }

        public void ShowJournalPickup(string bookText)
        {
            //journalText.text = bookText;
            //var cg = journalParent.GetComponent<CanvasGroup>();
            //if (journalFadeCR != null)
            //{
            //    StopCoroutine(journalFadeCR);
            //}
            //journalFadeCR = FadeInAndOut(cg, 2, 3);
            //StartCoroutine(journalFadeCR);
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
