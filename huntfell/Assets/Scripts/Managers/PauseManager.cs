using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hunter.Characters;

namespace Hunter
{
    public class PauseManager : MonoBehaviour
    {

        [HideInInspector]
        public static PauseManager instance;

        public TextMeshProUGUI leftPageTitle;
        public TextMeshProUGUI leftPageText;
        public TextMeshProUGUI rightPageTitle;
        public TextMeshProUGUI rightPageText;

        public TextMeshProUGUI journalHeader;
        public TextMeshProUGUI diaryHeader;

        public Canvas menuCanvas;

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

            Fabric.EventManager.Instance.PostEvent("UI Start Game");
            menuCanvas.gameObject.SetActive(true);

            DisplayJournals();
        }

        public void UnpauseGame ()
        {
            Time.timeScale = 1;
            playerWhoPaused = null;

            Fabric.EventManager.Instance.PostEvent("UI Navigation Back");
            menuCanvas.gameObject.SetActive(false);
        }

        public void DisplayJournals ()
        {
            if (playerWhoPaused == null) { return; }
            var journals = playerWhoPaused.Inventory.GetAllJournals();
            journalHeader.fontStyle = FontStyles.Bold;
            diaryHeader.fontStyle = FontStyles.Normal;
            Fabric.EventManager.Instance.PostEvent("UI Page Flip");
            //Only displaying two journals for now
            for (int i = 0; i < 2; i++)
            {
                if (journals.Count - 1 < i)
                {
                    if (i == 0)
                    {
                        leftPageTitle.text = "";
                        leftPageText.text = "Nothing here yet.";
                    }
                    else if (i == 1)
                    {
                        rightPageTitle.text = "";
                        rightPageText.text = "Nothing here yet.";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        leftPageTitle.text = journals[i].itemName;
                        leftPageText.text = journals[i].bookContents;
                    }
                    else if (i == 1)
                    {
                        rightPageTitle.text = journals[i].itemName;
                        rightPageText.text = journals[i].bookContents;
                    }
                }
            }
        }

        public void DisplayDiaries ()
        {
            if (playerWhoPaused == null) { return; }
            var diaries = playerWhoPaused.Inventory.GetAllDiaries();
            diaryHeader.fontStyle = FontStyles.Bold;
            journalHeader.fontStyle = FontStyles.Normal;
            Fabric.EventManager.Instance.PostEvent("UI Page Flip");
            //Only displaying two diaries for now
            for (int i = 0; i < 2; i++)
            {
                if (diaries.Count - 1 < i)
                {
                    if (i == 0)
                    {
                        leftPageTitle.text = "";
                        leftPageText.text = "Nothing here yet.";
                    }
                    else if (i == 1)
                    {
                        rightPageTitle.text = "";
                        rightPageText.text = "Nothing here yet.";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        leftPageTitle.text = diaries[i].itemName;
                        leftPageText.text = diaries[i].bookContents;
                    }
                    else if (i == 1)
                    {
                        rightPageTitle.text = diaries[i].itemName;
                        rightPageText.text = diaries[i].bookContents;
                    }
                }
            }
        }
    }
}
