using System.Collections.Generic;

namespace ExCSS.Model.Factories
{
    internal interface IRuleFactory
    {
        void Parse(IEnumerator<Block> source);
    }

    internal abstract class RuleFactory : IRuleFactory
    {
        public StyleSheetContext Context;

        internal RuleFactory(StyleSheetContext context)
        {
            Context = context;
        }

        public abstract void Parse(IEnumerator<Block> source);

        internal static Selector ParseSelector(string selector, bool quirksMode = false)
        {
            var parser = new Parser(selector)
                {
                    IsQuirksMode = quirksMode
                };

            var tokens = parser.Lexer.Tokens.GetEnumerator();
            var ctor = new SelectorConstructor();

            while (tokens.MoveNext())
            {
                ctor.PickSelector(tokens);
            }

            return ctor.Result;
        }

        internal static StyleDeclaration ParseDeclarations(string declarations, bool quirksMode = false)
        {
            var parser = new Parser(declarations)
                {
                    IsQuirksMode = quirksMode,
                    //_ignore = false
                };

            var it = parser.Lexer.Tokens.GetEnumerator();
            var decl = new StyleDeclaration();
            it.AppendDeclarations(decl.List);
            return decl;
        }
    }
}
