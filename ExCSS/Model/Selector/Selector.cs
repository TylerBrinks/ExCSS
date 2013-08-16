using System;

namespace ExCSS.Model
{
    public abstract class Selector
    {
        public abstract int Specifity
        {
            get;
        }

        //public abstract bool Match(Element element);
    }
}
