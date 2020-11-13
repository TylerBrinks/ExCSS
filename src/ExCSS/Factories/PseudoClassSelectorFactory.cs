using System;
using System.Collections.Generic;

namespace ExCSS
{
    public sealed class PseudoClassSelectorFactory
    {
        private static readonly Lazy<PseudoClassSelectorFactory> Lazy =
            new Lazy<PseudoClassSelectorFactory>(() =>
                {
                    var factory = new PseudoClassSelectorFactory();
                    Selectors.Add(PseudoElementNames.Before, PseudoElementSelectorFactory.Instance.Create(PseudoElementNames.Before));
                    Selectors.Add(PseudoElementNames.After, PseudoElementSelectorFactory.Instance.Create(PseudoElementNames.After));
                    Selectors.Add(PseudoElementNames.FirstLine, PseudoElementSelectorFactory.Instance.Create(PseudoElementNames.FirstLine));
                    Selectors.Add(PseudoElementNames.FirstLetter, PseudoElementSelectorFactory.Instance.Create(PseudoElementNames.FirstLetter));
                    return factory;
                }
            );

        internal static PseudoClassSelectorFactory Instance => Lazy.Value;

        #region Selectors
        private static readonly Dictionary<string, ISelector> Selectors =
            new Dictionary<string, ISelector>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    PseudoClassNames.Root,
                    SimpleSelector.PseudoClass( PseudoClassNames.Root)
                },
                {
                    PseudoClassNames.Scope,
                    SimpleSelector.PseudoClass(PseudoClassNames.Scope)
                },
                {
                    PseudoClassNames.OnlyType,
                    SimpleSelector.PseudoClass( PseudoClassNames.OnlyType)
                },
                {
                    PseudoClassNames.FirstOfType,
                    SimpleSelector.PseudoClass( PseudoClassNames.FirstOfType)
                },
                {
                    PseudoClassNames.LastOfType,
                    SimpleSelector.PseudoClass(  PseudoClassNames.LastOfType)
                },
                {
                    PseudoClassNames.OnlyChild,
                    SimpleSelector.PseudoClass( PseudoClassNames.OnlyChild)
                },
                {
                    PseudoClassNames.FirstChild,
                    SimpleSelector.PseudoClass( PseudoClassNames.FirstChild)
                },
                {
                    PseudoClassNames.LastChild,
                    SimpleSelector.PseudoClass(PseudoClassNames.LastChild)
                },
                {
                    PseudoClassNames.Empty,
                    SimpleSelector.PseudoClass(PseudoClassNames.Empty)
                },
                {
                    PseudoClassNames.AnyLink,
                    SimpleSelector.PseudoClass(  PseudoClassNames.AnyLink)
                },
                {
                    PseudoClassNames.Link, 
                    SimpleSelector.PseudoClass(  PseudoClassNames.Link)},
                {
                    PseudoClassNames.Visited,
                    SimpleSelector.PseudoClass(  PseudoClassNames.Visited)
                },
                {
                    PseudoClassNames.Active,
                    SimpleSelector.PseudoClass( PseudoClassNames.Active)
                },
                {
                    PseudoClassNames.Hover, 
                    SimpleSelector.PseudoClass( PseudoClassNames.Hover)
                },
                {
                    PseudoClassNames.Focus, 
                    SimpleSelector.PseudoClass( PseudoClassNames.Focus)
                },
                {
                    PseudoClassNames.Target, 
                    SimpleSelector.PseudoClass( PseudoClassNames.Target)
                },
                {
                    PseudoClassNames.Enabled,
                    SimpleSelector.PseudoClass( PseudoClassNames.Enabled)
                },
                {
                    PseudoClassNames.Disabled,
                    SimpleSelector.PseudoClass( PseudoClassNames.Disabled)
                },
                {
                    PseudoClassNames.Default,
                    SimpleSelector.PseudoClass( PseudoClassNames.Default)
                },
                {
                    PseudoClassNames.Checked,
                    SimpleSelector.PseudoClass( PseudoClassNames.Checked)
                },
                {
                    PseudoClassNames.Indeterminate,
                    SimpleSelector.PseudoClass(  PseudoClassNames.Indeterminate)
                },
                {
                    PseudoClassNames.PlaceholderShown,
                    SimpleSelector.PseudoClass(  PseudoClassNames.PlaceholderShown)
                },
                {
                    PseudoClassNames.Unchecked,
                    SimpleSelector.PseudoClass( PseudoClassNames.Unchecked)
                },
                {
                    PseudoClassNames.Valid, 
                    SimpleSelector.PseudoClass( PseudoClassNames.Valid)
                },
                {
                    PseudoClassNames.Invalid,
                    SimpleSelector.PseudoClass(  PseudoClassNames.Invalid)
                },
                {
                    PseudoClassNames.Required,
                    SimpleSelector.PseudoClass( PseudoClassNames.Required)
                },
                {
                    PseudoClassNames.ReadOnly,
                    SimpleSelector.PseudoClass( PseudoClassNames.ReadOnly)
                },
                {
                    PseudoClassNames.ReadWrite,
                    SimpleSelector.PseudoClass( PseudoClassNames.ReadWrite)
                },
                {
                    PseudoClassNames.InRange,
                    SimpleSelector.PseudoClass( PseudoClassNames.InRange)
                },
                {
                    PseudoClassNames.OutOfRange,
                    SimpleSelector.PseudoClass(  PseudoClassNames.OutOfRange)
                },
                {
                    PseudoClassNames.Optional,
                    SimpleSelector.PseudoClass( PseudoClassNames.Optional)
                },
                {
                    PseudoClassNames.Shadow, 
                    SimpleSelector.PseudoClass( PseudoClassNames.Shadow)
                },
            };
        #endregion

        public ISelector Create(string name)
        {
            if (Selectors.TryGetValue(name, out ISelector selector))
            {
                return selector;
            }

            return null;
        }
    }
}