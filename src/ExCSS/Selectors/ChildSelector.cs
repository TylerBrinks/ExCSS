using System.IO;

namespace ExCSS
{
    internal abstract class ChildSelector : StylesheetNode, ISelector
    {
        private readonly string _name;
        protected int _step;
        protected int _offset;
        protected ISelector _kind;

        protected ChildSelector(string name)
        {
            _name = name;
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            var a = _step.ToString();
            var b = string.Empty;

            if (_offset > 0)
            {
                b = "+" + _offset;
            }
            else if (_offset < 0)
            {
                b = _offset.ToString();
            }

            writer.Write(":{0}({1}n{2})", _name, a, b);
        }

        public Priority Specifity => Priority.OneClass;
        public string Text => this.ToCss();

        internal ChildSelector With(int step, int offset, ISelector kind)
        {
            _step = step;
            _offset = offset;
            _kind = kind;
            return this;
        }
    }
}