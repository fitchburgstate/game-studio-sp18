using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using Hunter.Characters;

namespace Hunter
{
    public enum FadeType
    {
        Out,
        In
    }
    public class GameManager : MonoBehaviour
    {

        [HideInInspector]
        public static GameManager instance;

        [Header("Scene Change Settings")]
        public float fadeDuration;
        public AnimationCurve fadeCurve;
        private CanvasGroup canvasGroup;
        private const string CANVASNAME = "FADECANVAS";
        private const string IMAGENAME = "FADEIMAGE";
        private PlayableDirector director;

        private void Awake ()
        {

            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (!Application.isEditor)
            {
                LoadNewScene("UI_Title_Menu", true);
                LoadNewScene("Audio", true);
            }

            //SceneManager.activeSceneChanged += CheckMainSceneLoaded;
            director = FindObjectOfType<PlayableDirector>();
        }

        public IEnumerator StartGame ()
        {
            if(director == null) {
                Debug.LogError("Could not start the game because there is no playable director to move the cameras.");
                yield break;
            }

            director.Play();
            yield return new WaitForSeconds((float)director.duration);

            SceneManager.LoadScene("UI_Hud", LoadSceneMode.Additive);
            SceneManager.LoadScene("UI_Pause_Menu", LoadSceneMode.Additive);
            DeviceManager.Instance.gameInputEnabled = true;

            var player = FindObjectOfType<Player>();
            player.transform.forward = Camera.main.transform.forward;

            Fabric.EventManager.Instance.PostEvent("Expo to Combat Music");
        }

        //private void CheckMainSceneLoaded (Scene oldScene, Scene newScene)
        //{
        //    if (newScene.buildIndex == 1) {
        //        if (Fabric.EventManager.Instance != null)
        //        {
        //        }
        //    }
        //    else if(newScene.buildIndex == 0)
        //    {
        //        if (Fabric.EventManager.Instance != null)
        //        {
        //        }
        //    }
        //}

        public void LoadNewScene (string sceneName, bool loadAdditively)
        {
            if (loadAdditively)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                StartCoroutine(ChangeActiveScene(sceneName));
            }
        }

        private IEnumerator ChangeActiveScene (string sceneName)
        {
            yield return InitiateFade(fadeDuration, Color.black, FadeType.Out);
            SceneManager.LoadScene(sceneName);
            yield return InitiateFade(fadeDuration, Color.black, FadeType.In);
            yield return null;
        }

        private IEnumerator InitiateFade (float fadeDuration, Color fadeColor, FadeType fadeType)
        {
            GameObject canvas = GameObject.Find(CANVASNAME);
            GameObject fadeImage = null;
            if (canvas != null)
            {
                fadeImage = canvas.transform.Find(IMAGENAME).gameObject;
            }
            else
            {
                canvas = new GameObject(CANVASNAME, typeof(Canvas), typeof(CanvasScaler), typeof(CanvasGroup));
                fadeImage = new GameObject(IMAGENAME, typeof(RawImage));
                fadeImage.transform.SetParent(canvas.transform);
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 10000;
                RectTransform rectComponent = fadeImage.GetComponent<RectTransform>();
                rectComponent.anchorMax = Vector2.one;
                rectComponent.anchorMin = Vector2.zero;
                rectComponent.anchoredPosition = Vector2.zero;
            }
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            RawImage imageComponent = fadeImage.GetComponent<RawImage>();
            imageComponent.color = fadeColor;
            DontDestroyOnLoad(canvas);

            yield return StartCoroutine(FadeCanvasGroup(fadeDuration, fadeType));
        }

        private IEnumerator FadeCanvasGroup (float fadeDuration, FadeType fadeType)
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
            if (fadeType == FadeType.In)
            {
                Destroy(canvasGroup.gameObject);
            }
        }
    }
}
