using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    class ContainsList<T> : IEnumerable<T>
    {
        public ContainsList()
        {

        }

        public ContainsList(IEnumerable<T> items)
        {
            AddRange(items);
        }

        readonly Dictionary<T, object> _Dict = new Dictionary<T, object>();

        public void Append(T item)
        {
            _Dict.Add(item, null);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Append(item);
            }
        }

        public bool Contains(T item)
        {
            return _Dict.ContainsKey(item);
        }

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return _Dict.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Dict.Keys.GetEnumerator();
        }

        #endregion
    }
}
