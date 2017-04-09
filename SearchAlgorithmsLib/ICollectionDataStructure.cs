
namespace SearchAlgorithmsLib
{
    public interface ICollectionDataStructure<T>
    {
        void Add(T element);
        bool Remove(T element);
        T PopFirst();
        int Count { get; }
    }
}
