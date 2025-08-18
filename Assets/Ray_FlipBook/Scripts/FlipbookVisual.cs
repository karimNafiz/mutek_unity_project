using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace Flipbook
{
    [RequireComponent(typeof(Animator))]
    public class FlipbookVisual : MonoBehaviour
    {
        enum PageFlipDirection { Next, Previous }
        
        /// <summary>
        /// A serializeable struct to store a page's transform and text field
        /// </summary>
        [Serializable]
        private struct PageDisplay
        {
            public Transform pageTransform;
            public TextMeshProUGUI contentTextGUI;
        }
        
        [SerializeField] private float pageFlipDuration = 1.3f;
        [SerializeField] private Vector3 originalPageLocalRotation = new Vector3(0f, 180f, 0f);
        
        // const string trigger names for Animator
        private const string HOLD_UP_TRIGGER = "Up";
        private const string HOLD_DOWN_TRIGGER = "Down";
        private Animator _animator;

        
        // Array of page content structs to represent multiple flippable pages
        // Now using it as a double buffer display.
        [SerializeField] private PageDisplay[] _pageDisplay;
        private int _displayIndex = 0;
        
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Helper method to advance the display index based on the given delta.
        /// If the result is out of bounds, it will wrap around.
        /// </summary>
        /// <param name="delta">+1 = going to next page, -1 = going to previous page</param>
        private void AdvanceDisplayIndex(int delta)
        {
            _displayIndex += delta;
            
            if (_displayIndex < 0)
            {
                _displayIndex = _pageDisplay.Length - 1;
            }
            else if (_displayIndex >= _pageDisplay.Length)
            {
                _displayIndex = 0;
            }
        }

        /// <summary>
        /// Using DOTween to animate the flipbook page flip.
        /// </summary>
        /// <param name="target">The page transform to be flipped</param>
        /// <param name="direction"></param>
        /// <param name="onCompleteCallback"></param>
        private void PlayPageFlipAnimation(Transform target, PageFlipDirection direction, Action onCompleteCallback)
        {
            // handle the start and target x rotation for the pages depending on the flip direction
            // note that the magic numbers are tested for best visual result. 
            float startXRot = 0;
            float targetXRot = 0;
            if (direction == PageFlipDirection.Next)
            {
                startXRot = 0;
                targetXRot = -350f;
            }
            else if (direction == PageFlipDirection.Previous)
            {
                startXRot = 10f;
                targetXRot = 360f;
            }
            
            target.localRotation = Quaternion.Euler(startXRot, originalPageLocalRotation.y, originalPageLocalRotation.z);
            Vector3 targetRotation = new Vector3(targetXRot, originalPageLocalRotation.y, originalPageLocalRotation.z);
            
            target.DOLocalRotate(targetRotation, pageFlipDuration, RotateMode.FastBeyond360).
                OnComplete(() =>
                {
                    onCompleteCallback?.Invoke();
                    // reset target local rotation to make sure the page position is correct for the next display
                    target.localRotation = Quaternion.Euler(originalPageLocalRotation);
                }).
                SetEase(Ease.InOutCubic);
        }

        /// <summary>
        /// The helper coroutine to update the current page content after the given delay
        /// </summary>
        /// <param name="contentToDisplay"></param>
        /// <param name="index">The index given at the time when the coroutine is started.
        /// To avoid the situation where the index is different when the delay elapsed</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator UpdateCurrentPageCoroutine(string contentToDisplay, int index, float delay)
        {
            yield return new WaitForSeconds(delay);
            // make sure the current display index transform is active
            _pageDisplay[index].pageTransform.gameObject.SetActive(true);
            // update the text field
            _pageDisplay[index].contentTextGUI.text = contentToDisplay;
        }
        
        
        
        
        //////////////// PUBLIC METHODS /////////////////////////////////////////
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
        /// <param name="delay">delay in seconds before the page is updated</param>
        public void UpdateCurrentPage(string contentToDisplay, float delay = 0f)
        {
            // make sure the delay is within the range of the page flip duration
            delay = Mathf.Clamp(delay, 0f, pageFlipDuration);
            StartCoroutine(UpdateCurrentPageCoroutine(contentToDisplay, _displayIndex, delay));
        }
        
        /// <summary>
        /// Flip away the current page and
        /// Show the next page with the content to Display
        /// </summary>
        /// <param name="contentToDisplay"></param>
        public void ShowNextPage(string contentToDisplay)
        {
            // Flip away the current page
            Transform currentPageTransform = _pageDisplay[_displayIndex].pageTransform;
            PlayPageFlipAnimation(currentPageTransform, PageFlipDirection.Next, 
                () => { currentPageTransform.gameObject.SetActive(false); });
            
            // Advance index and show the next page with the delay = 10% of pageFlipDuration
            // delay is to make sure the new text does not overlap with the old text. 
            AdvanceDisplayIndex(1);
            UpdateCurrentPage(contentToDisplay, 0.1f * pageFlipDuration);
        }

        /// <summary>
        /// Flip in the prev page and
        /// show the prev page with the content to display
        /// </summary>
        /// <param name="contentToDisplay"></param>
        public void ShowPreviousPage(string contentToDisplay)
        {
            // keep an record of the current page
            Transform currentPageTransform = _pageDisplay[_displayIndex].pageTransform;
            // hide current page content at the last 20% of pageFlipDuration, 
            // so that the new text does not overlap with the old text.
            UpdateCurrentPage("", 0.8f * pageFlipDuration);
            
            // advance index to the previous page, then update its content
            AdvanceDisplayIndex(-1);
            Transform prevPageTransform = _pageDisplay[_displayIndex].pageTransform;
            UpdateCurrentPage(contentToDisplay);
            
            // flip in the previous page, and turn off the current page when the prev page is flipped in.
            PlayPageFlipAnimation(prevPageTransform, PageFlipDirection.Previous, 
                () => { currentPageTransform.gameObject.SetActive(false); });
            
        }
    }

}
