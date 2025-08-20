using System;
using UnityEngine;
using ScriptableObjects;

namespace Flipbook
{
    public class FlipbookController : MonoBehaviour
    {
        [SerializeField] private SO_FlipbookPageCollection flipbookPageCollection;
        [SerializeField] private FlipbookVisual flipbookVisual;
        [SerializeField] private FlipbookButtons flipbookButtons;

        // keep track of the animation state of page flipping.
        private bool _isPageFlipping = false;
        private bool _isBookUp = false;
        
        private void Awake()
        {
            // Optional: start the book at page 0.
            // Not writing this line will have the book (page collection) to remember the last page it was on.
            if (flipbookPageCollection != null)
            {
                flipbookPageCollection.InitializeBook(0);
            }
        }

        private void Start()
        {
            flipbookButtons.OnNextPageClicked += FlipbookButtons_OnNextPageClicked;
            flipbookButtons.OnPrevPageClicked += FlipbookButtons_OnPrevPageClicked;
            
            // Initialize book position
            if (_isBookUp)
            {
                flipbookVisual.HoldUpFlipbook();
            }
            else
            {
                flipbookVisual.PutDownFlipbook();
            }
            
            // Make sure to show the current page to initialize the GUI display
            UpdateCurrentPage();
        }

        private void OnDestroy()
        {
            flipbookButtons.OnNextPageClicked -= FlipbookButtons_OnNextPageClicked;
            flipbookButtons.OnPrevPageClicked -= FlipbookButtons_OnPrevPageClicked;
        }

        private void FlipbookButtons_OnNextPageClicked(object sender, EventArgs e)
        {
            ShowNextPage();
        }
        
        private void FlipbookButtons_OnPrevPageClicked(object sender, EventArgs e)
        {
            ShowPreviousPage();
        }

        /// <summary>
        /// To be passed as a callback to the FlipbookVisual, to release the page flip lock when
        /// the page flipping animation has completed.
        /// </summary>
        private void ReleasePageFlipLock()
        {
            _isPageFlipping = false;
        }
        
        /// <summary>
        /// Update the text display content of with the current page content.
        /// </summary>
        private void UpdateCurrentPage()
        {
            if (flipbookPageCollection.TryGetCurrentPage(out SO_FlipbookPageContent content))
            {
                flipbookVisual.UpdateCurrentPage(content.PageContent);
                // update next/prev page button activities
                flipbookButtons.UpdateButtonActivities(
                    flipbookPageCollection.GetCurrentPageIndex(), 
                    flipbookPageCollection.GetPageCount());
            }
        }

        
        ////////////////////// PUBLIC METHODS ////////////////////////////////
        /// <summary>
        /// Sets a new page collection to the flipbook and displays the current page recorded
        /// in the collection.
        /// </summary>
        /// <param name="flipbookPageCollection"></param>
        public void SetFlipbookPageCollection(SO_FlipbookPageCollection flipbookPageCollection)
        {
            if (_isPageFlipping)
            {
                Debug.LogWarning("Page flipping animation in progress, change book request blocked");
                return;
            }
            this.flipbookPageCollection = flipbookPageCollection;
            UpdateCurrentPage();
        }
        
        /// <summary>
        /// To Show the flip book with a holding up animation
        /// </summary>
        public void HoldUpFlipbook()
        {
            if (_isBookUp)
            {
                return;
            }
            
            flipbookVisual.HoldUpFlipbook();
            _isBookUp = true;
        }
        
        /// <summary>
        /// To Hide the flip book with a putting down animation
        /// </summary>
        public void PutDownFlipbook()
        {
            if (!_isBookUp)
            {
                return;
            }
            
            flipbookVisual.PutDownFlipbook();
            _isBookUp = false;
        }

        
        /// <summary>
        /// To show the next page of the flipbook, with a page flip animation.
        /// </summary>
        public void ShowNextPage()
        {
            if (_isPageFlipping)
            {
                Debug.Log("Page flipping animation in progress, new request blocked");
                return;
            }
            
            if (flipbookPageCollection.TryGetNextPage(out SO_FlipbookPageContent content))
            {
                // mark the _isPageFlipping flag to prevent the next page flip from being triggered during animation
                _isPageFlipping = true;
                flipbookVisual.ShowNextPage(content.PageContent, ReleasePageFlipLock);
                // update next/prev page button activities
                flipbookButtons.UpdateButtonActivities(
                    flipbookPageCollection.GetCurrentPageIndex(), 
                    flipbookPageCollection.GetPageCount());
            }
        }

        /// <summary>
        /// To show the previous page of the flipbook, with a page flip animation.
        /// </summary>
        public void ShowPreviousPage()
        {
            if (_isPageFlipping)
            {
                Debug.Log("Page flipping animation in progress, new request blocked");
                return;
            }
            
            if (flipbookPageCollection.TryGetPrevPage(out SO_FlipbookPageContent content))
            {
                // mark the _isPageFlipping flag to prevent the next page flip from being triggered during animation
                _isPageFlipping = true;
                flipbookVisual.ShowPreviousPage(content.PageContent, ReleasePageFlipLock);
                // update next/prev page button activities
                flipbookButtons.UpdateButtonActivities(
                    flipbookPageCollection.GetCurrentPageIndex(), 
                    flipbookPageCollection.GetPageCount());
            }
        }
        
        /*
         * Possible Extension: Go to a specific page.
         */
    }

}
