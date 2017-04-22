
namespace SearchAlgorithmsLib
{
    /// <summary>
    /// We want our program to be as generic as possible, different Searchers are going to use different
    /// searching algorithms which may use different Data Structures, this interface represents a set of methods
    /// that are the basic methods of each data structure, all of our Searchers will expect each 
    /// of these abilities from the data structures that they are going to use. By using this
    /// interface we can use a Stack by the same code which we used to use a Priority Queue.
    /// </summary>
    /// <typeparam name="T">
    /// This is the type of the element that the data structure will hold.
    /// </typeparam>
    public interface ICollectionDataStructure<T>
    {
        /// <summary>
        /// This function will add an element to our data structure.
        /// </summary>
        /// <param name="element">
        /// The element we are going to add to this data structure.
        /// </param>
        void Add(T element);

        /// <summary>
        /// Remove an element from the data structure.
        /// </summary>
        /// <param name="element">
        /// The element we are going to remove.
        /// </param>
        /// <returns>
        /// Returns false in case it is not in the data structure.
        /// </returns>
        bool Remove(T element);

        /// <summary>
        /// Pop the element at the front.
        /// </summary>
        /// <returns>
        /// The element which is popped from the front.
        /// </returns>
        T PopFirst();

        /// <summary>
        /// A Count property which we will use to know the number of elements 
        /// in the data structure.
        /// </summary>
        /// <returns>
        /// Returns the number of elements in the data structure.
        /// </returns>
        int Count { get; }

        /// <summary>
        ///  Deletes everything in the structure without unnecessary reduce of the capacity
        /// i.e. delete everything in an array without shrinking the array.
        /// </summary>
        void Clear();
    }
}
