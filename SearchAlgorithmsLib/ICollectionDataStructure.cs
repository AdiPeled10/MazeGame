using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
