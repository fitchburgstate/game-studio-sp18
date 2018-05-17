using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hunter.Characters;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

namespace Hunter
{
    public class PauseManager : MonoBehaviour
    {
        [HideInInspector]
        public static PauseManager instance;

        [Header("Global Elements")]
        public TextMeshProUGUI journalHeader;
        public TextMeshProUGUI diaryHeader;
        public TextMeshProUGUI mapHeader;
        public TextMeshProUGUI bestiaryHeader;
        public TextMeshProUGUI pauseHeader;
        public Image leftArrow;
        public Image rightArrow;

        [Header("Pause Page Elements")]
        public GameObject pauseLeftPageRoot;
        public GameObject pauseRightPageRoot;
        public GameObject controlsLeftPage;
        public GameObject controlsRightPage;
        [Space]
        public Button defaultButton;
        public Image fireMaladiteIcon;
        public Image iceMaladiteIcon;
        public Image electricMaladiteIcon;
        public Image silverMaladiteIcon;
        public Image decanterFirstIcon;
        public Image decanterSecondIcon;

        [Header("Modular Page Elements")]
        public GameObject modularLeftPageRoot;
        /// <summary>
        /// This should be the map background
        /// </summary>
        public Image modularLeftPageBackground;
        public TextMeshProUGUI modularLeftPageTitle;
        public TextMeshProUGUI modularLeftPageText;
        [Space]
        public GameObject modularRightPageRoot;
        /// <summary>
        /// This should be the map background
        /// </summary>
        public Image modularRightPageBackground;
        public TextMeshProUGUI modularRightPageTitle;
        public TextMeshProUGUI modularRightPageText;

        [Header("Bestiary Page Elements")]
        public GameObject bestiaryLeftPageRoot;
        public TextMeshProUGUI bestiaryLeftPageTitle;
        public TextMeshProUGUI bestiaryLeftPageFirstEntry;
        public Image bestiaryLeftPageFirstSketch;
        public TextMeshProUGUI bestiaryLeftPageSecondEntry;
        public Image bestiaryLeftPageSecondSketch;
        public TextMeshProUGUI bestiaryLeftPageThirdEntry;
        public Image bestiaryLeftPageThirdSketch;
        [Space]
        public GameObject bestiaryRightPageRoot;
        public TextMeshProUGUI bestiaryRightPageTitle;
        public TextMeshProUGUI bestiaryRightPageFirstEntry;
        public Image bestiaryRightPageFirstSketch;
        public TextMeshProUGUI bestiaryRightPageSecondEntry;
        public Image bestiaryRightPageSecondSketch;
        public TextMeshProUGUI bestiaryRightPageThirdEntry;
        public Image bestiaryRightPageThirdSketch;

        public Canvas menuCanvas;

        private int currentTabIndex = 0;
        private int journalPageIndex = 0;
        private int diaryPageIndex = 0;
        private int bestiaryPageIndex = 0;

        private List<JournalItem> journalEntries;
        private List<DiaryItem> diaryEntries;
        private List<MapItem> mapEntries;
        private List<ElementModItem> elementEntries;

        private List<BestiaryItem> bestiaryEntries;
        private List<BestiaryItem> wolfEntries;
        private List<BestiaryItem> batEntries;
        private List<BestiaryItem> gargoyleEntries;
        private List<BestiaryItem> werewolfEntries;

        public bool IsGamePaused
        {
            get
            {
                return menuCanvas != null && menuCanvas.gameObject.activeSelf;
            }
        }

        public bool IsControlsOpen
        {
            get
            {
                return controlsLeftPage.activeSelf || controlsRightPage.activeSelf;
            }
        }

        public int CurrentTabIndex
        {
            get
            {
                return currentTabIndex;
            }

            set
            {
                // Our tab index ranges from 0 to 4 since there are 5 total tabs in the pause menu, this prevents under/over flow
                if (value < 0 || value > 4) { return; }
                currentTabIndex = value;
                SwitchTabs();
            }
        }

        private Player playerWhoPaused;

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

