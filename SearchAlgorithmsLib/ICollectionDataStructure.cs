
namespace SearchAlgorithmsLib
{
    public interface ICollectionDataStructure<T>
    {
        void Add(T element);
        bool Remove(T element);
        T PopFirst();
        int Count { get; }
        // deletes everything in the structure without unnecessary reduce of the capacity
        // i.e. delete everything in an array without shrinking the array
        void Clear();
    }
}
