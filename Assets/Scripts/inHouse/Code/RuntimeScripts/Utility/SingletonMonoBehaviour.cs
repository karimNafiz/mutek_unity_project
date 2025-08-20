using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Abstract base class to implement a singleton pattern for MonoBehaviour-derived classes.
    /// Ensures only one instance of the component exists in the scene.
    /// </summary>
    /// <typeparam name="T">Type of the MonoBehaviour to make a singleton.</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        /// <summary>
        /// Singleton instance of the type <typeparamref name="T"/>.
        /// </summary>
        public static T Instance => instance;

        /// <summary>
        /// Assigns the instance or destroys the duplicate on Awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
