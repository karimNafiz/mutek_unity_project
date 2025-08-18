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
        [SerializeField] int currentPageIndex = 0;
        
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
            currentPageIndex = Mathf.Clamp(pageIndex, 0, PageCollection.Length - 1);
        }

        /// <summary>
        /// Query the current page index of this page collection
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPageIndex()
        {
            return currentPageIndex;
        }

        /// <summary>
        /// Query the total number of pages in this page collection
        /// </summary>
        /// <returns></returns>
        public int GetPageCount()
        {
            return PageCollection.Length;
        }
        
        /// <summary>
        /// Query the current page of this page collection.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>true if the requested page exists. false otherwise</returns>
        public bool TryGetCurrentPage(out SO_FlipbookPageContent currentPage)
        {
            if (IsIndexValid(currentPageIndex))
            {
                currentPage = PageCollection[currentPageIndex];
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
        public bool TryGetNextPage(out SO_FlipbookPageContent nextPage)
        {
            if (IsIndexValid(currentPageIndex + 1))
            {
                nextPage = PageCollection[currentPageIndex + 1];
                currentPageIndex++;
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
        public bool TryGetPrevPage(out SO_FlipbookPageContent prevPage)
        {
            if (IsIndexValid(currentPageIndex - 1))
            {
                prevPage = PageCollection[currentPageIndex - 1];
                currentPageIndex--;
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
