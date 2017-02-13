using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public sealed class StyleDeclaration : StylesheetNode, IProperties
    {
        readonly Rule _parent;
        readonly StylesheetParser _parser;
        public event Action<string> Changed;

        StyleDeclaration(Rule parent, StylesheetParser parser)
        {
            _parent = parent;
            _parser = parser;
        }

        internal StyleDeclaration(StylesheetParser parser) : this(null, parser)
        {
        }

        internal StyleDeclaration() : this(null, null)
        {
        }

        internal StyleDeclaration(Rule parent) : this(parent, parent.Parser)
        {
        }

        public void Update(string value)
        {
            Clear();

            if (!string.IsNullOrEmpty(value))
            {
                _parser.AppendDeclarations(this, value);
            }
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            var list = new List<string>();
            var serialized = new List<string>();
            foreach (var declaration in Declarations)
            {
                var property = declaration.Name;
                if (IsStrictMode)
                {
                    if (serialized.Contains(property))
                    {
                        continue;
                    }

                    var shorthands = PropertyFactory.Instance.GetShorthands(property).ToList();
                    if (shorthands.Any())
                    {
                        var longhands = Declarations.Where(m => !serialized.Contains(m.Name)).ToList();
                        foreach (var shorthand in shorthands.OrderByDescending(m =>
                            PropertyFactory.Instance.GetLonghands(m).Length))
                        {
                            var rule = PropertyFactory.Instance.CreateShorthand(shorthand);
                            var properties = PropertyFactory.Instance.GetLonghands(shorthand);
                            var currentLonghands = longhands.Where(m => properties.Contains(m.Name)).ToArray();

                            if (currentLonghands.Length == 0)
                            {
                                continue;
                            }

                            var important = currentLonghands.Count(m => m.IsImportant);

                            if (important > 0 && important != currentLonghands.Length)
                            {
                                continue;
                            }

                            if (properties.Length != currentLonghands.Length)
                            {
                                continue;
                            }

                            var value = rule.Stringify(currentLonghands);

                            if (string.IsNullOrEmpty(value))
                            {
                                continue;
                            }

                            list.Add(CompressedStyleFormatter.Instance.Declaration(shorthand, value, important != 0));

                            foreach (var longhand in currentLonghands)
                            {
                                serialized.Add(longhand.Name);
                                longhands.Remove(longhand);
                            }
                        }
                    }
                    if (serialized.Contains(property))
                    {
                        continue;
                    }
                    serialized.Add(property);
                }
                list.Add(declaration.ToCss(formatter));
            }
            writer.Write(formatter.Declarations(list));
        }

        public string RemoveProperty(string propertyName)
        { 
            var value = GetPropertyValue(propertyName);
            RemovePropertyByName(propertyName);
            RaiseChanged();

            return value;
        }

        void RemovePropertyByName(string propertyName)
        {
            foreach (var declaration in Declarations)
            {
                if (!declaration.Name.Is(propertyName))
                {
                    continue;
                }
                RemoveChild(declaration);
                break;
            }

            if (!IsStrictMode || !PropertyFactory.Instance.IsShorthand(propertyName))
            {
                return;
            }

            var longhands = PropertyFactory.Instance.GetLonghands(propertyName);
            foreach (var longhand in longhands)
            {
                RemovePropertyByName(longhand);
            }
        }

        public string GetPropertyPriority(string propertyName)
        {
            var property = GetProperty(propertyName);
            if (property != null && property.IsImportant)
            {
                return Keywords.Important;
            }
            if (!IsStrictMode || !PropertyFactory.Instance.IsShorthand(propertyName))
            {
                return string.Empty;
            }

            var longhands = PropertyFactory.Instance.GetLonghands(propertyName);

            return longhands.Any(longhand => !GetPropertyPriority(longhand)
                .Isi(Keywords.Important)) ? string.Empty : Keywords.Important;
        }

        public string GetPropertyValue(string propertyName)
        {
            var property = GetProperty(propertyName);
            if (property != null)
            {
                return property.Value;
            }

            if (!IsStrictMode || !PropertyFactory.Instance.IsShorthand(propertyName))
            {
                return string.Empty;
            }

            var shortHand = PropertyFactory.Instance.CreateShorthand(propertyName);
            var declarations = PropertyFactory.Instance.GetLonghands(propertyName);
            var properties = new List<Property>();

            foreach (var declaration in declarations)
            {
                property = GetProperty(declaration);
                if (property == null)
                {
                    return string.Empty;
                }
                properties.Add(property);
            }
            return shortHand.Stringify(properties.ToArray());
        }

        public void SetPropertyValue(string propertyName, string propertyValue)
        {
            SetProperty(propertyName, propertyValue);
        }

        public void SetPropertyPriority(string propertyName, string priority)
        {
            if (!string.IsNullOrEmpty(priority) && !priority.Isi(Keywords.Important))
            {
                return;
            }

            var important = !string.IsNullOrEmpty(priority);
            var mappings = IsStrictMode && PropertyFactory.Instance.IsShorthand(propertyName) ?
                PropertyFactory.Instance.GetLonghands(propertyName) :
                Enumerable.Repeat(propertyName, 1);

            foreach (var mapping in mappings)
            {
                var property = GetProperty(mapping);
                if (property != null)
                {
                    property.IsImportant = important;
                }
            }
        }

        public void SetProperty(string propertyName, string propertyValue, string priority = null)
        {
            if (!string.IsNullOrEmpty(propertyValue))
            {
                if (priority != null && !priority.Isi(Keywords.Important))
                {
                    return;
                }

                var value = _parser.ParseValue(propertyValue);
                if (value == null)
                {
                    return;
                }

                var property = CreateProperty(propertyName);
                if (property == null || !property.TrySetValue(value))
                {
                    return;
                }

                property.IsImportant = priority != null;
                SetProperty(property);
                RaiseChanged();
            }
            else
            {
                RemoveProperty(propertyName);
            }
        }

        internal Property CreateProperty(string propertyName)
        {
            var property = GetProperty(propertyName);
            if (property != null)
            {
                return property;
            }

            property = PropertyFactory.Instance.Create(propertyName);
            if (property != null || IsStrictMode)
            {
                return property;
            }

            return new UnknownProperty(propertyName);
        }

        internal Property GetProperty(string name)
        {
            return Declarations.FirstOrDefault(m => m.Name.Isi(name));
        }

        internal void SetProperty(Property property)
        {
            var shorthand = property as ShorthandProperty;
            if (shorthand != null)
            {
                SetShorthand(shorthand);
            }
            else
            {
                SetLonghand(property);
            }
        }

        internal void SetDeclarations(IEnumerable<Property> decls)
        {
            ChangeDeclarations(decls, m => false, (o, n) => !o.IsImportant || n.IsImportant);
        }

        internal void UpdateDeclarations(IEnumerable<Property> decls)
        {
            ChangeDeclarations(decls, m => !m.CanBeInherited, (o, n) => o.IsInherited);
        }

        void ChangeDeclarations(IEnumerable<Property> decls, Predicate<Property> defaultSkip, Func<Property, Property, Boolean> removeExisting)
        {
            var declarations = new List<Property>();
            foreach (var newdecl in decls)
            {
                var skip = defaultSkip(newdecl);
                foreach (var olddecl in Declarations)
                {
                    if (!olddecl.Name.Is(newdecl.Name))
                    {
                        continue;
                    }

                    if (removeExisting(olddecl, newdecl))
                    {
                        RemoveChild(olddecl);
                    }
                    else
                    {
                        skip = true;
                    }
                    break;
                }
                if (!skip)
                {
                    declarations.Add(newdecl);
                }
            }
            foreach (var declaration in declarations)
            {
                AppendChild(declaration);
            }
        }

        void SetLonghand(Property property)
        {
            foreach (var declaration in Declarations)
            {
                if (!declaration.Name.Is(property.Name)) { continue;}
                RemoveChild(declaration);
                break;
            }
            AppendChild(property);
        }

        private void SetShorthand(ShorthandProperty shorthand)
        {
            var properties = PropertyFactory.Instance.CreateLonghandsFor(shorthand.Name);
            shorthand.Export(properties);

            foreach (var property in properties)
            {
                SetLonghand(property);
            }
        }

        private void RaiseChanged()
        {
            Changed?.Invoke(CssText);
        }

        public IEnumerator<IProperty> GetEnumerator()
        {
            return Declarations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IRule Parent => _parent;
        public string this[int index] => Declarations.GetItemByIndex(index).Name;
        public string this[string name] => GetPropertyValue(name);
        public int Length => Declarations.Count();
        public bool IsStrictMode =>/* IsReadOnly ||*/ _parser.Options.IncludeUnknownDeclarations == false;
        //public bool IsReadOnly => _parser == null;
        public IEnumerable<Property> Declarations => Children.OfType<Property>();

        public string CssText
        {
            get { return this.ToCss(); }
            set { Update(value); RaiseChanged(); }
        }

        public string AlignContent
        {
            get { return GetPropertyValue(PropertyNames.AlignContent); }
            set { SetProperty(PropertyNames.AlignContent, value); }
        }

        public string AlignItems
        {
            get { return GetPropertyValue(PropertyNames.AlignItems); }
            set { SetProperty(PropertyNames.AlignItems, value); }
        }

        public string AlignSelf
        {
            get { return GetPropertyValue(PropertyNames.AlignSelf); }
            set { SetProperty(PropertyNames.AlignSelf, value); }
        }

        public string Accelerator
        {
            get { return GetPropertyValue(PropertyNames.Accelerator); }
            set { SetProperty(PropertyNames.Accelerator, value); }
        }

        public string AlignmentBaseline
        {
            get { return GetPropertyValue(PropertyNames.AlignBaseline); }
            set { SetProperty(PropertyNames.AlignBaseline, value); }
        }

        public string Animation
        {
            get { return GetPropertyValue(PropertyNames.Animation); }
            set { SetProperty(PropertyNames.Animation, value); }
        }

        public string AnimationDelay
        {
            get { return GetPropertyValue(PropertyNames.AnimationDelay); }
            set { SetProperty(PropertyNames.AnimationDelay, value); }
        }

        public string AnimationDirection
        {
            get { return GetPropertyValue(PropertyNames.AnimationDirection); }
            set { SetProperty(PropertyNames.AnimationDirection, value); }
        }

        public string AnimationDuration
        {
            get { return GetPropertyValue(PropertyNames.AnimationDuration); }
            set { SetProperty(PropertyNames.AnimationDuration, value); }
        }

        public string AnimationFillMode
        {
            get { return GetPropertyValue(PropertyNames.AnimationFillMode); }
            set { SetProperty(PropertyNames.AnimationFillMode, value); }
        }

        public string AnimationIterationCount
        {
            get { return GetPropertyValue(PropertyNames.AnimationIterationCount); }
            set { SetProperty(PropertyNames.AnimationIterationCount, value); }
        }

        public string AnimationName
        {
            get { return GetPropertyValue(PropertyNames.AnimationName); }
            set { SetProperty(PropertyNames.AnimationName, value); }
        }

        public string AnimationPlayState
        {
            get { return GetPropertyValue(PropertyNames.AnimationPlayState); }
            set { SetProperty(PropertyNames.AnimationPlayState, value); }
        }

        public string AnimationTimingFunction
        {
            get { return GetPropertyValue(PropertyNames.AnimationTimingFunction); }
            set { SetProperty(PropertyNames.AnimationTimingFunction, value); }
        }

        public string BackfaceVisibility
        {
            get { return GetPropertyValue(PropertyNames.BackfaceVisibility); }
            set { SetProperty(PropertyNames.BackfaceVisibility, value); }
        }

        public string Background
        {
            get { return GetPropertyValue(PropertyNames.Background); }
            set { SetProperty(PropertyNames.Background, value); }
        }

        public string BackgroundAttachment
        {
            get { return GetPropertyValue(PropertyNames.BackgroundAttachment); }
            set { SetProperty(PropertyNames.BackgroundAttachment, value); }
        }

        public string BackgroundClip
        {
            get { return GetPropertyValue(PropertyNames.BackgroundClip); }
            set { SetProperty(PropertyNames.BackgroundClip, value); }
        }

        public string BackgroundColor
        {
            get { return GetPropertyValue(PropertyNames.BackgroundColor); }
            set { SetProperty(PropertyNames.BackgroundColor, value); }
        }

        public string BackgroundImage
        {
            get { return GetPropertyValue(PropertyNames.BackgroundImage); }
            set { SetProperty(PropertyNames.BackgroundImage, value); }
        }

        public string BackgroundOrigin
        {
            get { return GetPropertyValue(PropertyNames.BackgroundOrigin); }
            set { SetProperty(PropertyNames.BackgroundOrigin, value); }
        }

        public string BackgroundPosition
        {
            get { return GetPropertyValue(PropertyNames.BackgroundPosition); }
            set { SetProperty(PropertyNames.BackgroundPosition, value); }
        }

        public string BackgroundPositionX
        {
            get { return GetPropertyValue(PropertyNames.BackgroundPositionX); }
            set { SetProperty(PropertyNames.BackgroundPositionX, value); }
        }

        public string BackgroundPositionY
        {
            get { return GetPropertyValue(PropertyNames.BackgroundPositionY); }
            set { SetProperty(PropertyNames.BackgroundPositionY, value); }
        }

        public string BackgroundRepeat
        {
            get { return GetPropertyValue(PropertyNames.BackgroundRepeat); }
            set { SetProperty(PropertyNames.BackgroundRepeat, value); }
        }

        public string BackgroundSize
        {
            get { return GetPropertyValue(PropertyNames.BackgroundSize); }
            set { SetProperty(PropertyNames.BackgroundSize, value); }
        }

        public string BaselineShift
        {
            get { return GetPropertyValue(PropertyNames.BaselineShift); }
            set { SetProperty(PropertyNames.BaselineShift, value); }
        }

        public string Behavior
        {
            get { return GetPropertyValue(PropertyNames.Behavior); }
            set { SetProperty(PropertyNames.Behavior, value); }
        }

        public string Bottom
        {
            get { return GetPropertyValue(PropertyNames.Bottom); }
            set { SetProperty(PropertyNames.Bottom, value); }
        }

        public string Border
        {
            get { return GetPropertyValue(PropertyNames.Border); }
            set { SetProperty(PropertyNames.Border, value); }
        }

        public string BorderBottom
        {
            get { return GetPropertyValue(PropertyNames.BorderBottom); }
            set { SetProperty(PropertyNames.BorderBottom, value); }
        }

        public string BorderBottomColor
        {
            get { return GetPropertyValue(PropertyNames.BorderBottomColor); }
            set { SetProperty(PropertyNames.BorderBottomColor, value); }
        }

        public string BorderBottomLeftRadius
        {
            get { return GetPropertyValue(PropertyNames.BorderBottomLeftRadius); }
            set { SetProperty(PropertyNames.BorderBottomLeftRadius, value); }
        }

        public string BorderBottomRightRadius
        {
            get { return GetPropertyValue(PropertyNames.BorderBottomRightRadius); }
            set { SetProperty(PropertyNames.BorderBottomRightRadius, value); }
        }

        public string BorderBottomStyle
        {
            get { return GetPropertyValue(PropertyNames.BorderBottomStyle); }
            set { SetProperty(PropertyNames.BorderBottomStyle, value); }
        }

        public string BorderBottomWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderBottomWidth); }
            set { SetProperty(PropertyNames.BorderBottomWidth, value); }
        }

        public string BorderCollapse
        {
            get { return GetPropertyValue(PropertyNames.BorderCollapse); }
            set { SetProperty(PropertyNames.BorderCollapse, value); }
        }

        public string BorderColor
        {
            get { return GetPropertyValue(PropertyNames.BorderColor); }
            set { SetProperty(PropertyNames.BorderColor, value); }
        }

        public string BorderImage
        {
            get { return GetPropertyValue(PropertyNames.BorderImage); }
            set { SetProperty(PropertyNames.BorderImage, value); }
        }

        public string BorderImageOutset
        {
            get { return GetPropertyValue(PropertyNames.BorderImageOutset); }
            set { SetProperty(PropertyNames.BorderImageOutset, value); }
        }

        public string BorderImageRepeat
        {
            get { return GetPropertyValue(PropertyNames.BorderImageRepeat); }
            set { SetProperty(PropertyNames.BorderImageRepeat, value); }
        }

        public string BorderImageSlice
        {
            get { return GetPropertyValue(PropertyNames.BorderImageSlice); }
            set { SetProperty(PropertyNames.BorderImageSlice, value); }
        }

        public string BorderImageSource
        {
            get { return GetPropertyValue(PropertyNames.BorderImageSource); }
            set { SetProperty(PropertyNames.BorderImageSource, value); }
        }

        public string BorderImageWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderImageWidth); }
            set { SetProperty(PropertyNames.BorderImageWidth, value); }
        }

        public string BorderLeft
        {
            get { return GetPropertyValue(PropertyNames.BorderLeft); }
            set { SetProperty(PropertyNames.BorderLeft, value); }
        }

        public string BorderLeftColor
        {
            get { return GetPropertyValue(PropertyNames.BorderLeftColor); }
            set { SetProperty(PropertyNames.BorderLeftColor, value); }
        }

        public string BorderLeftStyle
        {
            get { return GetPropertyValue(PropertyNames.BorderLeftStyle); }
            set { SetProperty(PropertyNames.BorderLeftStyle, value); }
        }

        public string BorderLeftWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderLeftWidth); }
            set { SetProperty(PropertyNames.BorderLeftWidth, value); }
        }

        public string BorderRadius
        {
            get { return GetPropertyValue(PropertyNames.BorderRadius); }
            set { SetProperty(PropertyNames.BorderRadius, value); }
        }

        public string BorderRight
        {
            get { return GetPropertyValue(PropertyNames.BorderRight); }
            set { SetProperty(PropertyNames.BorderRight, value); }
        }

        public string BorderRightColor
        {
            get { return GetPropertyValue(PropertyNames.BorderRightColor); }
            set { SetProperty(PropertyNames.BorderRightColor, value); }
        }

        public string BorderRightStyle
        {
            get { return GetPropertyValue(PropertyNames.BorderRightStyle); }
            set { SetProperty(PropertyNames.BorderRightStyle, value); }
        }

        public string BorderRightWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderRightWidth); }
            set { SetProperty(PropertyNames.BorderRightWidth, value); }
        }

        public string BorderSpacing
        {
            get { return GetPropertyValue(PropertyNames.BorderSpacing); }
            set { SetProperty(PropertyNames.BorderSpacing, value); }
        }

        public string BorderStyle
        {
            get { return GetPropertyValue(PropertyNames.BorderStyle); }
            set { SetProperty(PropertyNames.BorderStyle, value); }
        }

        public string BorderTop
        {
            get { return GetPropertyValue(PropertyNames.BorderTop); }
            set { SetProperty(PropertyNames.BorderTop, value); }
        }

        public string BorderTopColor
        {
            get { return GetPropertyValue(PropertyNames.BorderTopColor); }
            set { SetProperty(PropertyNames.BorderTopColor, value); }
        }

        public string BorderTopLeftRadius
        {
            get { return GetPropertyValue(PropertyNames.BorderTopLeftRadius); }
            set { SetProperty(PropertyNames.BorderTopLeftRadius, value); }
        }

        public string BorderTopRightRadius
        {
            get { return GetPropertyValue(PropertyNames.BorderTopRightRadius); }
            set { SetProperty(PropertyNames.BorderTopRightRadius, value); }
        }

        public string BorderTopStyle
        {
            get { return GetPropertyValue(PropertyNames.BorderTopStyle); }
            set { SetProperty(PropertyNames.BorderTopStyle, value); }
        }

        public string BorderTopWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderTopWidth); }
            set { SetProperty(PropertyNames.BorderTopWidth, value); }
        }

        public string BorderWidth
        {
            get { return GetPropertyValue(PropertyNames.BorderWidth); }
            set { SetProperty(PropertyNames.BorderWidth, value); }
        }

        public string BoxShadow
        {
            get { return GetPropertyValue(PropertyNames.BoxShadow); }
            set { SetProperty(PropertyNames.BoxShadow, value); }
        }

        public string BoxSizing
        {
            get { return GetPropertyValue(PropertyNames.BoxSizing); }
            set { SetProperty(PropertyNames.BoxSizing, value); }
        }

        public string BreakAfter
        {
            get { return GetPropertyValue(PropertyNames.BreakAfter); }
            set { SetProperty(PropertyNames.BreakAfter, value); }
        }

        public string BreakBefore
        {
            get { return GetPropertyValue(PropertyNames.BreakBefore); }
            set { SetProperty(PropertyNames.BreakBefore, value); }
        }

        public string BreakInside
        {
            get { return GetPropertyValue(PropertyNames.BreakInside); }
            set { SetProperty(PropertyNames.BreakInside, value); }
        }

        public string CaptionSide
        {
            get { return GetPropertyValue(PropertyNames.CaptionSide); }
            set { SetProperty(PropertyNames.CaptionSide, value); }
        }

        public string Clear
        {
            get { return GetPropertyValue(PropertyNames.Clear); }
            set { SetProperty(PropertyNames.Clear, value); }
        }

        public string Clip
        {
            get { return GetPropertyValue(PropertyNames.Clip); }
            set { SetProperty(PropertyNames.Clip, value); }
        }

        public string ClipBottom
        {
            get { return GetPropertyValue(PropertyNames.ClipBottom); }
            set { SetProperty(PropertyNames.ClipBottom, value); }
        }

        public string ClipLeft
        {
            get { return GetPropertyValue(PropertyNames.ClipLeft); }
            set { SetProperty(PropertyNames.ClipLeft, value); }
        }

        public string ClipPath
        {
            get { return GetPropertyValue(PropertyNames.ClipPath); }
            set { SetProperty(PropertyNames.ClipPath, value); }
        }

        public string ClipRight
        {
            get { return GetPropertyValue(PropertyNames.ClipRight); }
            set { SetProperty(PropertyNames.ClipRight, value); }
        }

        public string ClipRule
        {
            get { return GetPropertyValue(PropertyNames.ClipRule); }
            set { SetProperty(PropertyNames.ClipRule, value); }
        }

        public string ClipTop
        {
            get { return GetPropertyValue(PropertyNames.ClipTop); }
            set { SetProperty(PropertyNames.ClipTop, value); }
        }

        public string Color
        {
            get { return GetPropertyValue(PropertyNames.Color); }
            set { SetProperty(PropertyNames.Color, value); }
        }

        public string ColorInterpolationFilters
        {
            get { return GetPropertyValue(PropertyNames.ColorInterpolationFilters); }
            set { SetProperty(PropertyNames.ColorInterpolationFilters, value); }
        }

        public string ColumnCount
        {
            get { return GetPropertyValue(PropertyNames.ColumnCount); }
            set { SetProperty(PropertyNames.ColumnCount, value); }
        }

        public string ColumnFill
        {
            get { return GetPropertyValue(PropertyNames.ColumnFill); }
            set { SetProperty(PropertyNames.ColumnFill, value); }
        }

        public string ColumnGap
        {
            get { return GetPropertyValue(PropertyNames.ColumnGap); }
            set { SetProperty(PropertyNames.ColumnGap, value); }
        }

        public string ColumnRule
        {
            get { return GetPropertyValue(PropertyNames.ColumnRule); }
            set { SetProperty(PropertyNames.ColumnRule, value); }
        }

        public string ColumnRuleColor
        {
            get { return GetPropertyValue(PropertyNames.ColumnRuleColor); }
            set { SetProperty(PropertyNames.ColumnRuleColor, value); }
        }

        public string ColumnRuleStyle
        {
            get { return GetPropertyValue(PropertyNames.ColumnRuleStyle); }
            set { SetProperty(PropertyNames.ColumnRuleStyle, value); }
        }

        public string ColumnRuleWidth
        {
            get { return GetPropertyValue(PropertyNames.ColumnRuleWidth); }
            set { SetProperty(PropertyNames.ColumnRuleWidth, value); }
        }

        public string Columns
        {
            get { return GetPropertyValue(PropertyNames.Columns); }
            set { SetProperty(PropertyNames.Columns, value); }
        }

        public string ColumnSpan
        {
            get { return GetPropertyValue(PropertyNames.ColumnSpan); }
            set { SetProperty(PropertyNames.ColumnSpan, value); }
        }

        public string ColumnWidth
        {
            get { return GetPropertyValue(PropertyNames.ColumnWidth); }
            set { SetProperty(PropertyNames.ColumnWidth, value); }
        }

        public string Content
        {
            get { return GetPropertyValue(PropertyNames.Content); }
            set { SetProperty(PropertyNames.Content, value); }
        }

        public string CounterIncrement
        {
            get { return GetPropertyValue(PropertyNames.CounterIncrement); }
            set { SetProperty(PropertyNames.CounterIncrement, value); }
        }

        public string CounterReset
        {
            get { return GetPropertyValue(PropertyNames.CounterReset); }
            set { SetProperty(PropertyNames.CounterReset, value); }
        }

        public string Float
        {
            get { return GetPropertyValue(PropertyNames.Float); }
            set { SetProperty(PropertyNames.Float, value); }
        }

        public string Cursor
        {
            get { return GetPropertyValue(PropertyNames.Cursor); }
            set { SetProperty(PropertyNames.Cursor, value); }
        }

        public string Direction
        {
            get { return GetPropertyValue(PropertyNames.Direction); }
            set { SetProperty(PropertyNames.Direction, value); }
        }

        public string Display
        {
            get { return GetPropertyValue(PropertyNames.Display); }
            set { SetProperty(PropertyNames.Display, value); }
        }

        public string DominantBaseline
        {
            get { return GetPropertyValue(PropertyNames.DominantBaseline); }
            set { SetProperty(PropertyNames.DominantBaseline, value); }
        }

        public string EmptyCells
        {
            get { return GetPropertyValue(PropertyNames.EmptyCells); }
            set { SetProperty(PropertyNames.EmptyCells, value); }
        }

        public string EnableBackground
        {
            get { return GetPropertyValue(PropertyNames.EnableBackground); }
            set { SetProperty(PropertyNames.EnableBackground, value); }
        }

        public string Fill
        {
            get { return GetPropertyValue(PropertyNames.Fill); }
            set { SetProperty(PropertyNames.Fill, value); }
        }

        public string FillOpacity
        {
            get { return GetPropertyValue(PropertyNames.FillOpacity); }
            set { SetProperty(PropertyNames.FillOpacity, value); }
        }

        public string FillRule
        {
            get { return GetPropertyValue(PropertyNames.FillRule); }
            set { SetProperty(PropertyNames.FillRule, value); }
        }

        public string Filter
        {
            get { return GetPropertyValue(PropertyNames.Filter); }
            set { SetProperty(PropertyNames.Filter, value); }
        }

        public string Flex
        {
            get { return GetPropertyValue(PropertyNames.Flex); }
            set { SetProperty(PropertyNames.Flex, value); }
        }

        public string FlexBasis
        {
            get { return GetPropertyValue(PropertyNames.FlexBasis); }
            set { SetProperty(PropertyNames.FlexBasis, value); }
        }

        public string FlexDirection
        {
            get { return GetPropertyValue(PropertyNames.FlexDirection); }
            set { SetProperty(PropertyNames.FlexDirection, value); }
        }

        public string FlexFlow
        {
            get { return GetPropertyValue(PropertyNames.FlexFlow); }
            set { SetProperty(PropertyNames.FlexFlow, value); }
        }

        public string FlexGrow
        {
            get { return GetPropertyValue(PropertyNames.FlexGrow); }
            set { SetProperty(PropertyNames.FlexGrow, value); }
        }

        public string FlexShrink
        {
            get { return GetPropertyValue(PropertyNames.FlexShrink); }
            set { SetProperty(PropertyNames.FlexShrink, value); }
        }

        public string FlexWrap
        {
            get { return GetPropertyValue(PropertyNames.FlexWrap); }
            set { SetProperty(PropertyNames.FlexWrap, value); }
        }

        public string Font
        {
            get { return GetPropertyValue(PropertyNames.Font); }
            set { SetProperty(PropertyNames.Font, value); }
        }

        public string FontFamily
        {
            get { return GetPropertyValue(PropertyNames.FontFamily); }
            set { SetProperty(PropertyNames.FontFamily, value); }
        }

        public string FontFeatureSettings
        {
            get { return GetPropertyValue(PropertyNames.FontFeatureSettings); }
            set { SetProperty(PropertyNames.FontFeatureSettings, value); }
        }

        public string FontSize
        {
            get { return GetPropertyValue(PropertyNames.FontSize); }
            set { SetProperty(PropertyNames.FontSize, value); }
        }

        public string FontSizeAdjust
        {
            get { return GetPropertyValue(PropertyNames.FontSizeAdjust); }
            set { SetProperty(PropertyNames.FontSizeAdjust, value); }
        }

        public string FontStretch
        {
            get { return GetPropertyValue(PropertyNames.FontStretch); }
            set { SetProperty(PropertyNames.FontStretch, value); }
        }

        public string FontStyle
        {
            get { return GetPropertyValue(PropertyNames.FontStyle); }
            set { SetProperty(PropertyNames.FontStyle, value); }
        }

        public string FontVariant
        {
            get { return GetPropertyValue(PropertyNames.FontVariant); }
            set { SetProperty(PropertyNames.FontVariant, value); }
        }

        public string FontWeight
        {
            get { return GetPropertyValue(PropertyNames.FontWeight); }
            set { SetProperty(PropertyNames.FontWeight, value); }
        }

        public string GlyphOrientationHorizontal
        {
            get { return GetPropertyValue(PropertyNames.GlyphOrientationHorizontal); }
            set { SetProperty(PropertyNames.GlyphOrientationHorizontal, value); }
        }

        public string GlyphOrientationVertical
        {
            get { return GetPropertyValue(PropertyNames.GlyphOrientationVertical); }
            set { SetProperty(PropertyNames.GlyphOrientationVertical, value); }
        }

        public string Height
        {
            get { return GetPropertyValue(PropertyNames.Height); }
            set { SetProperty(PropertyNames.Height, value); }
        }

        public string ImeMode
        {
            get { return GetPropertyValue(PropertyNames.ImeMode); }
            set { SetProperty(PropertyNames.ImeMode, value); }
        }

        public string JustifyContent
        {
            get { return GetPropertyValue(PropertyNames.JustifyContent); }
            set { SetProperty(PropertyNames.JustifyContent, value); }
        }

        public string LayoutGrid
        {
            get { return GetPropertyValue(PropertyNames.LayoutGrid); }
            set { SetProperty(PropertyNames.LayoutGrid, value); }
        }

        public string LayoutGridChar
        {
            get { return GetPropertyValue(PropertyNames.LayoutGridChar); }
            set { SetProperty(PropertyNames.LayoutGridChar, value); }
        }

        public string LayoutGridLine
        {
            get { return GetPropertyValue(PropertyNames.LayoutGridLine); }
            set { SetProperty(PropertyNames.LayoutGridLine, value); }
        }

        public string LayoutGridMode
        {
            get { return GetPropertyValue(PropertyNames.LayoutGridMode); }
            set { SetProperty(PropertyNames.LayoutGridMode, value); }
        }

        public string LayoutGridType
        {
            get { return GetPropertyValue(PropertyNames.LayoutGridType); }
            set { SetProperty(PropertyNames.LayoutGridType, value); }
        }

        public string Left
        {
            get { return GetPropertyValue(PropertyNames.Left); }
            set { SetProperty(PropertyNames.Left, value); }
        }

        public string LetterSpacing
        {
            get { return GetPropertyValue(PropertyNames.LetterSpacing); }
            set { SetProperty(PropertyNames.LetterSpacing, value); }
        }

        public string LineHeight
        {
            get { return GetPropertyValue(PropertyNames.LineHeight); }
            set { SetProperty(PropertyNames.LineHeight, value); }
        }

        public string ListStyle
        {
            get { return GetPropertyValue(PropertyNames.ListStyle); }
            set { SetProperty(PropertyNames.ListStyle, value); }
        }

        public string ListStyleImage
        {
            get { return GetPropertyValue(PropertyNames.ListStyleImage); }
            set { SetProperty(PropertyNames.ListStyleImage, value); }
        }

        public string ListStylePosition
        {
            get { return GetPropertyValue(PropertyNames.ListStylePosition); }
            set { SetProperty(PropertyNames.ListStylePosition, value); }
        }

        public string ListStyleType
        {
            get { return GetPropertyValue(PropertyNames.ListStyleType); }
            set { SetProperty(PropertyNames.ListStyleType, value); }
        }

        public string Margin
        {
            get { return GetPropertyValue(PropertyNames.Margin); }
            set { SetProperty(PropertyNames.Margin, value); }
        }

        public string MarginBottom
        {
            get { return GetPropertyValue(PropertyNames.MarginBottom); }
            set { SetProperty(PropertyNames.MarginBottom, value); }
        }

        public string MarginLeft
        {
            get { return GetPropertyValue(PropertyNames.MarginLeft); }
            set { SetProperty(PropertyNames.MarginLeft, value); }
        }

        public string MarginRight
        {
            get { return GetPropertyValue(PropertyNames.MarginRight); }
            set { SetProperty(PropertyNames.MarginRight, value); }
        }

        public string MarginTop
        {
            get { return GetPropertyValue(PropertyNames.MarginTop); }
            set { SetProperty(PropertyNames.MarginTop, value); }
        }

        public string Marker
        {
            get { return GetPropertyValue(PropertyNames.Marker); }
            set { SetProperty(PropertyNames.Marker, value); }
        }

        public string MarkerEnd
        {
            get { return GetPropertyValue(PropertyNames.MarkerEnd); }
            set { SetProperty(PropertyNames.MarkerEnd, value); }
        }

        public string MarkerMid
        {
            get { return GetPropertyValue(PropertyNames.MarkerMid); }
            set { SetProperty(PropertyNames.MarkerMid, value); }
        }

        public string MarkerStart
        {
            get { return GetPropertyValue(PropertyNames.MarkerStart); }
            set { SetProperty(PropertyNames.MarkerStart, value); }
        }

        public string Mask
        {
            get { return GetPropertyValue(PropertyNames.Mask); }
            set { SetProperty(PropertyNames.Mask, value); }
        }

        public string MaxHeight
        {
            get { return GetPropertyValue(PropertyNames.MaxHeight); }
            set { SetProperty(PropertyNames.MaxHeight, value); }
        }

        public string MaxWidth
        {
            get { return GetPropertyValue(PropertyNames.MaxWidth); }
            set { SetProperty(PropertyNames.MaxWidth, value); }
        }

        public string MinHeight
        {
            get { return GetPropertyValue(PropertyNames.MinHeight); }
            set { SetProperty(PropertyNames.MinHeight, value); }
        }

        public string MinWidth
        {
            get { return GetPropertyValue(PropertyNames.MinWidth); }
            set { SetProperty(PropertyNames.MinWidth, value); }
        }

        public string Opacity
        {
            get { return GetPropertyValue(PropertyNames.Opacity); }
            set { SetProperty(PropertyNames.Opacity, value); }
        }

        public string Order
        {
            get { return GetPropertyValue(PropertyNames.Order); }
            set { SetProperty(PropertyNames.Order, value); }
        }

        public string Orphans
        {
            get { return GetPropertyValue(PropertyNames.Orphans); }
            set { SetProperty(PropertyNames.Orphans, value); }
        }

        public string Outline
        {
            get { return GetPropertyValue(PropertyNames.Outline); }
            set { SetProperty(PropertyNames.Outline, value); }
        }

        public string OutlineColor
        {
            get { return GetPropertyValue(PropertyNames.OutlineColor); }
            set { SetProperty(PropertyNames.OutlineColor, value); }
        }

        public string OutlineStyle
        {
            get { return GetPropertyValue(PropertyNames.OutlineStyle); }
            set { SetProperty(PropertyNames.OutlineStyle, value); }
        }

        public string OutlineWidth
        {
            get { return GetPropertyValue(PropertyNames.OutlineWidth); }
            set { SetProperty(PropertyNames.OutlineWidth, value); }
        }

        public string Overflow
        {
            get { return GetPropertyValue(PropertyNames.Overflow); }
            set { SetProperty(PropertyNames.Overflow, value); }
        }

        public string OverflowX
        {
            get { return GetPropertyValue(PropertyNames.OverflowX); }
            set { SetProperty(PropertyNames.OverflowX, value); }
        }

        public string OverflowY
        {
            get { return GetPropertyValue(PropertyNames.OverflowY); }
            set { SetProperty(PropertyNames.OverflowY, value); }
        }

        public string OverflowWrap
        {
            get { return GetPropertyValue(PropertyNames.WordWrap); }
            set { SetProperty(PropertyNames.WordWrap, value); }
        }

        public string Padding
        {
            get { return GetPropertyValue(PropertyNames.Padding); }
            set { SetProperty(PropertyNames.Padding, value); }
        }

        public string PaddingBottom
        {
            get { return GetPropertyValue(PropertyNames.PaddingBottom); }
            set { SetProperty(PropertyNames.PaddingBottom, value); }
        }

        public string PaddingLeft
        {
            get { return GetPropertyValue(PropertyNames.PaddingLeft); }
            set { SetProperty(PropertyNames.PaddingLeft, value); }
        }

        public string PaddingRight
        {
            get { return GetPropertyValue(PropertyNames.PaddingRight); }
            set { SetProperty(PropertyNames.PaddingRight, value); }
        }

        public string PaddingTop
        {
            get { return GetPropertyValue(PropertyNames.PaddingTop); }
            set { SetProperty(PropertyNames.PaddingTop, value); }
        }

        public string PageBreakAfter
        {
            get { return GetPropertyValue(PropertyNames.PageBreakAfter); }
            set { SetProperty(PropertyNames.PageBreakAfter, value); }
        }

        public string PageBreakBefore
        {
            get { return GetPropertyValue(PropertyNames.PageBreakBefore); }
            set { SetProperty(PropertyNames.PageBreakBefore, value); }
        }

        public string PageBreakInside
        {
            get { return GetPropertyValue(PropertyNames.PageBreakInside); }
            set { SetProperty(PropertyNames.PageBreakInside, value); }
        }

        public string Perspective
        {
            get { return GetPropertyValue(PropertyNames.Perspective); }
            set { SetProperty(PropertyNames.Perspective, value); }
        }

        public string PerspectiveOrigin
        {
            get { return GetPropertyValue(PropertyNames.PerspectiveOrigin); }
            set { SetProperty(PropertyNames.PerspectiveOrigin, value); }
        }

        public string PointerEvents
        {
            get { return GetPropertyValue(PropertyNames.PointerEvents); }
            set { SetProperty(PropertyNames.PointerEvents, value); }
        }

        public string Quotes
        {
            get { return GetPropertyValue(PropertyNames.Quotes); }
            set { SetProperty(PropertyNames.Quotes, value); }
        }

        public string Position
        {
            get { return GetPropertyValue(PropertyNames.Position); }
            set { SetProperty(PropertyNames.Position, value); }
        }

        public string Right
        {
            get { return GetPropertyValue(PropertyNames.Right); }
            set { SetProperty(PropertyNames.Right, value); }
        }

        public string RubyAlign
        {
            get { return GetPropertyValue(PropertyNames.RubyAlign); }
            set { SetProperty(PropertyNames.RubyAlign, value); }
        }

        public string RubyOverhang
        {
            get { return GetPropertyValue(PropertyNames.RubyOverhang); }
            set { SetProperty(PropertyNames.RubyOverhang, value); }
        }

        public string RubyPosition
        {
            get { return GetPropertyValue(PropertyNames.RubyPosition); }
            set { SetProperty(PropertyNames.RubyPosition, value); }
        }

        public string Scrollbar3DLightColor
        {
            get { return GetPropertyValue(PropertyNames.Scrollbar3dLightColor); }
            set { SetProperty(PropertyNames.Scrollbar3dLightColor, value); }
        }

        public string ScrollbarArrowColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarArrowColor); }
            set { SetProperty(PropertyNames.ScrollbarArrowColor, value); }
        }

        public string ScrollbarDarkShadowColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarDarkShadowColor); }
            set { SetProperty(PropertyNames.ScrollbarDarkShadowColor, value); }
        }

        public string ScrollbarFaceColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarFaceColor); }
            set { SetProperty(PropertyNames.ScrollbarFaceColor, value); }
        }

        public string ScrollbarHighlightColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarHighlightColor); }
            set { SetProperty(PropertyNames.ScrollbarHighlightColor, value); }
        }

        public string ScrollbarShadowColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarShadowColor); }
            set { SetProperty(PropertyNames.ScrollbarShadowColor, value); }
        }

        public string ScrollbarTrackColor
        {
            get { return GetPropertyValue(PropertyNames.ScrollbarTrackColor); }
            set { SetProperty(PropertyNames.ScrollbarTrackColor, value); }
        }

        public string Stroke
        {
            get { return GetPropertyValue(PropertyNames.Stroke); }
            set { SetProperty(PropertyNames.Stroke, value); }
        }

        public string StrokeDasharray
        {
            get { return GetPropertyValue(PropertyNames.StrokeDasharray); }
            set { SetProperty(PropertyNames.StrokeDasharray, value); }
        }

        public string StrokeDashoffset
        {
            get { return GetPropertyValue(PropertyNames.StrokeDashoffset); }
            set { SetProperty(PropertyNames.StrokeDashoffset, value); }
        }

        public string StrokeLinecap
        {
            get { return GetPropertyValue(PropertyNames.StrokeLinecap); }
            set { SetProperty(PropertyNames.StrokeLinecap, value); }
        }

        public string StrokeLinejoin
        {
            get { return GetPropertyValue(PropertyNames.StrokeLinejoin); }
            set { SetProperty(PropertyNames.StrokeLinejoin, value); }
        }

        public string StrokeMiterlimit
        {
            get { return GetPropertyValue(PropertyNames.StrokeMiterlimit); }
            set { SetProperty(PropertyNames.StrokeMiterlimit, value); }
        }

        public string StrokeOpacity
        {
            get { return GetPropertyValue(PropertyNames.StrokeOpacity); }
            set { SetProperty(PropertyNames.StrokeOpacity, value); }
        }

        public string StrokeWidth
        {
            get { return GetPropertyValue(PropertyNames.StrokeWidth); }
            set { SetProperty(PropertyNames.StrokeWidth, value); }
        }

        public string TableLayout
        {
            get { return GetPropertyValue(PropertyNames.TableLayout); }
            set { SetProperty(PropertyNames.TableLayout, value); }
        }

        public string TextAlign
        {
            get { return GetPropertyValue(PropertyNames.TextAlign); }
            set { SetProperty(PropertyNames.TextAlign, value); }
        }

        public string TextAlignLast
        {
            get { return GetPropertyValue(PropertyNames.TextAlignLast); }
            set { SetProperty(PropertyNames.TextAlignLast, value); }
        }

        public string TextAnchor
        {
            get { return GetPropertyValue(PropertyNames.TextAnchor); }
            set { SetProperty(PropertyNames.TextAnchor, value); }
        }

        public string TextAutospace
        {
            get { return GetPropertyValue(PropertyNames.TextAutospace); }
            set { SetProperty(PropertyNames.TextAutospace, value); }
        }

        public string TextDecoration
        {
            get { return GetPropertyValue(PropertyNames.TextDecoration); }
            set { SetProperty(PropertyNames.TextDecoration, value); }
        }

        public string TextIndent
        {
            get { return GetPropertyValue(PropertyNames.TextIndent); }
            set { SetProperty(PropertyNames.TextIndent, value); }
        }

        public string TextJustify
        {
            get { return GetPropertyValue(PropertyNames.TextJustify); }
            set { SetProperty(PropertyNames.TextJustify, value); }
        }

        public string TextOverflow
        {
            get { return GetPropertyValue(PropertyNames.TextOverflow); }
            set { SetProperty(PropertyNames.TextOverflow, value); }
        }

        public string TextShadow
        {
            get { return GetPropertyValue(PropertyNames.TextShadow); }
            set { SetProperty(PropertyNames.TextShadow, value); }
        }

        public string TextTransform
        {
            get { return GetPropertyValue(PropertyNames.TextTransform); }
            set { SetProperty(PropertyNames.TextTransform, value); }
        }

        public string TextUnderlinePosition
        {
            get { return GetPropertyValue(PropertyNames.TextUnderlinePosition); }
            set { SetProperty(PropertyNames.TextUnderlinePosition, value); }
        }

        public string Top
        {
            get { return GetPropertyValue(PropertyNames.Top); }
            set { SetProperty(PropertyNames.Top, value); }
        }

        public string Transform
        {
            get { return GetPropertyValue(PropertyNames.Transform); }
            set { SetProperty(PropertyNames.Transform, value); }
        }

        public string TransformOrigin
        {
            get { return GetPropertyValue(PropertyNames.TransformOrigin); }
            set { SetProperty(PropertyNames.TransformOrigin, value); }
        }

        public string TransformStyle
        {
            get { return GetPropertyValue(PropertyNames.TransformStyle); }
            set { SetProperty(PropertyNames.TransformStyle, value); }
        }

        public string Transition
        {
            get { return GetPropertyValue(PropertyNames.Transition); }
            set { SetProperty(PropertyNames.Transition, value); }
        }

        public string TransitionDelay
        {
            get { return GetPropertyValue(PropertyNames.TransitionDelay); }
            set { SetProperty(PropertyNames.TransitionDelay, value); }
        }

        public string TransitionDuration
        {
            get { return GetPropertyValue(PropertyNames.TransitionDuration); }
            set { SetProperty(PropertyNames.TransitionDuration, value); }
        }

        public string TransitionProperty
        {
            get { return GetPropertyValue(PropertyNames.TransitionProperty); }
            set { SetProperty(PropertyNames.TransitionProperty, value); }
        }

        public string TransitionTimingFunction
        {
            get { return GetPropertyValue(PropertyNames.TransitionTimingFunction); }
            set { SetProperty(PropertyNames.TransitionTimingFunction, value); }
        }

        public string UnicodeBidirectional
        {
            get { return GetPropertyValue(PropertyNames.UnicodeBidirectional); }
            set { SetProperty(PropertyNames.UnicodeBidirectional, value); }
        }

        public string VerticalAlign
        {
            get { return GetPropertyValue(PropertyNames.VerticalAlign); }
            set { SetProperty(PropertyNames.VerticalAlign, value); }
        }

        public string Visibility
        {
            get { return GetPropertyValue(PropertyNames.Visibility); }
            set { SetProperty(PropertyNames.Visibility, value); }
        }

        public string WhiteSpace
        {
            get { return GetPropertyValue(PropertyNames.WhiteSpace); }
            set { SetProperty(PropertyNames.WhiteSpace, value); }
        }

        public string Widows
        {
            get { return GetPropertyValue(PropertyNames.Widows); }
            set { SetProperty(PropertyNames.Widows, value); }
        }

        public string Width
        {
            get { return GetPropertyValue(PropertyNames.Width); }
            set { SetProperty(PropertyNames.Width, value); }
        }

        public string WordBreak
        {
            get { return GetPropertyValue(PropertyNames.WordBreak); }
            set { SetProperty(PropertyNames.WordBreak, value); }
        }

        public string WordSpacing
        {
            get { return GetPropertyValue(PropertyNames.WordSpacing); }
            set { SetProperty(PropertyNames.WordSpacing, value); }
        }

        public string WritingMode
        {
            get { return GetPropertyValue(PropertyNames.WritingMode); }
            set { SetProperty(PropertyNames.WritingMode, value); }
        }

        public string ZIndex
        {
            get { return GetPropertyValue(PropertyNames.ZIndex); }
            set { SetProperty(PropertyNames.ZIndex, value); }
        }

        public string Zoom
        {
            get { return GetPropertyValue(PropertyNames.Zoom); }
            set { SetProperty(PropertyNames.Zoom, value); }
        }

    }
}