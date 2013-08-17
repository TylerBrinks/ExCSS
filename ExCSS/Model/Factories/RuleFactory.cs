using System.Collections.Generic;
using ExCSS.Model.Extensions;

namespace ExCSS.Model.Factories
{
    internal abstract class RuleFactory : IRuleFactory
    {
        public StyleSheetContext Context;

        internal RuleFactory(StyleSheetContext context)
        {
            Context = context;
        }

        public abstract void Parse(IEnumerator<Block> reader);

        internal static Selector ParseSelector(string selector)
        {
            var parser = new Parser(selector);

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
            var parser = new Parser(declarations);

            var enumerator = parser.Lexer.Tokens.GetEnumerator();
            var declaration = new StyleDeclaration();
            
            enumerator.AppendDeclarations(declaration.List);
            
            return declaration;
        }
    }
}
