using EventHandling;
using ScriptableObjects;

namespace EventHandling.Events
{
    /// <summary>Raised when the player holds a book up (becomes actively reading/visible).</summary>
    public readonly struct OnBookHeldUp : IEvent
    {
        public readonly SO_FlipbookPageCollection Book;
        public OnBookHeldUp(SO_FlipbookPageCollection book) { Book = book; }
    }

    /// <summary>Raised when the player puts the book down (no longer visible/reading).</summary>
    public readonly struct OnBookPutDown : IEvent
    {
        public readonly SO_FlipbookPageCollection Book;
        public OnBookPutDown(SO_FlipbookPageCollection book) { Book = book; }
    }

    /// <summary>Raised when the player changes the active book.</summary>
    public readonly struct OnBookChanged : IEvent
    {
        public readonly SO_FlipbookPageCollection PreviousBook;
        public readonly SO_FlipbookPageCollection NewBook;

        public OnBookChanged(SO_FlipbookPageCollection previousBook, SO_FlipbookPageCollection newBook)
        {
            PreviousBook = previousBook;
            NewBook = newBook;
        }
    }
}
