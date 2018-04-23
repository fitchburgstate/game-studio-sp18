using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        #region Variables
        [HideInInspector]
        public static GameManager instance;

        [Header("Player Respawn Settings")]
        /// <summary>
        /// The list of spawnpoints that the manager can see.
        /// </summary>
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        [SerializeField]
        private Player playerScript;
        private float respawnTime;
        private IEnumerator respawnPlayerCR;

        [Header("Scene Change Settings")]
        public float fadeDuration;
        public AnimationCurve fadeCurve;

        private CanvasGroup canvasGroup;
        private const string CANVASNAME = "FADECANVAS";
        private const string IMAGENAME = "FADEIMAGE";
        #endregion

        #region Properties
        public Player PlayerScript
        {
            get
            {
                if (playerScript == null) { playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); }
                return playerScript;
            }
        }

        public float RespawnTime
        {
            get
            {
                if (respawnTime != PlayerScript.respawnTime)
                {
                    respawnTime = PlayerScript.respawnTime;
                }
                return respawnTime;
            }
        }
        #endregion

        #region Unity Functions
        private void Awake()
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

            LoadNewScene("Audio", true);
            SceneManager.activeSceneChanged += CheckMainSceneLoaded;
        }

        private void Start()
        {
            spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
        }
        #endregion

        #region Scene Management
        private void CheckMainSceneLoaded(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == 1)
            {
                SceneManager.LoadScene("UI_Hud", LoadSceneMode.Additive);
                SceneManager.LoadScene("UI_Pause_Menu", LoadSceneMode.Additive);

                Fabric.EventManager.Instance?.PostEvent("Expo to Combat Music");
            }
            else if (newScene.buildIndex == 0)
            {
                Fabric.EventManager.Instance?.PostEvent("Combat to Expo Music");
            }
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
            yield return InitiateFade(fadeDuration, Color.black, FadeType.Out);
            SceneManager.LoadScene(sceneName);
            yield return InitiateFade(fadeDuration, Color.black, FadeType.In);
            yield return null;
        }

        private IEnumerator InitiateFade(float fadeDuration, Color fadeColor, FadeType fadeType)
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
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            var imageComponent = fadeImage.GetComponent<RawImage>();
            imageComponent.color = fadeColor;
            DontDestroyOnLoad(canvas);

            yield return StartCoroutine(FadeCanvasGroup(fadeDuration, fadeType));
        }

        private IEnumerator FadeCanvasGroup(float fadeDuration, FadeType fadeType)
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
        #endregion

        #region Helper Functions
        public Vector3 GetClosestSpawnPoint(List<SpawnPoint> potentialPoints)
        {
            var bestSpawnPoint = new Vector3();

            GameObject bestGameObject = null;
            var closestDistanceSqr = Mathf.Infinity;
            var currentPosition = PlayerScript.gameObject.transform.position;

            foreach (var potentialTarget in potentialPoints)
            {
                if (potentialTarget.activated == true)
                {
                    var directionToTarget = potentialTarget.transform.position - currentPosition;
                    var dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestGameObject = potentialTarget.gameObject;
                    }
                }
            }
            bestSpawnPoint = bestGameObject.GetComponent<SpawnPoint>().respawnPosition;

            return bestSpawnPoint;
        }

        public void RespawnPlayer(Vector3 bestSpawnPoint)
        {
            if (respawnPlayerCR != null) { return; }
            respawnPlayerCR = RespawnAction(bestSpawnPoint);
            StartCoroutine(respawnPlayerCR);
        }

        private IEnumerator RespawnAction(Vector3 bestSpawnPoint)
        {
            PlayerScript.isDying = true;
            PlayerScript.PerformingAction = true;
            PlayerScript.invincible = true;
            PlayerScript.anim.SetTrigger("isDead");

            yield return InitiateFade(fadeDuration, Color.black, FadeType.Out);
            yield return new WaitForSeconds(fadeDuration);

            PlayerScript.transform.position = bestSpawnPoint;

            yield return new WaitForSeconds(RespawnTime);
            PlayerScript.RestoreHealthToCharacter(PlayerScript.totalHealth);

            yield return InitiateFade(fadeDuration, Color.black, FadeType.In);
            yield return new WaitForSeconds(fadeDuration / 2);

            PlayerScript.isDying = false;
            PlayerScript.PerformingAction = false;
            PlayerScript.invincible = false;
            yield return null;
            respawnPlayerCR = null;
        }
        #endregion
    }
}