            if (menuCanvas == null)
            {
                menuCanvas = GetComponentInChildren<Canvas>();
            }
            Time.timeScale = 1;
            menuCanvas.gameObject.SetActive(false);
        }

        public void PauseGame(Player player, int tabToOpen)
        {
            Time.timeScale = 0;
            playerWhoPaused = player;

            if (GameManager.instance != null)
            {
                GameManager.instance.DeviceManager.PauseInputEnabled = true;
                GameManager.instance.DeviceManager.GameInputEnabled = false;
            }

            Fabric.EventManager.Instance?.PostEvent("UI Page Flip");
            menuCanvas.gameObject.SetActive(true);

            CurrentTabIndex = Mathf.Clamp(tabToOpen, 0, 4);
        }

        public void UnpauseGame()
        {
            if (IsControlsOpen) { return; }

            Time.timeScale = 1;
            playerWhoPaused = null;

            if (GameManager.instance != null)
            {
                GameManager.instance.DeviceManager.PauseInputEnabled = false;
                GameManager.instance.DeviceManager.GameInputEnabled = true;
            }

            Fabric.EventManager.Instance?.PostEvent("UI Page Flip");

            journalEntries = null;
            diaryEntries = null;
            bestiaryEntries = null;
            mapEntries = null;
            elementEntries = null;
            wolfEntries = null;
            batEntries = null;
            gargoyleEntries = null;
            werewolfEntries = null;
            journalPageIndex = 0;
            diaryPageIndex = 0;
            bestiaryPageIndex = 0;

            menuCanvas.gameObject.SetActive(false);
        }

        public void QuitToMainMenu()
        {
            if (IsControlsOpen) { return; }

            GameManager.instance?.QuitToMenu();
        }

        public void OpenControls()
        {
            if (IsControlsOpen) { return; }

            controlsLeftPage.SetActive(true);
            controlsRightPage.SetActive(true);
        }

        public void CloseControls()
        {
            controlsLeftPage.SetActive(false);
            controlsRightPage.SetActive(false);
        }

        public void SwitchTabs()
        {
            if (playerWhoPaused == null) { return; }
            ResetAllPages();

            switch (CurrentTabIndex)
            {
                case 0:
                    DisplayPause();
                    break;
                case 1:
                    EnableJournalTab();
                    break;
                case 2:
                    EnableDiaryTab();
                    break;
                case 3:
                    EnableBestiaryTab();
                    break;
                case 4:
                    EnableMapTab();
                    break;
            }
        }

        public void SwitchPage(bool forwards)
        {
            if (playerWhoPaused == null) { return; }
            switch (CurrentTabIndex)
            {
                case 1:
                    FlipJournalPage(forwards);
                    break;
                case 2:
                    FlipDiaryPage(forwards);
                    break;
                case 3:
                    FlipBestiaryPage(forwards);
                    break;
                default:
                    return;
            }
        }

        #region Pause
        public void DisplayPause()
        {
            if (elementEntries == null)
            {
                elementEntries = playerWhoPaused.Inventory.GetAllElements();
            }
            pauseHeader.fontStyle = FontStyles.Bold;
            pauseLeftPageRoot.SetActive(true);
            pauseRightPageRoot.SetActive(true);

            StartCoroutine(SelectDefaultButton());

            foreach (var element in elementEntries)
            {
                switch (element.elementOption)
                {
                    case ElementOption.Fire:
                        fireMaladiteIcon.enabled = true;
                        break;
                    case ElementOption.Ice:
                        iceMaladiteIcon.enabled = true;
                        break;
                    case ElementOption.Electric:
                        electricMaladiteIcon.enabled = true;
                        break;
                    case ElementOption.Silver:
                        silverMaladiteIcon.enabled = true;
                        break;
                }
            }

            if (playerWhoPaused.PotionCount > 0)
            {
                decanterFirstIcon.enabled = true;
            }
            if (playerWhoPaused.PotionCount > 1)
            {
                decanterSecondIcon.enabled = true;
            }
        }

        private IEnumerator SelectDefaultButton()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return null;
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
        #endregion

