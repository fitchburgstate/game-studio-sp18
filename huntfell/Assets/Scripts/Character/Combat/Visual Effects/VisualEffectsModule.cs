using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class VisualEffectsModule : MonoBehaviour
    {
        [Header("World Space UI Options")]
        public bool displayWorldSpaceHealth = true;
        public Image worldSpaceHealthBar;
        public Image worldSpaceWoundBar;
        private GameObject healthBarParent;

        [Space]
        public GameObject damagePopUpRootPrefab;
        public float criticalFontSizeMultiplier = 1.5f;
        public bool boldOnCriticalHit = true;
        public bool italicsOnCriticalHit = true;
        private Canvas popUpCanvas;

        [Header("Hit Particle Options")]
        public ParticleSystem nullHitSystem;
        public ParticleSystem fireHitSystem;
        public ParticleSystem iceHitSystem;
        public ParticleSystem electricHitSystem;
        public ParticleSystem silverHitSystem;

        private void Awake ()
        {
            popUpCanvas = GetComponentInChildren<Canvas>();
            if (popUpCanvas == null)
            {
                Debug.LogWarning($"{transform.parent.name} does not have the Damage Pop-Up Canvas childed to it. No numbers will pop up when it takes damage.", gameObject);
            }

            if(worldSpaceHealthBar != null) { healthBarParent = worldSpaceHealthBar.transform.parent.gameObject; }
            else if(worldSpaceWoundBar != null) { healthBarParent = worldSpaceWoundBar.transform.parent.gameObject; }
            
            DisableHealthBars();
        }

        public void EnableHealthBars ()
        {
            healthBarParent?.SetActive(true);
        }

        public void DisableHealthBars ()
        {
            healthBarParent?.SetActive(false);
        }

        public void SetHealthBarFill(float fill)
        {
            worldSpaceHealthBar.fillAmount = fill;
        }

        public void SetWoundBarFill (float fill)
        {
            worldSpaceWoundBar.fillAmount = fill;
        }

        public void StartDamageEffects(int damage, bool isCritical, Element element, bool playHitEffect)
        {
            if(popUpCanvas != null) {
                var damageDisplay = damage.ToString();
                if (damage < 1) { damageDisplay = "Immune"; }

                var displayColor = Color.white;
                if(element != null) { displayColor = element.elementColor; }

                SpawnPopUpText(damageDisplay, isCritical, displayColor);
            }
            if (!playHitEffect) { return; }

            switch (Utility.ElementToElementOption(element))
            {
                case ElementOption.None:
                    nullHitSystem?.Play();
                    break;
                case ElementOption.Fire:
                    fireHitSystem?.Play();
                    break;
                case ElementOption.Ice:
                    iceHitSystem?.Play();
                    break;
                case ElementOption.Electric:
                    electricHitSystem?.Play();
                    break;
                case ElementOption.Silver:
                    silverHitSystem?.Play();
                    break;
                default:
                    Debug.LogWarning("Looks like you got hit with an element that doesnt have a hit effect yet. Defaulting to null.");
                    nullHitSystem?.Play();
                    break;
            }
        }

        public void StartHealEffects (int restore, bool isCritical)
        {
            if (popUpCanvas != null) {
                var restoreDisplay = "+" + restore.ToString();
                SpawnPopUpText(restoreDisplay, isCritical, Color.green);
            }
        }

        private void SpawnPopUpText (string textDisplay, bool isCritical, Color textColor)
        {
            //Create the text pop-up from a prefab
            var damageTextRoot = Instantiate(damagePopUpRootPrefab, popUpCanvas.transform);
            //Set the text to the proper color and damage amount
            var damageText = damageTextRoot.GetComponentInChildren<TextMeshProUGUI>();

            damageText.color = textColor; 
            if (isCritical)
            {
                damageText.fontSize = damageText.fontSize * criticalFontSizeMultiplier;
                if (boldOnCriticalHit) { textDisplay = $"<b>{textDisplay}</b>"; }
                if (italicsOnCriticalHit) { textDisplay = $"<i>{textDisplay}</i>"; }
            }
            damageText.SetText(textDisplay);
        }
    }
}
