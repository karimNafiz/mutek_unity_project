namespace Appearable
{
    /// <summary>
    /// Defines a contract for UI or game objects that can be shown or hidden,
    /// normally with a fade-in/out or similar effects.
    /// potentially with a delay before appearing.
    /// </summary>
    public interface IAppearable
    {
        /// <summary>
        /// Activates the visual the object with an optional start delay.
        /// </summary>
        /// <param name="startDelay">Time in seconds before the object appears.</param>
        public void Show(float startDelay = 0f);

        /// <summary>
        /// Hides the visual of the object.
        /// </summary>
        public void Hide();
        
        public bool GetActiveState();
    }
}
