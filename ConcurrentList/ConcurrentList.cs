using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentList
{
    public class ConcurrentList<T> : IList<T>
    {
        private readonly List<T> _list;
        private readonly object _syncObject = new object();

        public ConcurrentList() 
        {
            _list = new List<T>();
        }

        public ConcurrentList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        T IList<T>.this[int index]
        {
            get
            {
                lock (_syncObject)
                {
                    return _list[index];
                }
            }
            set
            {

                lock (_syncObject)
                {
                    _list[index] = value;
                }
            }
        }

        int ICollection<T>.Count
        { 
            get 
            {
                lock (_syncObject)
                {
                    return _list.Count;
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                lock (_syncObject)
                {
                    return ((ICollection<T>)_list).IsReadOnly;
                }
            }
        }

        void ICollection<T>.Add(T item)
        {
            lock (_syncObject)
            {
                _list.Add(item);
            }
        }

        void ICollection<T>.Clear()
        {
            lock (_syncObject)
            {
                _list.Clear();
            }
        }

        bool ICollection<T>.Contains(T item)
        {
            lock (_syncObject)
            {
                return _list.Contains(item);
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncObject)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_syncObject)
            {
                /*
                для решения проблемы с вылетом exception при изменении версии листа будет просто возвращён Enumerator нового листа. см. проект ConsoleAppTest.
                не очень эфективно зато просто и работает.
                */
                return _list.ToList().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_syncObject)
            {
                return _list.GetEnumerator();
            }
        }

        int IList<T>.IndexOf(T item)
        {
            lock (_syncObject)
            {
                return _list.IndexOf(item);
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            lock (_syncObject)
            {
                _list.Insert(index, item);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            lock (_syncObject)
            {
                return _list.Remove(item);
            }
        }

        void IList<T>.RemoveAt(int index)
        {
            lock (_syncObject)
            {
                _list.RemoveAt(index);
            }
        }
    }
}
