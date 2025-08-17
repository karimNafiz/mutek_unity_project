using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// Holds a list of SO_FlipbookPageContent to make up a collection of pages. 
    /// </summary>
    [CreateAssetMenu(fileName = "SO_FlipbookPageCollection", menuName = "Scriptable Objects/Flipbook/SO_FlipbookPageCollection")]
    public class SO_FlipbookPageCollection : ScriptableObject
    {
        [field: SerializeField]
        public SO_FlipbookPageContent[] PageCollection { get; set; }
        
        /// <summary>
        /// Keeps track of the current page index.
        /// </summary>
        [SerializeField] int _currentPageIndex = 0;
        
        /// <summary>
        /// Internal helper method to check if the current page index is valid.
        /// </summary>
        /// <returns></returns>
        private bool IsIndexValid(int index = -1)
        {
            return index >= 0 && index < PageCollection.Length;
        }

        /// <summary>
        /// Initialize the flipbook with a specific page index
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public void InitializeBook(int pageIndex)
        {
            _currentPageIndex = pageIndex;
        }

        /// <summary>
        /// Query the current page index of this page collection
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPageIndex()
        {
            return _currentPageIndex;
        }
        
        /// <summary>
        /// Query the current page of this page collection.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>true if the requested page exists. false otherwise</returns>
        public bool GetCurrentPage(out SO_FlipbookPageContent currentPage)
        {
            if (IsIndexValid(_currentPageIndex))
            {
                currentPage = PageCollection[_currentPageIndex];
                return true;
            }
            else
            {
                Debug.LogError("Request to CURRENT page failed, check current page index");
                currentPage = null;
                return false;
            }
        }

        /// <summary>
        /// Query the next page of this page collection if it exists.
        /// </summary>
        /// <param name="nextPage"></param>
        /// <returns>true if the requested page exists. false otherwise</returns>
        public bool GetNextPage(out SO_FlipbookPageContent nextPage)
        {
            if (IsIndexValid(_currentPageIndex + 1))
            {
                nextPage = PageCollection[_currentPageIndex + 1];
                _currentPageIndex++;
                return true;
            }
            else
            {
                Debug.LogWarning("Already at the LAST page, request to NEXT page failed");
                nextPage = null;
                return false;
            }
        }

        /// <summary>
        /// Query the previous page of this page collection if it exists.
        /// </summary>
        /// <param name="prevPage"></param>
        /// <returns>true if the requested page exists. false otherwise</returns>
        public bool GetPrevPage(out SO_FlipbookPageContent prevPage)
        {
            if (IsIndexValid(_currentPageIndex - 1))
            {
                prevPage = PageCollection[_currentPageIndex - 1];
                _currentPageIndex--;
                return true;
            }
            else
            {
                prevPage = null;
                Debug.LogWarning("Already at the FIRST page, request to PREV page failed");
                return false;
            }
        }
        
        /*
         * Possible Extension: Go to a specific page.
         */
    }
}
