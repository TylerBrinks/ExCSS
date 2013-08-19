
namespace ExCSS.Model
{
    public sealed class Property
    {
        private Term _term;
        private bool _important;
        
        internal Property(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }

        public bool Important
        {
            get { return _important; }
            set { _important = value; }
        }

        public override string ToString()
        {
            var value = Name + ":" + _term;

            if (_important)
            {
                value += " !important";
            }

            return value;
        }
    }
}
