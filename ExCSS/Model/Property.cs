
namespace ExCSS.Model
{
    public sealed class Property
    {
        private Value _value;
        private bool _important;
        
        internal Property(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public Value Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Important
        {
            get { return _important; }
            set { _important = value; }
        }

        public override string ToString()
        {
            var value = Name + ":" + _value;

            if (_important)
            {
                value += " !important";
            }

            return value;
        }
    }
}
