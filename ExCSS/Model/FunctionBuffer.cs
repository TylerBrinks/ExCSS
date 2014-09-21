using System;
using System.Collections.Generic;

namespace ExCSS.Model
{
    internal class FunctionBuffer
    {
        private readonly string _function;
        private readonly List<Term> _termList;
        private Term _term;

        internal FunctionBuffer(string function)
        {
            _termList = new List<Term>();
            _function = function;
        }

        public List<Term> TermList
        {
            get { return _termList; }
        }

        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }

        public void Include()
        {
            if (_term != null)
            {
                _termList.Add(_term);
            }

            _term = null;
        }

        public Term Done()
        {
            Include();
            return BuildFunctionTerm(_function, _termList);
        }

        private Term BuildFunctionTerm(string name, List<Term> terms)
        {
            switch (name)
            {
                case "rgb":
                    {
                        if (terms.Count == 5)
                        {
                            if (CheckNumber(terms[0]) &&
                                CheckNumber(terms[2]) &&
                                CheckNumber(terms[4]))
                            {
                                return HtmlColor.FromRgb(
                                    ToByte(terms[0]),
                                    ToByte(terms[2]),
                                    ToByte(terms[4]));
                            }
                        }

                        break;
                    }
                case "rgba":
                    {
                        if (terms.Count == 7)
                        {
                            if (CheckNumber(terms[0]) &&
                                CheckNumber(terms[2]) &&
                                CheckNumber(terms[4]) &&
                                CheckNumber(terms[6]))
                            {
                                return HtmlColor.FromRgba(
                                    ToByte(terms[0]),
                                    ToByte(terms[2]),
                                    ToByte(terms[4]),
                                    ToSingle(terms[6]));
                            }
                        }

                        break;
                    }
                case "hsl":
                    {
                        if (_termList.Count == 5)
                        {
                            if (CheckNumber(terms[0]) &&
                                CheckPercentage(terms[2]) &&
                                CheckPercentage(terms[4]))
                            {
                                return HtmlColor.FromHsl(
                                    ToSingle(terms[0]),
                                    ToSingle(terms[2], UnitType.Percentage),
                                    ToSingle(terms[4], UnitType.Percentage));
                            }
                        }

                        break;
                    }
            }

            return new GenericFunction(name, terms);
        }

        private static bool CheckNumber(Term cssValue)
        {
            return (cssValue is PrimitiveTerm && 
                    ((PrimitiveTerm)cssValue).PrimitiveType == UnitType.Number);
        }

        private static bool CheckPercentage(Term cssValue)
        {
            return (cssValue is PrimitiveTerm &&
                    (((PrimitiveTerm)cssValue).PrimitiveType == UnitType.Number || 
                    ((PrimitiveTerm)cssValue).PrimitiveType == UnitType.Percentage));
        }

        private static Single ToSingle(Term cssValue, UnitType asType = UnitType.Number)
        {
            var value = ((PrimitiveTerm)cssValue).GetFloatValue(asType);
                
            return value.HasValue 
                ? value.Value 
                : 0f;
        }

        private static byte ToByte(Single? value)
        {
            if (value.HasValue)
            {
                return (byte)Math.Min(Math.Max(value.Value, 0), 255);
            }

            return 0;
        }

        private static byte ToByte(Term cssValue)
        {
            return ToByte(((PrimitiveTerm)cssValue).GetFloatValue(UnitType.Number));
        }
    }
}