using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hunter
{
    public class TitleManager : MonoBehaviour
    {
        private DeviceManager myDeviceManager;
        public CanvasGroup titleCanvasGroup;
        public AnimationCurve fadeCurve;

        private bool acceptingInput = true;

        private void Start ()
        {
            myDeviceManager = DeviceManager.Instance;
            myDeviceManager.gameInputEnabled = false;
            myDeviceManager.uiInputEnabled = true;
        }

        private void Update ()
        {
            if (acceptingInput & (myDeviceManager.PressedConfirm || myDeviceManager.PressedCancel))
            {
                acceptingInput = false;
                StartCoroutine(StartGame());
            }
        }

        private IEnumerator StartGame ()
        {
            Fabric.EventManager.Instance.PostEvent("UI Start Game");
            yield return FadeCanvasGroup(titleCanvasGroup, 2, FadeType.Out);
            GameManager.instance.StartCoroutine(GameManager.instance.StartGame());
            yield return null;
            SceneManager.UnloadSceneAsync("UI_Title_Menu");
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
                    if (fadeType == FadeType.Out)
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
