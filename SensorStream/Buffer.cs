using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorStream
{
    public class Buffer
    {
        public class SlidingBuffer<T> : ObservableCollection<T>
        {
            private readonly Queue<T> _queue;
            private readonly int _maxCount;

            public new int Count => _queue.Count;

            public bool IsReadOnly
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void Clear()
            {
                _queue.Clear();
            }

            public bool Contains(T item) => _queue.Contains(item);

            

            public SlidingBuffer(int maxCount)
            {
                _maxCount = maxCount;
                _queue = new Queue<T>(maxCount);
            }

            public new void Add(T item)
            {
                if (_queue.Count == _maxCount)
                    _queue.Dequeue();
                _queue.Enqueue(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _queue.GetEnumerator();
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(T item)
            {
                throw new NotImplementedException();
            }
        }
    }
}
