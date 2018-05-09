using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hunter.Characters;
using UnityEngine.UI;

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
        public Image fireMaladiteIcon;
        public Image iceMaladiteIcon;
        public Image electricMaladiteIcon;
        public Image silverMaladiteIcon;
        public Image decanterFirstIcon;
        public Image decanterSecondIcon;
        public Image controlsLayoutBackground;

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
        private List<BestiaryItem> bestiaryEntries;
        private List<MapItem> mapEntries;
        private List<ElementModItem> elementEntries;

        public bool IsGamePaused
        {
            get
            {
                return menuCanvas != null && menuCanvas.gameObject.activeSelf;
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
                if(value < 0 || value > 4) { return; }
                currentTabIndex = value;
                SwitchTabs();
            }
        }

        private Player playerWhoPaused;

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

            if (menuCanvas == null)
            {
                menuCanvas = GetComponentInChildren<Canvas>();
            }
            Time.timeScale = 1;
            menuCanvas.gameObject.SetActive(false);
        }

        public void PauseGame (Player player)
        {
            Time.timeScale = 0;
            playerWhoPaused = player;

            Fabric.EventManager.Instance?.PostEvent("UI Start Game");
            menuCanvas.gameObject.SetActive(true);

            CurrentTabIndex = 0;
        }

        public void UnpauseGame ()
        {
            Time.timeScale = 1;
            playerWhoPaused = null;

            Fabric.EventManager.Instance?.PostEvent("UI Navigation Back");

            journalEntries = null;
            diaryEntries = null;
            bestiaryEntries = null;
            mapEntries = null;
            elementEntries = null;
            journalPageIndex = 0;
            diaryPageIndex = 0;
            bestiaryPageIndex = 0;

            menuCanvas.gameObject.SetActive(false);
        }

        public void SwitchTabs ()
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
                    DisplayBestiary();
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
                    DisplayBestiary();
                    break;
                default:
                    return;
            }
        }

        #region Pause
        public void DisplayPause ()
        {

        }
        #endregion


        #region Bestiary
        public void EnableBestiaryTab ()
        {
            if (bestiaryEntries == null)
            {
                bestiaryEntries = playerWhoPaused.Inventory.GetAllBestiaries();
            }

            journalHeader.fontStyle = FontStyles.Bold;
            bestiaryLeftPageRoot.SetActive(true);
            bestiaryRightPageRoot.SetActive(true);
            //Fabric.EventManager.Instance?.PostEvent("UI Page Flip");
            DisplayBestiaryPages();
        }

        private void FlipBestiaryPage (bool forwards)
        {
            if (bestiaryEntries == null || bestiaryEntries.Count == 0) { return; }

            if (forwards)
            {
                if (bestiaryPageIndex + 2 > bestiaryEntries.Count) { return; }
                bestiaryPageIndex += 2;
            }
            else
            {
                if (bestiaryPageIndex - 2 < 0) { return; }
                bestiaryPageIndex -= 2;
            }

            DisplayBestiaryPages();
        }

        private void DisplayBestiaryPages ()
        {
            for (int i = journalPageIndex; i < journalPageIndex + 2; i++)
            {
                if (i >= journalEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = "";
                        modularLeftPageText.text = "Nothing here yet.";
                    }
                    else
                    {
                        modularRightPageTitle.text = "";
                        modularRightPageText.text = "Nothing here yet.";
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

            leftArrow.gameObject.SetActive(bestiaryPageIndex - 2 > 0);
            rightArrow.gameObject.SetActive(bestiaryPageIndex + 2 < bestiaryEntries.Count);
        }
        #endregion

        #region Map
        public void EnableMapTab ()
        {
            if(mapEntries == null)
            {
                mapEntries = playerWhoPaused.Inventory.GetAllMaps();
            }

            mapHeader.fontStyle = FontStyles.Bold;
            modularLeftPageRoot.SetActive(true);
            modularRightPageRoot.SetActive(true);

            DisplayMaps();
        }

        private void DisplayMaps ()
        {
            for (int i = 0; i < 2; i++)
            {
                if (i >= mapEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = "";
                        modularLeftPageText.text = "Nothing here yet.";
                    }
                    else
                    {
                        modularRightPageTitle.text = "";
                        modularRightPageText.text = "Nothing here yet.";
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = mapEntries[i].itemName;
                        modularLeftPageBackground.sprite = mapEntries[i].mapSprite;
                    }
                    else
                    {
                        modularRightPageTitle.text = mapEntries[i].itemName;
                        modularRightPageBackground.sprite = mapEntries[i].mapSprite;
                    }
                }
            }
        }
        #endregion

        #region Journal
        public void EnableJournalTab ()
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

        private void FlipJournalPage (bool forwards)
        {
            if(journalEntries == null) { return; }

            if (forwards)
            {
                if(journalPageIndex + 2 > journalEntries.Count) { return; }
                journalPageIndex += 2;
            }
            else
            {
                if(journalPageIndex - 2 < 0) { return; }
                journalPageIndex -= 2;
            }
            
            DisplayJournalPages();
        }

        private void DisplayJournalPages ()
        {
            for (int i = journalPageIndex; i < journalPageIndex + 2; i++)
            {
                if (i >= journalEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = "";
                        modularLeftPageText.text = "Nothing here yet.";
                    }
                    else
                    {
                        modularRightPageTitle.text = "";
                        modularRightPageText.text = "Nothing here yet.";
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

            leftArrow.gameObject.SetActive(journalPageIndex - 2 > 0);
            rightArrow.gameObject.SetActive(journalPageIndex + 2 < journalEntries.Count);
        }
        #endregion

        #region Diaries
        public void EnableDiaryTab ()
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

        private void FlipDiaryPage (bool forwards)
        {
            if (diaryEntries == null) { return; }

            if (forwards)
            {
                if (diaryPageIndex + 2 > diaryEntries.Count) { return; }
                diaryPageIndex += 2;
            }
            else
            {
                if (diaryPageIndex - 2 < 0) { return; }
                diaryPageIndex -= 2;
            }

            DisplayDiaryPages();
        }

        private void DisplayDiaryPages ()
        {
            for (int i = diaryPageIndex; i < diaryPageIndex + 2; i++)
            {
                if (i >= diaryEntries.Count)
                {
                    //Even numbered entries go on the left page, odd numbered ones go to the right
                    if (i % 2 == 0)
                    {
                        modularLeftPageTitle.text = "";
                        modularLeftPageText.text = "Nothing here yet.";
                    }
                    else
                    {
                        modularRightPageTitle.text = "";
                        modularRightPageText.text = "Nothing here yet.";
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

            leftArrow.gameObject.SetActive(diaryPageIndex - 2 > 0);
            rightArrow.gameObject.SetActive(diaryPageIndex + 2 < diaryEntries.Count);
        }
        #endregion

        private void ResetAllPages ()
        {
            diaryHeader.fontStyle = journalHeader.fontStyle = mapHeader.fontStyle = bestiaryHeader.fontStyle = pauseHeader.fontStyle = FontStyles.Normal;
            modularLeftPageBackground.enabled = modularRightPageBackground.enabled = false;
            modularLeftPageText.text = modularRightPageText.text = "";
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            pauseLeftPageRoot.SetActive(false);
            pauseRightPageRoot.SetActive(false);
            modularLeftPageRoot.SetActive(false);
            modularRightPageRoot.SetActive(false);
            bestiaryLeftPageRoot.SetActive(false);
            bestiaryRightPageRoot.SetActive(false);
        }
    }
}
