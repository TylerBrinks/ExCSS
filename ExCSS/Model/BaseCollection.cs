using System;
using System.Collections;
using System.Collections.Generic;

namespace ExCSS.Model
{
    public abstract class BaseCollection<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}