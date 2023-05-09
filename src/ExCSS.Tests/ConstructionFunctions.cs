namespace ExCSS.Tests
{
    using System.Collections.Generic;

    using ExCSS;

    public class CssConstructionFunctions
    {
        internal static Stylesheet ParseStyleSheet(string source,
             bool includeUnknownRules = false,
             bool includeUnknownDeclarations = false,
             bool tolerateInvalidSelectors = false,
             bool tolerateInvalidValues = false,
             bool tolerateInvalidConstraints = false,
             bool preserveComments = false,
             bool preserveDuplicateProperties = false)
        {
            var parser = new StylesheetParser(
                includeUnknownRules,
                includeUnknownDeclarations,
                tolerateInvalidSelectors,
                tolerateInvalidValues,
                tolerateInvalidConstraints,
                preserveComments,
                preserveDuplicateProperties);

            return parser.Parse(source);
        }


        internal static Rule ParseRule(string source)
        {
            var parser = new StylesheetParser();
            return parser.ParseRule(source);
        }

        internal static Property ParseDeclaration(string source,
             bool includeUnknownRules = false,
             bool includeUnknownDeclarations = false,
             bool tolerateInvalidSelectors = false,
             bool tolerateInvalidValues = false,
             bool tolerateInvalidConstraints = false,
             bool preserveComments = false)
        {
            var parser = new StylesheetParser(
                includeUnknownRules,
                includeUnknownDeclarations,
                tolerateInvalidSelectors,
                tolerateInvalidValues,
                tolerateInvalidConstraints,
                preserveComments);
            return parser.ParseDeclaration(source);
        }
        
        internal static TokenValue ParseValue(string source)
        {
            var parser = new StylesheetParser();
            return parser.ParseValue(source);
        }

        internal static StyleDeclaration ParseDeclarations(string declarations)
        {
            var parser = new StylesheetParser();
            var style = new StyleDeclaration(parser);
            style.Update(declarations);
            return style;
        }

        internal static KeyframeRule ParseKeyframeRule(string source)
        {
            var parser = new StylesheetParser();
            return parser.ParseKeyframeRule(source);
        }
    }
}
