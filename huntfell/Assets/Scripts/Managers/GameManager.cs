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

        [Header("Toggle Floor Settings")]
        public List<GameObject> firstFloor = new List<GameObject>();
        public List<GameObject> secondFloor = new List<GameObject>();

        private GameObject player;
        #endregion

        #region Properties
        public GameObject Player
        {
            get
            {
                if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
                return player;
            }
        }

        public List<Minion> MinionsInPlayerRadius { get; private set; } = new List<Minion>();
        public Boss BossInRadius { get; private set; }
        private string currentMusicEvent;
        #endregion

        #region Unity Functions
        private void Awake()
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


            DeviceManager = GetComponent<DeviceManager>();
            director = GetComponentInChildren<PlayableDirector>();
            spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());

            if (loadTitleScreen) { LoadTitleScreen(); }
            else { StartGame(); }

            if (GameObject.Find("Audio") == null)
            {
                LoadNewScene("Audio", true);
            }

            Time.timeScale = 1;
            StartCoroutine(FadeScreen(fadeDuration, Color.black, FadeType.In));
        }

        private void Update()
        {
            if (Player.transform.position.y < -1)
            {
                for (var i = 0; i < firstFloor.Count; i++)
                {
                    if (firstFloor[i].activeInHierarchy == true) { firstFloor[i].SetActive(false); }
                }

                for (var i = 0; i < secondFloor.Count; i++)
                {
                    if (secondFloor[i].activeInHierarchy == false) { secondFloor[i].SetActive(true); }
                }
            }
            else if (Player.transform.position.y > -1)
            {
                for (var i = 0; i < firstFloor.Count; i++)
                {
                    if (firstFloor[i].activeInHierarchy == false) { firstFloor[i].SetActive(true); }
                }

                for (var i = 0; i < secondFloor.Count; i++)
                {
                    if (secondFloor[i].activeInHierarchy == true) { secondFloor[i].SetActive(false); }
                }
            }
        }
        #endregion

        #region Scene Management

        public IEnumerator IntroCutscene(CanvasGroup titleScreenCanvasGroup)
        {
            Fabric.EventManager.Instance?.PostEvent("Stop Main Menu Loop, Start Game");
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

            StartGame();
        }

        private void StartGame()
        {
            SceneManager.LoadScene("SpencerWithers_Hud_Scene", LoadSceneMode.Additive);
            SceneManager.LoadScene("SpencerWithers_Pause_Scene", LoadSceneMode.Additive);
            Fabric.EventManager.Instance?.PostEvent("Music - Start Expo");
            DeviceManager.GameInputEnabled = true;
        }

        public void QuitToMenu()
        {
            DeviceManager.PauseInputEnabled = false;
            LoadNewScene(0, false);
        }

        private void LoadTitleScreen()
        {
            LoadNewScene("UI_Title_Menu", true);
            titleScreenCM.Priority = 2;
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

        public void LoadNewScene(int sceneIndex, bool loadAdditively)
        {
            if (loadAdditively)
            {
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
            }
            else
            {
                StartCoroutine(ChangeActiveScene(sceneIndex));
            }
        }

        private IEnumerator ChangeActiveScene(string sceneName)
        {
            yield return FadeScreen(fadeDuration, Color.black, FadeType.Out);
            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator ChangeActiveScene(int sceneIndex)
        {
            yield return FadeScreen(fadeDuration, Color.black, FadeType.Out);
            SceneManager.LoadScene(sceneIndex);
        }

        #endregion

        #region Helper Functions
        public void ResetRadius ()
        {
            MinionsInPlayerRadius.Clear();
            BossInRadius = null;
            PostMusicEvent("Music - Stop Boss Combat");
            PostMusicEvent("Music - Start Expo");
            HUDManager.instance?.FadeBossHealth(FadeType.In);
        }

        public void RemoveMinionFromRadius (Minion minion)
        {
            if (MinionsInPlayerRadius.Contains(minion))
            {
                MinionsInPlayerRadius.Remove(minion);
                Debug.Log($"Removed {minion.name} from the radius.");
            }

            if (MinionsInPlayerRadius.Count < 1 && BossInRadius == null)
            {
                PostMusicEvent("Music - Regular Combat to Expo");
            }
        }

        public void AddMinionToRadius (Minion minion)
        {
            if (!MinionsInPlayerRadius.Contains(minion))
            {
                MinionsInPlayerRadius.Add(minion);
                Debug.Log($"Added {minion.name} to the radius.");
            }

            if (MinionsInPlayerRadius.Count > 0 && BossInRadius == null)
            {
                PostMusicEvent("Music - Expo to Regular Combat");
            }
        }

        public void RemoveBossFromRadius(Boss boss)
        {
            if(BossInRadius == boss)
            {
                BossInRadius = null;
                HUDManager.instance?.FadeBossHealth(FadeType.In);

                PostMusicEvent("Music - Stop Boss Combat");
            }
        }

        public void AddBossToRadius (Boss boss)
        {
            if (BossInRadius == null)
            {
                BossInRadius = boss;
                HUDManager.instance?.FadeBossHealth(FadeType.Out);

                PostMusicEvent("Music - Stop Combat");
                PostMusicEvent("Music - Stop Expo");
                PostMusicEvent("Music - Start Boss Combat");
            }
        }

        private void PostMusicEvent (string musicSoundEvent)
        {
            if (musicSoundEvent != currentMusicEvent && !string.IsNullOrWhiteSpace(musicSoundEvent))
            {
                Fabric.EventManager.Instance?.PostEvent(musicSoundEvent);
                currentMusicEvent = musicSoundEvent;
            }
        }

        public IEnumerator FadeScreen(Color fadeColor, FadeType fadeType)
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
                if (!spawnPoint.activated || !spawnPoint.gameObject.activeInHierarchy) { continue; }

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
