using System;

namespace ExCSS.Model
{
    public sealed class Property
    {
        string _name;
        Value _value;
        bool _important;
        internal Property(string name)
        {
            _name = name;
        }


        public string Name
        {
            get { return _name; }
        }


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
            var value = _name + ":" + _value;

            if (_important)
            {
                value += " !important";
            }

            return value;
        }
    }
}
