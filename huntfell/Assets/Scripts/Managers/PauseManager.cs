using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        public void PauseGame ()
        {
            Time.timeScale = 0;
            Fabric.EventManager.Instance.PostEvent("UI Start Game");
            DisplayJournals();
            menuCanvas.gameObject.SetActive(true);
        }

        public void UnpauseGame ()
        {
            Time.timeScale = 1;
            Fabric.EventManager.Instance.PostEvent("UI Navigation Back");
            menuCanvas.gameObject.SetActive(false);
        }

        public void DisplayJournals ()
        {
            if (InventoryManager.instance == null) { return; }
            var journals = InventoryManager.instance.GetAllJournals();
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
            if (InventoryManager.instance == null) { return; }
            var diaries = InventoryManager.instance.GetAllDiaries();
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
