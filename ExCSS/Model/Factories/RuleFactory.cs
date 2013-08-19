using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

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

        internal static SimpleSelector ParseSelector(string selector)
        {
            //var parser = new Parser(selector);
            var lexer = new Lexer(new StylesheetStreamReader(selector));//.Tokens.GetEnumerator()
            var tokens = lexer.Tokens.GetEnumerator();
            var ctor = new SelectorConstructor();

            while (tokens.MoveNext())
            {
                ctor.AssignSelector(tokens);
            }

            return ctor.Result;
        }

        internal static StyleDeclaration ParseDeclarations(string declarations, bool quirksMode = false)
        {
            //var parser = new Parser(declarations);
            var lexer = new Lexer(new StylesheetStreamReader(declarations));
            var enumerator = lexer.Tokens.GetEnumerator();
            var declaration = new StyleDeclaration();
            
            enumerator.AppendDeclarations(declaration.Properties);
            
            return declaration;
        }
    }
}