        #region Bestiary
        public void EnableBestiaryTab()
        {
            if (bestiaryEntries == null)
            {
                bestiaryEntries = playerWhoPaused.Inventory.GetAllBestiaries();

                wolfEntries = new List<BestiaryItem>(bestiaryEntries.Where(entry => entry.enemyType == EnemyType.Wolf).OrderBy(entry => entry.entryOrder));
                batEntries = new List<BestiaryItem>(bestiaryEntries.Where(entry => entry.enemyType == EnemyType.Bat).OrderBy(entry => entry.entryOrder));
                gargoyleEntries = new List<BestiaryItem>(bestiaryEntries.Where(entry => entry.enemyType == EnemyType.Gargoyle).OrderBy(entry => entry.entryOrder));
                werewolfEntries = new List<BestiaryItem>(bestiaryEntries.Where(entry => entry.enemyType == EnemyType.Thomas).OrderBy(entry => entry.entryOrder));
            }

            bestiaryHeader.fontStyle = FontStyles.Bold;
            bestiaryLeftPageRoot.SetActive(true);
            bestiaryRightPageRoot.SetActive(true);
            DisplayBestiaryPages();
        }

        private void FlipBestiaryPage(bool forwards)
        {
            if (bestiaryEntries == null) { return; }

            if (forwards)
            {
                if (bestiaryPageIndex + 2 > 2) { return; }
                bestiaryPageIndex += 2;
            }
            else
            {
                if (bestiaryPageIndex - 2 < 0) { return; }
                bestiaryPageIndex -= 2;
            }

            DisplayBestiaryPages();
        }

        private void DisplayBestiaryPages()
        {
            switch (bestiaryPageIndex)
            {
                case 0:
                    DisplayLeftBestiaryPage(wolfEntries);
                    DisplayRightBestiaryPage(batEntries);

                    rightArrow.gameObject.SetActive(true);
                    leftArrow.gameObject.SetActive(false);
                    break;
                case 2:
                    DisplayLeftBestiaryPage(gargoyleEntries);
                    DisplayRightBestiaryPage(werewolfEntries);

                    leftArrow.gameObject.SetActive(true);
                    rightArrow.gameObject.SetActive(false);
                    break;
            }
        }

        private void DisplayLeftBestiaryPage(List<BestiaryItem> entries)
        {
            if (entries.Count == 0)
            {
                bestiaryLeftPageTitle.text = "Undiscovered Beast";
                bestiaryLeftPageFirstEntry.text = bestiaryLeftPageSecondEntry.text = bestiaryLeftPageThirdEntry.text = "";
                bestiaryLeftPageFirstSketch.enabled = bestiaryLeftPageSecondSketch.enabled = bestiaryLeftPageThirdSketch.enabled = false;
                return;
            }
            else if(entries.Count == 1)
            {
                bestiaryLeftPageSecondEntry.text = bestiaryLeftPageThirdEntry.text = "";
                bestiaryLeftPageSecondSketch.enabled = bestiaryLeftPageThirdSketch.enabled = false;
            }
            else if (entries.Count == 2)
            {
                bestiaryLeftPageThirdEntry.text = "";
                bestiaryLeftPageThirdSketch.enabled = false;
            }

            bestiaryLeftPageTitle.text = entries[0].enemyType.ToString();
            for (int i = 0; i < 3; i++)
            {
                if (i >= entries.Count) { break; }
                switch (i)
                {
                    case 0:
                        bestiaryLeftPageFirstEntry.text = entries[i].bookContents;
                        bestiaryLeftPageFirstSketch.sprite = entries[i].entrySketch;
                        bestiaryLeftPageFirstSketch.enabled = true;
                        break;
                    case 1:
                        bestiaryLeftPageSecondEntry.text = entries[i].bookContents;
                        bestiaryLeftPageSecondSketch.sprite = entries[i].entrySketch;
                        bestiaryLeftPageSecondSketch.enabled = true;
                        break;
                    case 2:
                        bestiaryLeftPageThirdEntry.text = entries[i].bookContents;
                        bestiaryLeftPageThirdSketch.sprite = entries[i].entrySketch;
                        bestiaryLeftPageThirdSketch.enabled = true;
                        break;
                }
            }
        }

