using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hunter {

    [RequireComponent(typeof(Animator), typeof(DeviceManager))]
    public class Credits : MonoBehaviour {

        public CanvasGroup closeCanvasGroup;
        public AnimationCurve fadeCurve;
        public float playbackSpeed = 1;

        private bool inputEnabled = false;
        private Animator anim;
        private DeviceManager deviceManager;

        private void Awake ()
        {
            anim = GetComponent<Animator>();
            deviceManager = GetComponent<DeviceManager>();
            anim.SetFloat("playbackSpeed", playbackSpeed);

            var fadeCanvas = GameObject.Find(GameManager.CANVASNAME);
            if(fadeCanvas != null) { Destroy(fadeCanvas); }
        }

        private void Update ()
        {
            if (inputEnabled && (deviceManager.PressedConfirm || deviceManager.PressedCancel))
            {
                inputEnabled = false;
                SceneManager.LoadScene(0);
            }
        }

        public void EndOfCreditsAnimationEvent ()
        {
            StartCoroutine(EndOfCreditsAction());
        }

        private IEnumerator EndOfCreditsAction ()
        {
            yield return new WaitForSeconds(0.5f);
            yield return Utility.FadeCanvasGroup(closeCanvasGroup, fadeCurve, 0.5f, FadeType.Out);
            inputEnabled = true;
        }
    }
}
