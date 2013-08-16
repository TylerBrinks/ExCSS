using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ExCSS.Model
{
    public sealed class RuleList : List<Ruleset>
    {
        //readonly List<Ruleset> _rules;
        
        //internal RuleList()
        //{
        //    _rules = new List<Ruleset>();
        //}


        //public int Length
        //{
        //    get { return _rules.Count; }
        //}

        //public Ruleset this[int index]
        //{
        //    get { return index >= 0 && index < _rules.Count ? _rules[index] : null; }
        //}

        //internal List<Ruleset> List
        //{
        //    get { return _rules; }
        //}

        //#region Internal methods

        //internal void RemoveAt(int index)
        //{
        //    _rules.RemoveAt(index);
        //}

        //internal void InsertAt(int index, Ruleset rule)
        //{
        //    _rules.Insert(index, rule);
        //}

        //#endregion

        //#region Implemented interface

      
        //public IEnumerator<Ruleset> GetEnumerator()
        //{
        //    return ((IEnumerable<Ruleset>) _rules).GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //#endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var t in this)
            {
                sb.AppendLine(t.ToString());
            }

            return sb.ToString();
        }
    }
}