        private void DisplayRightBestiaryPage(List<BestiaryItem> entries)
        {
            if (entries.Count == 0)
            {
                bestiaryRightPageTitle.text = "Undiscovered Beast";
                bestiaryRightPageFirstEntry.text = bestiaryRightPageSecondEntry.text = bestiaryRightPageThirdEntry.text = "";
                bestiaryRightPageFirstSketch.enabled = bestiaryRightPageSecondSketch.enabled = bestiaryRightPageThirdSketch.enabled = false;
                return;
            }
            else if (entries.Count == 1)
            {
                bestiaryRightPageSecondEntry.text = bestiaryRightPageThirdEntry.text = "";
                bestiaryRightPageSecondSketch.enabled = bestiaryRightPageThirdSketch.enabled = false;
            }
            else if (entries.Count == 2)
            {
                bestiaryRightPageThirdEntry.text = "";
                bestiaryRightPageThirdSketch.enabled = false;
            }

            bestiaryRightPageTitle.text = entries[0].enemyType.ToString();
            for (int i = 0; i < 3; i++)
            {
                if (i >= entries.Count) { break; }
                switch (i)
                {
                    case 0:
                        bestiaryRightPageFirstEntry.text = entries[i].bookContents;
                        bestiaryRightPageFirstSketch.sprite = entries[i].entrySketch;
                        bestiaryRightPageFirstSketch.enabled = true;
                        break;
                    case 1:
                        bestiaryRightPageSecondEntry.text = entries[i].bookContents;
                        bestiaryRightPageSecondSketch.sprite = entries[i].entrySketch;
                        bestiaryRightPageSecondSketch.enabled = true;
                        break;
                    case 2:
                        bestiaryRightPageThirdEntry.text = entries[i].bookContents;
                        bestiaryRightPageThirdSketch.sprite = entries[i].entrySketch;
                        bestiaryRightPageThirdSketch.enabled = true;
                        break;
                }
            }
        }
        #endregion

        #region Map
        public void EnableMapTab()
        {
            if (mapEntries == null)
            {
                mapEntries = playerWhoPaused.Inventory.GetAllMaps();
            }

            mapHeader.fontStyle = FontStyles.Bold;
            modularLeftPageRoot.SetActive(true);
            modularRightPageRoot.SetActive(true);

            DisplayMaps();
        }

