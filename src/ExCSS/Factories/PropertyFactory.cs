using System;
using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class PropertyFactory 
    {
        private static readonly Lazy<PropertyFactory> Lazy =
            new Lazy<PropertyFactory>(() => new PropertyFactory());

        internal static PropertyFactory Instance => Lazy.Value;

        private delegate Property LonghandCreator();
        private delegate ShorthandProperty ShorthandCreator();

        private readonly Dictionary<string, LonghandCreator> _longhands = new Dictionary<string, LonghandCreator>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, ShorthandCreator> _shorthands = new Dictionary<string, ShorthandCreator>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, LonghandCreator> _fonts = new Dictionary<string, LonghandCreator>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string[]> _mappings = new Dictionary<string, string[]>();
        private readonly List<string> _animatables = new List<string>();

        private PropertyFactory()
        {
            AddShorthand(PropertyNames.Animation, () => new AnimationProperty(),
                PropertyNames.AnimationName,
                PropertyNames.AnimationDuration,
                PropertyNames.AnimationTimingFunction,
                PropertyNames.AnimationDelay,
                PropertyNames.AnimationDirection,
                PropertyNames.AnimationFillMode,
                PropertyNames.AnimationIterationCount,
                PropertyNames.AnimationPlayState);
            AddLonghand(PropertyNames.AnimationDelay, () => new AnimationDelayProperty());
            AddLonghand(PropertyNames.AnimationDirection, () => new AnimationDirectionProperty());
            AddLonghand(PropertyNames.AnimationDuration, () => new AnimationDurationProperty());
            AddLonghand(PropertyNames.AnimationFillMode, () => new AnimationFillModeProperty());
            AddLonghand(PropertyNames.AnimationIterationCount, () => new AnimationIterationCountProperty());
            AddLonghand(PropertyNames.AnimationName, () => new AnimationNameProperty());
            AddLonghand(PropertyNames.AnimationPlayState, () => new AnimationPlayStateProperty());
            AddLonghand(PropertyNames.AnimationTimingFunction, () => new AnimationTimingFunctionProperty());

            AddShorthand(PropertyNames.Background, () => new BackgroundProperty(),
                PropertyNames.BackgroundAttachment,
                PropertyNames.BackgroundClip,
                PropertyNames.BackgroundColor,
                PropertyNames.BackgroundImage,
                PropertyNames.BackgroundOrigin,
                PropertyNames.BackgroundPosition,
                PropertyNames.BackgroundRepeat,
                PropertyNames.BackgroundSize);
            AddLonghand(PropertyNames.BackgroundAttachment, () => new BackgroundAttachmentProperty());
            AddLonghand(PropertyNames.BackgroundColor, () => new BackgroundColorProperty(), true);
            AddLonghand(PropertyNames.BackgroundClip, () => new BackgroundClipProperty());
            AddLonghand(PropertyNames.BackgroundOrigin, () => new BackgroundOriginProperty());
            AddLonghand(PropertyNames.BackgroundSize, () => new BackgroundSizeProperty(), true);
            AddLonghand(PropertyNames.BackgroundImage, () => new BackgroundImageProperty());
            AddLonghand(PropertyNames.BackgroundPosition, () => new BackgroundPositionProperty(), true);
            AddLonghand(PropertyNames.BackgroundRepeat, () => new BackgroundRepeatProperty());

            AddLonghand(PropertyNames.BorderSpacing, () => new BorderSpacingProperty());
            AddLonghand(PropertyNames.BorderCollapse, () => new BorderCollapseProperty());
            AddLonghand(PropertyNames.BoxShadow, () => new BoxShadowProperty(), true);
            AddLonghand(PropertyNames.BoxDecorationBreak, () => new BoxDecorationBreak());
            AddLonghand(PropertyNames.BreakAfter, () => new BreakAfterProperty());
            AddLonghand(PropertyNames.BreakBefore, () => new BreakBeforeProperty());
            AddLonghand(PropertyNames.BreakInside, () => new BreakInsideProperty());
            AddLonghand(PropertyNames.BackfaceVisibility, () => new BackfaceVisibilityProperty());

            AddShorthand(PropertyNames.BorderRadius, () => new BorderRadiusProperty(),
                PropertyNames.BorderTopLeftRadius,
                PropertyNames.BorderTopRightRadius,
                PropertyNames.BorderBottomRightRadius,
                PropertyNames.BorderBottomLeftRadius);
            AddLonghand(PropertyNames.BorderTopLeftRadius, () => new BorderTopLeftRadiusProperty(), true);
            AddLonghand(PropertyNames.BorderTopRightRadius, () => new BorderTopRightRadiusProperty(), true);
            AddLonghand(PropertyNames.BorderBottomLeftRadius, () => new BorderBottomLeftRadiusProperty(), true);
            AddLonghand(PropertyNames.BorderBottomRightRadius, () => new BorderBottomRightRadiusProperty(), true);

            AddShorthand(PropertyNames.BorderImage, () => new BorderImageProperty(),
                PropertyNames.BorderImageOutset,
                PropertyNames.BorderImageRepeat,
                PropertyNames.BorderImageSlice,
                PropertyNames.BorderImageSource,
                PropertyNames.BorderImageWidth);
            AddLonghand(PropertyNames.BorderImageOutset, () => new BorderImageOutsetProperty());
            AddLonghand(PropertyNames.BorderImageRepeat, () => new BorderImageRepeatProperty());
            AddLonghand(PropertyNames.BorderImageSource, () => new BorderImageSourceProperty());
            AddLonghand(PropertyNames.BorderImageSlice, () => new BorderImageSliceProperty());
            AddLonghand(PropertyNames.BorderImageWidth, () => new BorderImageWidthProperty());

            AddShorthand(PropertyNames.BorderColor, () => new BorderColorProperty(),
                PropertyNames.BorderTopColor,
                PropertyNames.BorderRightColor,
                PropertyNames.BorderBottomColor,
                PropertyNames.BorderLeftColor);
            AddShorthand(PropertyNames.BorderStyle, () => new BorderStyleProperty(),
                PropertyNames.BorderTopStyle,
                PropertyNames.BorderRightStyle,
                PropertyNames.BorderBottomStyle,
                PropertyNames.BorderLeftStyle);
            AddShorthand(PropertyNames.BorderWidth, () => new BorderWidthProperty(),
                PropertyNames.BorderTopWidth,
                PropertyNames.BorderRightWidth,
                PropertyNames.BorderBottomWidth,
                PropertyNames.BorderLeftWidth);
            AddShorthand(PropertyNames.BorderTop, () => new BorderTopProperty(),
                PropertyNames.BorderTopWidth,
                PropertyNames.BorderTopStyle,
                PropertyNames.BorderTopColor);
            AddShorthand(PropertyNames.BorderRight, () => new BorderRightProperty(),
                PropertyNames.BorderRightWidth,
                PropertyNames.BorderRightStyle,
                PropertyNames.BorderRightColor);
            AddShorthand(PropertyNames.BorderBottom, () => new BorderBottomProperty(),
                PropertyNames.BorderBottomWidth,
                PropertyNames.BorderBottomStyle,
                PropertyNames.BorderBottomColor);
            AddShorthand(PropertyNames.BorderLeft, () => new BorderLeftProperty(),
                PropertyNames.BorderLeftWidth,
                PropertyNames.BorderLeftStyle,
                PropertyNames.BorderLeftColor);

            AddShorthand(PropertyNames.Border, () => new BorderProperty(),
                PropertyNames.BorderTopWidth,
                PropertyNames.BorderTopStyle,
                PropertyNames.BorderTopColor,
                PropertyNames.BorderRightWidth,
                PropertyNames.BorderRightStyle,
                PropertyNames.BorderRightColor,
                PropertyNames.BorderBottomWidth,
                PropertyNames.BorderBottomStyle,
                PropertyNames.BorderBottomColor,
                PropertyNames.BorderLeftWidth,
                PropertyNames.BorderLeftStyle,
                PropertyNames.BorderLeftColor);
            AddLonghand(PropertyNames.BorderTopColor, () => new BorderTopColorProperty(), true);
            AddLonghand(PropertyNames.BorderLeftColor, () => new BorderLeftColorProperty(), true);
            AddLonghand(PropertyNames.BorderRightColor, () => new BorderRightColorProperty(), true);
            AddLonghand(PropertyNames.BorderBottomColor, () => new BorderBottomColorProperty(), true);
            AddLonghand(PropertyNames.BorderTopStyle, () => new BorderTopStyleProperty());
            AddLonghand(PropertyNames.BorderLeftStyle, () => new BorderLeftStyleProperty());
            AddLonghand(PropertyNames.BorderRightStyle, () => new BorderRightStyleProperty());
            AddLonghand(PropertyNames.BorderBottomStyle, () => new BorderBottomStyleProperty());
            AddLonghand(PropertyNames.BorderTopWidth, () => new BorderTopWidthProperty(), true);
            AddLonghand(PropertyNames.BorderLeftWidth, () => new BorderLeftWidthProperty(), true);
            AddLonghand(PropertyNames.BorderRightWidth, () => new BorderRightWidthProperty(), true);
            AddLonghand(PropertyNames.BorderBottomWidth, () => new BorderBottomWidthProperty(), true);

            AddLonghand(PropertyNames.Bottom, () => new BottomProperty(), true);

            AddShorthand(PropertyNames.Columns, () => new ColumnsProperty(),
                PropertyNames.ColumnWidth,
                PropertyNames.ColumnCount);
            AddLonghand(PropertyNames.ColumnCount, () => new ColumnCountProperty(), true);
            AddLonghand(PropertyNames.ColumnWidth, () => new ColumnWidthProperty(), true);

            AddLonghand(PropertyNames.ColumnFill, () => new ColumnFillProperty());
            AddLonghand(PropertyNames.ColumnGap, () => new ColumnGapProperty(), true);
            AddLonghand(PropertyNames.ColumnSpan, () => new ColumnSpanProperty());

            AddShorthand(PropertyNames.ColumnRule, () => new ColumnRuleProperty(),
                PropertyNames.ColumnRuleWidth,
                PropertyNames.ColumnRuleStyle,
                PropertyNames.ColumnRuleColor);
            AddLonghand(PropertyNames.ColumnRuleColor, () => new ColumnRuleColorProperty(), true);
            AddLonghand(PropertyNames.ColumnRuleStyle, () => new ColumnRuleStyleProperty());
            AddLonghand(PropertyNames.ColumnRuleWidth, () => new ColumnRuleWidthProperty(), true);

            AddLonghand(PropertyNames.CaptionSide, () => new CaptionSideProperty());
            AddLonghand(PropertyNames.Clear, () => new ClearProperty());
            AddLonghand(PropertyNames.Clip, () => new ClipProperty(), true);
            AddLonghand(PropertyNames.Color, () => new ColorProperty(), true);
            AddLonghand(PropertyNames.Content, () => new ContentProperty());
            AddLonghand(PropertyNames.CounterIncrement, () => new CounterIncrementProperty());
            AddLonghand(PropertyNames.CounterReset, () => new CounterResetProperty());
            AddLonghand(PropertyNames.Cursor, () => new CursorProperty());
            AddLonghand(PropertyNames.Direction, () => new DirectionProperty());
            AddLonghand(PropertyNames.Display, () => new DisplayProperty());
            AddLonghand(PropertyNames.EmptyCells, () => new EmptyCellsProperty());
            AddLonghand(PropertyNames.Float, () => new FloatProperty());

            AddShorthand(PropertyNames.Font, () => new FontProperty(),
                PropertyNames.FontFamily,
                PropertyNames.FontSize,
                PropertyNames.FontStretch,
                PropertyNames.FontStyle,
                PropertyNames.FontVariant,
                PropertyNames.FontWeight,
                PropertyNames.LineHeight);
            AddLonghand(PropertyNames.FontFamily, () => new FontFamilyProperty(), false, true);
            AddLonghand(PropertyNames.FontSize, () => new FontSizeProperty(), true);
            AddLonghand(PropertyNames.FontSizeAdjust, () => new FontSizeAdjustProperty(), true);
            AddLonghand(PropertyNames.FontStyle, () => new FontStyleProperty(), false, true);
            AddLonghand(PropertyNames.FontVariant, () => new FontVariantProperty(), false, true);
            AddLonghand(PropertyNames.FontWeight, () => new FontWeightProperty(), true, true);
            AddLonghand(PropertyNames.FontStretch, () => new FontStretchProperty(), true, true);
            AddLonghand(PropertyNames.LineHeight, () => new LineHeightProperty(), true);

            AddLonghand(PropertyNames.Height, () => new HeightProperty(), true);
            AddLonghand(PropertyNames.Left, () => new LeftProperty(), true);
            AddLonghand(PropertyNames.LetterSpacing, () => new LetterSpacingProperty());

            AddShorthand(PropertyNames.ListStyle, () => new ListStyleProperty(),
                PropertyNames.ListStyleType,
                PropertyNames.ListStyleImage,
                PropertyNames.ListStylePosition);
            AddLonghand(PropertyNames.ListStyleImage, () => new ListStyleImageProperty());
            AddLonghand(PropertyNames.ListStylePosition, () => new ListStylePositionProperty());
            AddLonghand(PropertyNames.ListStyleType, () => new ListStyleTypeProperty());

            AddShorthand(PropertyNames.Margin, () => new MarginProperty(),
                PropertyNames.MarginTop,
                PropertyNames.MarginRight,
                PropertyNames.MarginBottom,
                PropertyNames.MarginLeft);
            AddLonghand(PropertyNames.MarginRight, () => new MarginRightProperty(), true);
            AddLonghand(PropertyNames.MarginLeft, () => new MarginLeftProperty(), true);
            AddLonghand(PropertyNames.MarginTop, () => new MarginTopProperty(), true);
            AddLonghand(PropertyNames.MarginBottom, () => new MarginBottomProperty(), true);

            AddLonghand(PropertyNames.MaxHeight, () => new MaxHeightProperty(), true);
            AddLonghand(PropertyNames.MaxWidth, () => new MaxWidthProperty(), true);
            AddLonghand(PropertyNames.MinHeight, () => new MinHeightProperty(), true);
            AddLonghand(PropertyNames.MinWidth, () => new MinWidthProperty(), true);
            AddLonghand(PropertyNames.Opacity, () => new OpacityProperty(), true);
            AddLonghand(PropertyNames.Orphans, () => new OrphansProperty());

            AddShorthand(PropertyNames.Outline, () => new OutlineProperty(),
                PropertyNames.OutlineWidth,
                PropertyNames.OutlineStyle,
                PropertyNames.OutlineColor);
            AddLonghand(PropertyNames.OutlineColor, () => new OutlineColorProperty(), true);
            AddLonghand(PropertyNames.OutlineStyle, () => new OutlineStyleProperty());
            AddLonghand(PropertyNames.OutlineWidth, () => new OutlineWidthProperty(), true);

            AddLonghand(PropertyNames.Overflow, () => new OverflowProperty());
            AddLonghand(PropertyNames.OverflowWrap, () => new OverflowWrapProperty());

            AddShorthand(PropertyNames.Padding, () => new PaddingProperty(),
                PropertyNames.PaddingTop,
                PropertyNames.PaddingRight,
                PropertyNames.PaddingBottom,
                PropertyNames.PaddingLeft);
            AddLonghand(PropertyNames.PaddingTop, () => new PaddingTopProperty(), true);
            AddLonghand(PropertyNames.PaddingRight, () => new PaddingRightProperty(), true);
            AddLonghand(PropertyNames.PaddingLeft, () => new PaddingLeftProperty(), true);
            AddLonghand(PropertyNames.PaddingBottom, () => new PaddingBottomProperty(), true);

            AddLonghand(PropertyNames.PageBreakAfter, () => new PageBreakAfterProperty());
            AddLonghand(PropertyNames.PageBreakBefore, () => new PageBreakBeforeProperty());
            AddLonghand(PropertyNames.PageBreakInside, () => new PageBreakInsideProperty());
            AddLonghand(PropertyNames.Perspective, () => new PerspectiveProperty(), true);
            AddLonghand(PropertyNames.PerspectiveOrigin, () => new PerspectiveOriginProperty(), true);
            AddLonghand(PropertyNames.Position, () => new PositionProperty());
            AddLonghand(PropertyNames.Quotes, () => new QuotesProperty());
            AddLonghand(PropertyNames.Right, () => new RightProperty(), true);
            AddLonghand(PropertyNames.Stroke, () => new StrokeProperty(), true);
            AddLonghand(PropertyNames.StrokeDasharray, () => new StrokeDasharrayProperty(), true);
            AddLonghand(PropertyNames.StrokeDashoffset, () => new StrokeDashoffsetProperty(), true);
            AddLonghand(PropertyNames.StrokeLinecap, () => new StrokeLinecapProperty(), true);
            AddLonghand(PropertyNames.StrokeLinejoin, () => new StrokeLinejoinProperty(), true);
            AddLonghand(PropertyNames.StrokeMiterlimit, () => new StrokeMiterlimitProperty(), true);
            AddLonghand(PropertyNames.StrokeOpacity, () => new StrokeOpacityProperty(), true);
            AddLonghand(PropertyNames.StrokeWidth, () => new StrokeWidthProperty(), true);
            AddLonghand(PropertyNames.TableLayout, () => new TableLayoutProperty());
            AddLonghand(PropertyNames.TextAlign, () => new TextAlignProperty());
            AddLonghand(PropertyNames.TextAlignLast, () => new TextAlignLastProperty());
            AddLonghand(PropertyNames.TextAnchor, () => new TextAnchorProperty());

            AddShorthand(PropertyNames.TextDecoration, () => new TextDecorationProperty(),
                PropertyNames.TextDecorationLine,
                PropertyNames.TextDecorationStyle,
                PropertyNames.TextDecorationColor);
            AddLonghand(PropertyNames.TextDecorationStyle, () => new TextDecorationStyleProperty());
            AddLonghand(PropertyNames.TextDecorationLine, () => new TextDecorationLineProperty());
            AddLonghand(PropertyNames.TextDecorationColor, () => new TextDecorationColorProperty(), true);

            AddLonghand(PropertyNames.TextIndent, () => new TextIndentProperty(), true);
            AddLonghand(PropertyNames.TextJustify, () => new TextJustifyProperty());
            AddLonghand(PropertyNames.TextTransform, () => new TextTransformProperty());
            AddLonghand(PropertyNames.TextShadow, () => new TextShadowProperty(), true);
            AddLonghand(PropertyNames.Transform, () => new TransformProperty(), true);
            AddLonghand(PropertyNames.TransformOrigin, () => new TransformOriginProperty(), true);
            AddLonghand(PropertyNames.TransformStyle, () => new TransformStyleProperty());

            AddShorthand(PropertyNames.Transition, () => new TransitionProperty(),
                PropertyNames.TransitionProperty,
                PropertyNames.TransitionDuration,
                PropertyNames.TransitionTimingFunction,
                PropertyNames.TransitionDelay);
            AddLonghand(PropertyNames.TransitionDelay, () => new TransitionDelayProperty());
            AddLonghand(PropertyNames.TransitionDuration, () => new TransitionDurationProperty());
            AddLonghand(PropertyNames.TransitionTimingFunction, () => new TransitionTimingFunctionProperty());
            AddLonghand(PropertyNames.TransitionProperty, () => new TransitionPropertyProperty());

            AddLonghand(PropertyNames.Top, () => new TopProperty(), true);
            AddLonghand(PropertyNames.UnicodeBidirectional, () => new UnicodeBidirectionalProperty());
            AddLonghand(PropertyNames.VerticalAlign, () => new VerticalAlignProperty(), true);
            AddLonghand(PropertyNames.Visibility, () => new VisibilityProperty(), true);
            AddLonghand(PropertyNames.WhiteSpace, () => new WhiteSpaceProperty());
            AddLonghand(PropertyNames.Widows, () => new WidowsProperty());
            AddLonghand(PropertyNames.Width, () => new WidthProperty(), true);
            AddLonghand(PropertyNames.WordBreak, () => new WordBreakProperty(), true);
            AddLonghand(PropertyNames.WordSpacing, () => new WordSpacingProperty(), true);
            AddLonghand(PropertyNames.WordWrap, () => new OverflowWrapProperty());
            AddLonghand(PropertyNames.ZIndex, () => new ZIndexProperty(), true);
            AddLonghand(PropertyNames.ObjectFit, () => new ObjectFitProperty());
            AddLonghand(PropertyNames.ObjectPosition, () => new ObjectPositionProperty(), true);

            _fonts.Add(PropertyNames.Src, () => new SrcProperty());
            _fonts.Add(PropertyNames.UnicodeRange, () => new UnicodeRangeProperty());
        }

        private void AddShorthand(string name, ShorthandCreator creator, params string[] longhands)
        {
            _shorthands.Add(name, creator);
            _mappings.Add(name, longhands);
        }

        private void AddLonghand(string name, LonghandCreator creator, bool animatable = false, bool font = false)
        {
            _longhands.Add(name, creator);

            if (animatable)
            {
                _animatables.Add(name);
            }

            if (font)
            {
                _fonts.Add(name, creator);
            }
        }

        public Property Create(string name)
        {
            return CreateLonghand(name) ?? CreateShorthand(name);
        }

        public Property CreateFont(string name)
        {
            LonghandCreator propertyCreator;

            if (_fonts.TryGetValue(name, out propertyCreator))
            {
                return propertyCreator();
            }

            return null;
        }

        public Property CreateViewport(string name)
        {
            var feature = MediaFeatureFactory.Instance.Create(name);

            return feature != null ? new FeatureProperty(feature) : null;
        }

        public Property CreateLonghand(string name)
        {
            LonghandCreator createProperty;

            if (_longhands.TryGetValue(name, out createProperty))
            {
                return createProperty();
            }

            return null;
        }

        public ShorthandProperty CreateShorthand(string name)
        {
            ShorthandCreator propertyCreator;

            return _shorthands.TryGetValue(name, out propertyCreator) ? propertyCreator() : null;
        }

        public Property[] CreateLonghandsFor(string name)
        {
            var propertyNames = GetLonghands(name);

            return propertyNames.Select(CreateLonghand).ToArray();
        }

        public bool IsShorthand(string name)
        {
            return _shorthands.ContainsKey(name);
        }

        public bool IsAnimatable(string name)
        {
            return _longhands.ContainsKey(name) 
                ? _animatables.Contains(name) 
                : GetLonghands(name).Any(longhand => _animatables.Contains(name));
        }

        public string[] GetLonghands(string name)
        {
            return _mappings.ContainsKey(name) 
                ? _mappings[name] 
                : new string[0];
        }

        public IEnumerable<string> GetShorthands(string name)
        {
            foreach (var mapping in _mappings)
            {
                if (mapping.Value.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    yield return mapping.Key;
                }
            }
        }
    }
}