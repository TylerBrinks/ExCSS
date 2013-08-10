using System;

namespace ExCSS.Model
{
    public abstract class Selector
    {
        public abstract Int32 Specifity
        {
            get;
        }

        public abstract bool Match(Element element);
    }
}