        private void DisplayMaps()
        {
            for (int i = 0; i < 2; i++)
            {
                if (i >= mapEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = "Undiscovered Map";
                        //modularLeftPageText.text = "Nothing here yet.";
                    }
                    else
                    {
                        modularRightPageTitle.text = "Undiscovered Map";
                        //modularRightPageText.text = "Nothing here yet.";
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = mapEntries[i].itemName;
                        modularLeftPageBackground.sprite = mapEntries[i].mapSprite;
                        modularLeftPageBackground.enabled = true;
                    }
                    else
                    {
                        modularRightPageTitle.text = mapEntries[i].itemName;
                        modularRightPageBackground.sprite = mapEntries[i].mapSprite;
                        modularRightPageBackground.enabled = true;
                    }
                }
            }
        }
        #endregion

        #region Journal
        public void EnableJournalTab()
        {
            if (journalEntries == null)
            {
                journalEntries = playerWhoPaused.Inventory.GetAllJournals();
            }

            journalHeader.fontStyle = FontStyles.Bold;
            modularLeftPageRoot.SetActive(true);
            modularRightPageRoot.SetActive(true);
            //Fabric.EventManager.Instance?.PostEvent("UI Page Flip");
            DisplayJournalPages();
        }

        private void FlipJournalPage(bool forwards)
        {
            if (journalEntries == null) { return; }

            if (forwards)
            {
                if (journalPageIndex + 2 >= journalEntries.Count) { return; }
                journalPageIndex += 2;
            }
            else
            {
                if (journalPageIndex - 2 < 0) { return; }
                journalPageIndex -= 2;
            }

            DisplayJournalPages();
        }

        private void DisplayJournalPages()
        {
            for (int i = journalPageIndex; i < journalPageIndex + 2; i++)
            {
                if (i >= journalEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = modularLeftPageText.text = "";
                    }
                    else
                    {
                        modularRightPageTitle.text = modularRightPageText.text = "";
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = journalEntries[i].itemName;
                        modularLeftPageText.text = journalEntries[i].bookContents;
                    }
                    else
                    {
                        modularRightPageTitle.text = journalEntries[i].itemName;
                        modularRightPageText.text = journalEntries[i].bookContents;
                    }
                }
            }

            leftArrow.gameObject.SetActive(journalPageIndex - 2 >= 0);
            rightArrow.gameObject.SetActive(journalPageIndex + 2 < journalEntries.Count);
        }
        #endregion

        #region Diaries
        public void EnableDiaryTab()
        {
            if (diaryEntries == null)
            {
                diaryEntries = playerWhoPaused.Inventory.GetAllDiaries();
            }

            diaryHeader.fontStyle = FontStyles.Bold;
            modularLeftPageRoot.SetActive(true);
            modularRightPageRoot.SetActive(true);
            //Fabric.EventManager.Instance?.PostEvent("UI Page Flip");
            DisplayDiaryPages();
        }

        private void FlipDiaryPage(bool forwards)
        {
            if (diaryEntries == null) { return; }

            if (forwards)
            {
                if (diaryPageIndex + 2 >= diaryEntries.Count) { return; }
                diaryPageIndex += 2;
            }
            else
            {
                if (diaryPageIndex - 2 < 0) { return; }
                diaryPageIndex -= 2;
            }

            DisplayDiaryPages();
        }

        private void DisplayDiaryPages()
        {
            for (int i = diaryPageIndex; i < diaryPageIndex + 2; i++)
            {
                if (i >= diaryEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = modularLeftPageText.text = "";
                    }
                    else
                    {
                        modularRightPageTitle.text = modularRightPageText.text = "";
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = diaryEntries[i].itemName;
                        modularLeftPageText.text = diaryEntries[i].bookContents;
                    }
                    else
                    {
                        modularRightPageTitle.text = diaryEntries[i].itemName;
                        modularRightPageText.text = diaryEntries[i].bookContents;
                    }
                }
            }

            leftArrow.gameObject.SetActive(diaryPageIndex - 2 >= 0);
            rightArrow.gameObject.SetActive(diaryPageIndex + 2 < diaryEntries.Count);
        }
        #endregion

        private void ResetAllPages()
        {
            //Reset tab styling
            diaryHeader.fontStyle = journalHeader.fontStyle = mapHeader.fontStyle = bestiaryHeader.fontStyle = pauseHeader.fontStyle = FontStyles.Normal;

            //Disable all images in pages
            bestiaryLeftPageFirstSketch.enabled = bestiaryLeftPageSecondSketch.enabled = bestiaryLeftPageThirdSketch.enabled = bestiaryRightPageFirstSketch.enabled = bestiaryRightPageSecondSketch.enabled = bestiaryRightPageThirdSketch.enabled = modularLeftPageBackground.enabled = modularRightPageBackground.enabled = fireMaladiteIcon.enabled = iceMaladiteIcon.enabled = electricMaladiteIcon.enabled = silverMaladiteIcon.enabled = decanterFirstIcon.enabled = decanterSecondIcon.enabled = false;

            //Reset all text in pages
            bestiaryLeftPageFirstEntry.text = bestiaryLeftPageSecondEntry.text = bestiaryLeftPageThirdEntry.text = bestiaryRightPageFirstEntry.text = bestiaryRightPageSecondEntry.text = bestiaryRightPageThirdEntry.text = modularLeftPageText.text = modularRightPageText.text = modularLeftPageTitle.text = modularRightPageTitle.text = "";

            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            pauseLeftPageRoot.SetActive(false);
            pauseRightPageRoot.SetActive(false);
            modularLeftPageRoot.SetActive(false);
            modularRightPageRoot.SetActive(false);
            bestiaryLeftPageRoot.SetActive(false);
            bestiaryRightPageRoot.SetActive(false);
            controlsLeftPage.SetActive(false);
            controlsRightPage.SetActive(false);
        }
    }
}
