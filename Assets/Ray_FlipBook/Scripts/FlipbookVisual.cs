using System;
using UnityEngine;
using TMPro;

namespace Flipbook
{
    [RequireComponent(typeof(Animator))]
    public class FlipbookVisual : MonoBehaviour
    {
        /// <summary>
        /// A serializeable struct to store a page's transform and text field
        /// </summary>
        [Serializable]
        private struct PageContent
        {
            public Transform pageTransform;
            public TextMeshProUGUI contentTextGUI;
        }
        
        // const string trigger names for Animator
        private const string HOLD_UP_TRIGGER = "Up";
        private const string HOLD_DOWN_TRIGGER = "Down";
        private Animator _animator;

        
        // Array of page content structs to represent multiple flippable pages
        [SerializeField] private PageContent[] _pageContents;
        private int _pageIndex = 0;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Hold up the flip book from outside camera FOV
        /// </summary>
        public void HoldUpFlipbook()
        {
            _animator.SetTrigger(HOLD_UP_TRIGGER);
        }
    
        /// <summary>
        /// Put the flip book down to outslide camera FOV
        /// </summary>
        public void PutDownFlipbook()
        {
            _animator.SetTrigger(HOLD_DOWN_TRIGGER);
        }

        /// <summary>
        /// Update the current page with the new content to display
        /// </summary>
        /// <param name="contentToDisplay"></param>
        public void UpdateCurrentPage(string contentToDisplay)
        {
            
        }
        
        /// <summary>
        /// Flip out the current page and
        /// Show the next page with the content to Display
        /// </summary>
        /// <param name="contentToDisplay"></param>
        public void ShowNextPage(string contentToDisplay)
        {
            
        }

        /// <summary>
        /// Flip in the prev page and
        /// show the prev page with the content to display
        /// </summary>
        /// <param name="contentToDisplay"></param>
        public void ShowPreviousPage(string contentToDisplay)
        {
            
        }
    }

}
