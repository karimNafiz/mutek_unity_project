using System;
using UnityEngine;
using ScriptableObjects;

namespace Flipbook
{
    public class FlipbookController : MonoBehaviour
    {
        [SerializeField] private SO_FlipbookPageCollection _flipbookPageCollection;
        [SerializeField] private FlipbookVisual _flipbookVisual;

        private void Awake()
        {
            // Optional: start the book at page 0.
            // Not writing this line will have the book (page collection) to remember the last page it was on.
            if (_flipbookPageCollection != null)
            {
                _flipbookPageCollection.InitializeBook(0);
            }
            
            // Make sure to show the current page to initialize the GUI display
            UpdateCurrentPage();
        }

        [ContextMenu("Flipbook Up")]
        public void HoldUpFlipbook()
        {
            _flipbookVisual.HoldUpFlipbook();
        }

        [ContextMenu("Flipbook Down")]
        public void PutDownFlipbook()
        {
            _flipbookVisual.PutDownFlipbook();
        }

        public void UpdateCurrentPage()
        {
            SO_FlipbookPageContent content = null;
            if (_flipbookPageCollection.GetCurrentPage(out content))
            {
                _flipbookVisual.UpdateCurrentPage(content.PageContent);
            }
        }
        
        [ContextMenu("Show Next Page")]
        public void ShowNextPage()
        {
            SO_FlipbookPageContent content = null;
            if (_flipbookPageCollection.GetNextPage(out content))
            {
                _flipbookVisual.ShowNextPage(content.PageContent); 
            }
        }

        [ContextMenu("Show Previous Page")]
        public void ShowPreviousPage()
        {
            SO_FlipbookPageContent content = null;
            if (_flipbookPageCollection.GetPrevPage(out content))
            {
                _flipbookVisual.ShowPreviousPage(content.PageContent);
            }
        }
        
        /*
         * Possible Extension: Go to a specific page.
         */
    }

}
