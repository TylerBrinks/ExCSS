using System;
using System.Collections.Generic;

namespace ExCSS
{
    public sealed class PseudoElementSelectorFactory
    {
        private static readonly Lazy<PseudoElementSelectorFactory> Lazy =
            new(() => new PseudoElementSelectorFactory());

        #region Selectors

        private readonly Dictionary<string, ISelector> _selectors =
            new(StringComparer.OrdinalIgnoreCase)
            {
                //TODO some lack implementation (selection, content, ...)
                // some implementations are dubious (first-line, first-letter, ...)
                {
                    PseudoElementNames.Before,
                    SimpleSelector.PseudoElement(PseudoElementNames.Before)
                },
                {
                    PseudoElementNames.After,
                    SimpleSelector.PseudoElement(PseudoElementNames.After)
                },
                {
                    PseudoElementNames.Selection,
                    SimpleSelector.PseudoElement(PseudoElementNames.Selection)
                },
                {
                    PseudoElementNames.FirstLine,
                    SimpleSelector.PseudoElement(PseudoElementNames.FirstLine)
                },
                {
                    PseudoElementNames.FirstLetter,
                    SimpleSelector.PseudoElement(PseudoElementNames.FirstLetter)
                },
                {
                    PseudoElementNames.Content,
                    SimpleSelector.PseudoElement(PseudoElementNames.Content)
                }
            };

        #endregion

        private PseudoElementSelectorFactory()
        {
        }

        internal static PseudoElementSelectorFactory Instance => Lazy.Value;

        public ISelector Create(string name)
        {
            return _selectors.TryGetValue(name, out var selector) ? selector : null;
        }
    }
}