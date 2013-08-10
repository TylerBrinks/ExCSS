using System;
using ExCSS.Model;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents an @namespace rule.
    /// </summary>
    ////[DOM("NamespaceRule")]
    public sealed class NamespaceRule : Ruleset
    {
        #region Constants

        internal const string RuleName = "namespace";

        #endregion

        #region Members

        string _namespaceURI;
        string _prefix;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new @namespace rule.
        /// </summary>
        internal NamespaceRule()
        {
            _type = RuleType.Namespace;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a string containing the text of the URI of the given namespace.
        /// </summary>
        //[DOM("namespaceURI")]
        public string NamespaceURI
        {
            get { return _namespaceURI; }
            internal set { _namespaceURI = value; }
        }

        /// <summary>
        /// Gets a string with the name of the prefix associated to this namespace. If there is no such prefix, returns null.
        /// </summary>
        //[DOM("prefix")]
        public string Prefix
        {
            get { return _prefix; }
            internal set { _prefix = value; }
        }

        #endregion

       public override string ToString()
        {
            return String.Format("@namespace {0} '{1}';", _prefix, _namespaceURI);
        }
    }
}
