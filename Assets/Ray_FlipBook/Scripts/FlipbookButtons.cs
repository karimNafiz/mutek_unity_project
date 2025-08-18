using UnityEngine;
using UnityEngine.UI;
using System;

namespace Flipbook
{
    /// <summary>
    /// References to page turning buttons and dispatches events when clicked.
    /// </summary>
    public class FlipbookButtons : MonoBehaviour
    {
        public EventHandler OnNextPageClicked;
        public EventHandler OnPrevPageClicked;
        
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;

        private void Awake()
        {
            nextPageButton.onClick.AddListener(() => 
                OnNextPageClicked?.Invoke(this, EventArgs.Empty));
            prevPageButton.onClick.AddListener(() => 
                OnPrevPageClicked?.Invoke(this, EventArgs.Empty));
        }

        private void ShowBothButtons()
        {
            nextPageButton.gameObject.SetActive(true);
            prevPageButton.gameObject.SetActive(true);
        }

        private void PrevButtonOnly()
        {
            nextPageButton.gameObject.SetActive(false);
            prevPageButton.gameObject.SetActive(true);
        }

        private void NextButtonOnly()
        {
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
            
        }

        /// <summary>
        /// Update the next and prev button activities based on the current page index and total page count.
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="totalPageCount"></param>
        public void UpdateButtonActivities(int currentPageIndex, int totalPageCount)
        {
            if (currentPageIndex == 0)
            {
                NextButtonOnly();
            }
            else if (currentPageIndex == totalPageCount - 1)
            {
                PrevButtonOnly();
            }
            else
            {
                ShowBothButtons();
            }
        }
    }

}
