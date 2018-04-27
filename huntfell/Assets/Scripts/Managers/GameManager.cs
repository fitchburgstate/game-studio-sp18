using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using Hunter.Characters;
using Cinemachine;

namespace Hunter
{
    public enum FadeType
    {
        Out,
        In
    }

    [RequireComponent(typeof(DeviceManager))]
    public class GameManager : MonoBehaviour
    {
        #region Variables
        [HideInInspector]
        public static GameManager instance;
        public DeviceManager DeviceManager { get; private set; }

        [Header("Player Respawn Settings")]
        /// <summary>
        /// The list of spawnpoints that the manager can see.
        /// </summary>
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        [Header("Scene Change Settings")]
        public float fadeDuration;
        public AnimationCurve fadeCurve;

        [Header("Game Start Settings")]
        public bool loadTitleScreen = false;

        public const string CANVASNAME = "FADECANVAS";
        public const string IMAGENAME = "FADEIMAGE";
        private PlayableDirector director;
        public CinemachineVirtualCamera isometricFollowCM;
        public CinemachineVirtualCamera titleScreenCM;
        #endregion

        #region Unity Functions
        private void Awake ()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (!Application.isEditor || loadTitleScreen)
            {
                LoadNewScene("UI_Title_Menu", true);
                titleScreenCM.Priority = 2;
                if (GameObject.Find("Audio") == null)
                {
                    LoadNewScene("Audio", true);
                }
            }

            DeviceManager = GetComponent<DeviceManager>();
            director = GetComponentInChildren<PlayableDirector>();
            spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
        }

        #endregion

        #region Scene Management
        
        public IEnumerator StartGame (CanvasGroup titleScreenCanvasGroup)
        {
            Fabric.EventManager.Instance?.PostEvent("UI Start Game");
            yield return Utility.FadeCanvasGroup(titleScreenCanvasGroup, fadeCurve, 2, FadeType.In);

            SceneManager.UnloadSceneAsync("UI_Title_Menu");
            if (director == null)
            {
                Debug.LogWarning("Could not play the opening cutscene because there is no playable director to move the cameras.");
                titleScreenCM.gameObject.SetActive(false);
            }
            else
            {
                director.Play();
                yield return new WaitForSeconds((float)director.duration);
            }

            SceneManager.LoadScene("UI_Hud", LoadSceneMode.Additive);
            SceneManager.LoadScene("UI_Pause_Menu", LoadSceneMode.Additive);
            DeviceManager.gameInputEnabled = true;

            Fabric.EventManager.Instance.PostEvent("Expo to Combat Music");
        }

        public void LoadNewScene(string sceneName, bool loadAdditively)
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

        private IEnumerator ChangeActiveScene(string sceneName)
        {
            yield return FadeScreen(fadeDuration, Color.black, FadeType.Out);
            SceneManager.LoadScene(sceneName);
            yield return FadeScreen(fadeDuration, Color.black, FadeType.In);
        }

        #endregion

        #region Helper Functions

        public IEnumerator FadeScreen (Color fadeColor, FadeType fadeType)
        {
            yield return FadeScreen(fadeDuration, fadeColor, fadeType);
        }

        public IEnumerator FadeScreen(float fadeDuration, Color fadeColor, FadeType fadeType)
        {
            var canvas = GameObject.Find(CANVASNAME);
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
                var canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 10000;
                var rectComponent = fadeImage.GetComponent<RectTransform>();
                rectComponent.anchorMax = Vector2.one;
                rectComponent.anchorMin = Vector2.zero;
                rectComponent.anchoredPosition = Vector2.zero;
            }
            var canvasGroup = canvas.GetComponent<CanvasGroup>();
            var imageComponent = fadeImage.GetComponent<RawImage>();
            imageComponent.color = fadeColor;
            DontDestroyOnLoad(canvas);

            yield return StartCoroutine(Utility.FadeCanvasGroup(canvasGroup, fadeCurve, fadeDuration, fadeType));
        }

        public SpawnPoint GetClosestSpawnPoint(Vector3 currentPosition)
        {
            SpawnPoint bestSpawnPoint = null;
            var closestDistanceSqr = Mathf.Infinity;

            foreach (var spawnPoint in spawnPoints)
            {
                if (!spawnPoint.activated) { continue; }

                var directionToTarget = spawnPoint.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestSpawnPoint = spawnPoint;
                }
            }

            return bestSpawnPoint;
        }
        #endregion
    }
}
