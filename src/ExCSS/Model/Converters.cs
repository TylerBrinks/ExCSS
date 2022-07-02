﻿using System;
using System.Linq;

namespace ExCSS
{
    internal static class Converters
    {
        public static readonly IValueConverter LineWidthConverter =
            new StructValueConverter<Length>(ValueExtensions.ToBorderWidth);

        public static readonly IValueConverter LengthConverter =
            new StructValueConverter<Length>(ValueExtensions.ToLength);

        public static readonly IValueConverter ResolutionConverter =
            new StructValueConverter<Resolution>(ValueExtensions.ToResolution);

        public static readonly IValueConverter TimeConverter = new StructValueConverter<Time>(ValueExtensions.ToTime);
        public static readonly IValueConverter UrlConverter = new UrlValueConverter();
        public static readonly IValueConverter StringConverter = new StringValueConverter();
        public static readonly IValueConverter EvenStringsConverter = new StringsValueConverter();

        public static readonly IValueConverter LiteralsConverter =
            new IdentifierValueConverter(ValueExtensions.ToLiterals);

        public static readonly IValueConverter IdentifierConverter =
            new IdentifierValueConverter(ValueExtensions.ToIdentifier);

        public static readonly IValueConverter AnimatableConverter =
            new IdentifierValueConverter(ValueExtensions.ToAnimatableIdentifier);

        public static readonly IValueConverter IntegerConverter =
            new StructValueConverter<long>(ValueExtensions.ToInteger);

        public static readonly IValueConverter NaturalIntegerConverter =
            new StructValueConverter<long>(ValueExtensions.ToNaturalInteger);

        public static readonly IValueConverter WeightIntegerConverter =
            new StructValueConverter<long>(ValueExtensions.ToWeightInteger);

        public static readonly IValueConverter PositiveIntegerConverter =
            new StructValueConverter<long>(ValueExtensions.ToPositiveInteger);

        public static readonly IValueConverter
            BinaryConverter = new StructValueConverter<long>(ValueExtensions.ToBinary);

        public static readonly IValueConverter
            AngleConverter = new StructValueConverter<Angle>(ValueExtensions.ToAngle);

        public static readonly IValueConverter NumberConverter =
            new StructValueConverter<float>(ValueExtensions.ToSingle);

        public static readonly IValueConverter NaturalNumberConverter =
            new StructValueConverter<float>(ValueExtensions.ToNaturalSingle);

        public static readonly IValueConverter PercentConverter =
            new StructValueConverter<Percent>(ValueExtensions.ToPercent);

        public static readonly IValueConverter RgbComponentConverter =
            new StructValueConverter<byte>(ValueExtensions.ToRgbComponent);

        public static readonly IValueConverter AlphaValueConverter =
            new StructValueConverter<float>(ValueExtensions.ToAlphaValue);

        public static readonly IValueConverter PureColorConverter =
            new StructValueConverter<Color>(ValueExtensions.ToColor);

        public static readonly IValueConverter LengthOrPercentConverter =
            new StructValueConverter<Length>(ValueExtensions.ToDistance);

        public static readonly IValueConverter AngleNumberConverter =
            new StructValueConverter<Angle>(ValueExtensions.ToAngleNumber);

        public static readonly IValueConverter SideOrCornerConverter = WithAny(
            Assign(Keywords.Left, -1.0).Or(Keywords.Right, 1.0).Option(0.0),
            Assign(Keywords.Top, 1.0).Or(Keywords.Bottom, -1.0).Option(0.0)
        );

        public static readonly IValueConverter PointConverter = Construct(() =>
        {
            var hi = Assign(Keywords.Left, Length.Zero)
                .Or(Keywords.Right, new Length(100f, Length.Unit.Percent))
                .Or(Keywords.Center, new Length(50f, Length.Unit.Percent));
            var vi = Assign(Keywords.Top, Length.Zero)
                .Or(Keywords.Bottom, new Length(100f, Length.Unit.Percent))
                .Or(Keywords.Center, new Length(50f, Length.Unit.Percent));
            var h = hi.Or(LengthOrPercentConverter).Required();
            var v = vi.Or(LengthOrPercentConverter).Required();

            return LengthOrPercentConverter.Or(
                Toggle(Keywords.Left, Keywords.Right)).Or(
                Toggle(Keywords.Top, Keywords.Bottom)).Or(
                Keywords.Center, Point.Center).Or(
                WithOrder(h, v)).Or(
                WithOrder(v, h)).Or(
                WithOrder(hi, vi, LengthOrPercentConverter)).Or(
                WithOrder(hi, LengthOrPercentConverter, vi)).Or(
                WithOrder(hi, LengthOrPercentConverter, vi, LengthOrPercentConverter));
        });

        public static readonly IValueConverter AttrConverter = new FunctionValueConverter(
            FunctionNames.Attr, WithArgs(StringConverter.Or(IdentifierConverter)));

        public static readonly IValueConverter StepsConverter = new FunctionValueConverter(
            FunctionNames.Steps, WithArgs(
                IntegerConverter.Required(),
                Assign(Keywords.Start, true).Or(Keywords.End, false).Option(false)));

        public static readonly IValueConverter CubicBezierConverter = Construct(() =>
        {
            var number = NumberConverter.Required();
            return new FunctionValueConverter(FunctionNames.CubicBezier,
                WithArgs(number, number, number, number));
        });

        public static readonly IValueConverter CounterConverter = Construct(() =>
        {
            var name = IdentifierConverter.Required();
            var kind = IdentifierConverter.Option(Keywords.Decimal);
            var def = StringConverter.Required();
            return new FunctionValueConverter(FunctionNames.Counter, WithArgs(name, kind)
                .Or(new FunctionValueConverter(FunctionNames.Counters, WithArgs(name, def, kind))));
        });

        public static readonly IValueConverter ShapeConverter = Construct(() =>
        {
            var length = LengthConverter.Required();
            return new FunctionValueConverter(FunctionNames.Rect, WithArgs(length, length, length, length)
                .Or(WithArgs(LengthConverter.Many(4, 4))));
        }).OrAuto();

        public static readonly IValueConverter LinearGradientConverter = Construct(() =>
            new FunctionValueConverter(FunctionNames.LinearGradient, new LinearGradientConverter()).Or(
                new FunctionValueConverter(FunctionNames.RepeatingLinearGradient, new LinearGradientConverter())));

        public static readonly IValueConverter RadialGradientConverter = Construct(() =>
            new FunctionValueConverter(FunctionNames.RadialGradient, new RadialGradientConverter()).Or(
                new FunctionValueConverter(FunctionNames.RepeatingRadialGradient, new RadialGradientConverter())));

        public static readonly IValueConverter RgbColorConverter = Construct(() =>
        {
            var number = RgbComponentConverter.Required();
            return new FunctionValueConverter(FunctionNames.Rgb, WithArgs(number, number, number));
        });

        public static readonly IValueConverter RgbaColorConverter = Construct(() =>
        {
            var value = RgbComponentConverter.Required();
            var alpha = AlphaValueConverter.Required();
            return new FunctionValueConverter(FunctionNames.Rgba, WithArgs(value, value, value, alpha));
        });

        public static readonly IValueConverter HslColorConverter = Construct(() =>
        {
            var hue = AngleNumberConverter.Required();
            var percent = PercentConverter.Required();
            return new FunctionValueConverter(FunctionNames.Hsl, WithArgs(hue, percent, percent));
        });

        public static readonly IValueConverter HslaColorConverter = Construct(() =>
        {
            var hue = AngleNumberConverter.Required();
            var percent = PercentConverter.Required();
            var alpha = AlphaValueConverter.Required();
            return new FunctionValueConverter(FunctionNames.Hsla, WithArgs(hue, percent, percent, alpha));
        });

        public static readonly IValueConverter GrayColorConverter = Construct(() =>
        {
            var value = RgbComponentConverter.Required();
            var alpha = AlphaValueConverter.Option(1f);
            return new FunctionValueConverter(FunctionNames.Gray, WithArgs(value, alpha));
        });

        public static readonly IValueConverter HwbColorConverter = Construct(() =>
        {
            var hue = AngleNumberConverter.Required();
            var percent = PercentConverter.Required();
            var alpha = AlphaValueConverter.Option(1f);
            return new FunctionValueConverter(FunctionNames.Hwb, WithArgs(hue, percent, percent, alpha));
        });

        public static readonly IValueConverter PerspectiveConverter =
            Construct(() => new FunctionValueConverter(FunctionNames.Perspective, WithArgs(LengthConverter)));

        public static readonly IValueConverter MatrixTransformConverter = Construct(() =>
            new FunctionValueConverter(FunctionNames.Matrix, WithArgs(NumberConverter, 6)).Or(
                new FunctionValueConverter(FunctionNames.Matrix3d, WithArgs(NumberConverter, 16))));

        public static readonly IValueConverter TranslateTransformConverter = Construct(() =>
        {
            var distance = LengthOrPercentConverter.Required();
            var option = LengthOrPercentConverter.Option(Length.Zero);
            return new FunctionValueConverter(FunctionNames.Translate, WithArgs(distance, option)).Or(
                new FunctionValueConverter(FunctionNames.Translate3d, WithArgs(distance, option, option))).Or(
                new FunctionValueConverter(FunctionNames.TranslateX, WithArgs(LengthOrPercentConverter))).Or(
                new FunctionValueConverter(FunctionNames.TranslateY, WithArgs(LengthOrPercentConverter))).Or(
                new FunctionValueConverter(FunctionNames.TranslateZ, WithArgs(LengthOrPercentConverter)));
        });

        public static readonly IValueConverter ScaleTransformConverter = Construct(() =>
        {
            var number = NumberConverter.Required();
            var option = NumberConverter.Option(float.NaN);
            return new FunctionValueConverter(FunctionNames.Scale, WithArgs(number, option)).Or(
                new FunctionValueConverter(FunctionNames.Scale3d, WithArgs(number, option, option))).Or(
                new FunctionValueConverter(FunctionNames.ScaleX, WithArgs(NumberConverter))).Or(
                new FunctionValueConverter(FunctionNames.ScaleY, WithArgs(NumberConverter))).Or(
                new FunctionValueConverter(FunctionNames.ScaleZ, WithArgs(NumberConverter)));
        });

        public static readonly IValueConverter RotateTransformConverter = Construct(() =>
        {
            var number = NumberConverter.Required();
            return new FunctionValueConverter(FunctionNames.Rotate, WithArgs(AngleConverter)).Or(
                new FunctionValueConverter(FunctionNames.Rotate3d,
                    WithArgs(number, number, number, AngleConverter.Required()))).Or(
                new FunctionValueConverter(FunctionNames.RotateX, WithArgs(AngleConverter))).Or(
                new FunctionValueConverter(FunctionNames.RotateY, WithArgs(AngleConverter))).Or(
                new FunctionValueConverter(FunctionNames.RotateZ, WithArgs(AngleConverter)));
        });

        public static readonly IValueConverter SkewTransformConverter = Construct(() =>
        {
            var angle = AngleConverter.Required();
            return new FunctionValueConverter(FunctionNames.Skew, WithArgs(angle, angle)).Or(
                new FunctionValueConverter(FunctionNames.SkewX, WithArgs(AngleConverter))).Or(
                new FunctionValueConverter(FunctionNames.SkewY, WithArgs(AngleConverter)));
        });

        public static readonly IValueConverter DefaultFontFamiliesConverter = Map.DefaultFontFamilies.ToConverter();
        public static readonly IValueConverter LineStyleConverter = Map.LineStyles.ToConverter();
        public static readonly IValueConverter BackgroundAttachmentConverter = Map.BackgroundAttachments.ToConverter();
        public static readonly IValueConverter BackgroundRepeatConverter = Map.BackgroundRepeats.ToConverter();
        public static readonly IValueConverter BoxModelConverter = Map.BoxModels.ToConverter();
        public static readonly IValueConverter AnimationDirectionConverter = Map.AnimationDirections.ToConverter();
        public static readonly IValueConverter AnimationFillStyleConverter = Map.AnimationFillStyles.ToConverter();
        public static readonly IValueConverter TextDecorationStyleConverter = Map.TextDecorationStyles.ToConverter();

        public static readonly IValueConverter TextDecorationLinesConverter =
            Map.TextDecorationLines.ToConverter().Many().OrNone();

        public static readonly IValueConverter ListPositionConverter = Map.ListPositions.ToConverter();
        public static readonly IValueConverter ListStyleConverter = Map.ListStyles.ToConverter();
        public static readonly IValueConverter BreakModeConverter = Map.BreakModes.ToConverter();
        public static readonly IValueConverter BreakInsideModeConverter = Map.BreakInsideModes.ToConverter();
        public static readonly IValueConverter PageBreakModeConverter = Map.PageBreakModes.ToConverter();
        public static readonly IValueConverter UnicodeModeConverter = Map.UnicodeModes.ToConverter();
        public static readonly IValueConverter VisibilityConverter = Map.Visibilities.ToConverter();
        public static readonly IValueConverter PlayStateConverter = Map.PlayStates.ToConverter();
        public static readonly IValueConverter FontVariantConverter = Map.FontVariants.ToConverter();
        public static readonly IValueConverter DirectionModeConverter = Map.DirectionModes.ToConverter();
        public static readonly IValueConverter HorizontalAlignmentConverter = Map.HorizontalAlignments.ToConverter();
        public static readonly IValueConverter VerticalAlignmentConverter = Map.VerticalAlignments.ToConverter();
        public static readonly IValueConverter WhitespaceConverter = Map.WhitespaceModes.ToConverter();
        public static readonly IValueConverter TextTransformConverter = Map.TextTransforms.ToConverter();
        public static readonly IValueConverter TextAlignLastConverter = Map.TextAlignmentsLast.ToConverter();
        public static readonly IValueConverter TextAnchorConverter = Map.TextAnchors.ToConverter();
        public static readonly IValueConverter TextJustifyConverter = Map.TextJustifyOptions.ToConverter();
        public static readonly IValueConverter JustifyContentConverter = Map.JustifyContentOptions.ToConverter();
        public static readonly IValueConverter ObjectFittingConverter = Map.ObjectFittings.ToConverter();
        public static readonly IValueConverter PositionModeConverter = Map.PositionModes.ToConverter();
        public static readonly IValueConverter OverflowModeConverter = Map.OverflowModes.ToConverter();
        public static readonly IValueConverter FloatingConverter = Map.FloatingModes.ToConverter();
        public static readonly IValueConverter DisplayModeConverter = Map.DisplayModes.ToConverter();
        public static readonly IValueConverter ClearModeConverter = Map.ClearModes.ToConverter();
        public static readonly IValueConverter FontStretchConverter = Map.FontStretches.ToConverter();
        public static readonly IValueConverter FontStyleConverter = Map.FontStyles.ToConverter();
        public static readonly IValueConverter FontWeightConverter = Map.FontWeights.ToConverter();
        public static readonly IValueConverter SystemFontConverter = Map.SystemFonts.ToConverter();
        public static readonly IValueConverter StrokeLinecapConverter = Map.StrokeLinecaps.ToConverter();
        public static readonly IValueConverter StrokeLinejoinConverter = Map.StrokeLinejoins.ToConverter();
        public static readonly IValueConverter WordBreakConverter = Map.WordBreaks.ToConverter();
        public static readonly IValueConverter OverflowWrapConverter = Map.OverflowWraps.ToConverter();
        public static readonly IValueConverter FillRuleConverter = Map.FillRules.ToConverter();

        #region Specific

        public static readonly IValueConverter OptionalIntegerConverter = IntegerConverter.OrAuto();

        public static readonly IValueConverter PositiveOrInfiniteNumberConverter =
            NaturalNumberConverter.Or(Keywords.Infinite, float.PositiveInfinity);

        public static readonly IValueConverter OptionalNumberConverter = NumberConverter.OrNone();

        public static readonly IValueConverter LengthOrNormalConverter =
            LengthConverter.Or(Keywords.Normal, new Length(1f, Length.Unit.Em));

        public static readonly IValueConverter OptionalLengthConverter = LengthConverter.Or(Keywords.Normal);
        public static readonly IValueConverter AutoLengthConverter = LengthConverter.OrAuto();
        public static readonly IValueConverter OptionalLengthOrPercentConverter = LengthOrPercentConverter.OrNone();
        public static readonly IValueConverter AutoLengthOrPercentConverter = LengthOrPercentConverter.OrAuto();

        public static readonly IValueConverter FontSizeConverter =
            LengthOrPercentConverter.Or(Map.FontSizes.ToConverter());

        #endregion

        #region Composed

        public static readonly IValueConverter LineHeightConverter =
            LengthOrPercentConverter.Or(NumberConverter).Or(Keywords.Normal);

        public static readonly IValueConverter BorderSliceConverter = PercentConverter.Or(NumberConverter);

        public static readonly IValueConverter ImageBorderWidthConverter =
            LengthOrPercentConverter.Or(NumberConverter).Or(Keywords.Auto);

        public static readonly IValueConverter TransitionConverter = new DictionaryValueConverter<ITimingFunction>(
            Map.TimingFunctions).Or(StepsConverter).Or(CubicBezierConverter);

        public static readonly IValueConverter GradientConverter = LinearGradientConverter.Or(RadialGradientConverter);

        public static readonly IValueConverter TransformConverter = MatrixTransformConverter
            .Or(ScaleTransformConverter)
            .Or(RotateTransformConverter)
            .Or(TranslateTransformConverter)
            .Or(SkewTransformConverter)
            .Or(PerspectiveConverter);

        public static readonly IValueConverter ColorConverter = PureColorConverter
            .Or(RgbColorConverter.Or(RgbaColorConverter))
            .Or(HslColorConverter.Or(HslaColorConverter))
            .Or(GrayColorConverter.Or(HwbColorConverter));

        public static readonly IValueConverter CurrentColorConverter = ColorConverter.WithCurrentColor();
        public static readonly IValueConverter InvertedColorConverter = CurrentColorConverter.Or(Keywords.Invert);
        public static readonly IValueConverter PaintConverter = UrlConverter.Or(CurrentColorConverter.OrNone());

        public static readonly IValueConverter StrokeDasharrayConverter =
            LengthOrPercentConverter.Or(NumberConverter).Many().OrNone();

        public static readonly IValueConverter StrokeMiterlimitConverter =
            new StructValueConverter<float>(ValueExtensions.ToGreaterOrEqualOneSingle);

        public static readonly IValueConverter RatioConverter = WithOrder(
            IntegerConverter.Required(),
            IntegerConverter.StartsWithDelimiter().Required());

        public static readonly IValueConverter ShadowConverter = WithAny(
            Assign(Keywords.Inset, true).Option(false),
            LengthConverter.Many(2, 4).Required(),
            ColorConverter.WithCurrentColor().Option(Color.Black));

        public static readonly IValueConverter MultipleShadowConverter = ShadowConverter.FromList().OrNone();
        public static readonly IValueConverter ImageSourceConverter = UrlConverter.Or(GradientConverter);
        public static readonly IValueConverter OptionalImageSourceConverter = ImageSourceConverter.OrNone();
        public static readonly IValueConverter MultipleImageSourceConverter = OptionalImageSourceConverter.FromList();
        public static readonly IValueConverter BorderRadiusShorthandConverter = new BorderRadiusConverter();

        public static readonly IValueConverter BorderRadiusConverter = WithOrder(
            LengthOrPercentConverter.Required(), LengthOrPercentConverter.Option());

        public static readonly IValueConverter FontFamiliesConverter =
            DefaultFontFamiliesConverter.Or(StringConverter).Or(LiteralsConverter).FromList();

        public static readonly IValueConverter BackgroundSizeConverter = AutoLengthOrPercentConverter.Or(
            Keywords.Cover).Or(Keywords.Contain).Or(
            WithOrder(AutoLengthOrPercentConverter.Required(), AutoLengthOrPercentConverter.Required()));

        public static readonly IValueConverter BackgroundRepeatsConverter = BackgroundRepeatConverter.Or(
            Keywords.RepeatX).Or(Keywords.RepeatY).Or(
            WithOrder(BackgroundRepeatConverter.Required(), BackgroundRepeatConverter.Required()));

        #endregion

        #region Toggles

        public static readonly IValueConverter TableLayoutConverter = Toggle(Keywords.Fixed, Keywords.Auto);
        public static readonly IValueConverter EmptyCellsConverter = Toggle(Keywords.Show, Keywords.Hide);
        public static readonly IValueConverter CaptionSideConverter = Toggle(Keywords.Top, Keywords.Bottom);
        public static readonly IValueConverter BackfaceVisibilityConverter = Toggle(Keywords.Visible, Keywords.Hidden);
        public static readonly IValueConverter BorderCollapseConverter = Toggle(Keywords.Separate, Keywords.Collapse);
        public static readonly IValueConverter BoxDecorationConverter = Toggle(Keywords.Clone, Keywords.Slice);
        public static readonly IValueConverter ColumnSpanConverter = Toggle(Keywords.All, Keywords.None);
        public static readonly IValueConverter ColumnFillConverter = Toggle(Keywords.Balance, Keywords.Auto);

        #endregion

        #region Misc

        public static IValueConverter Any = new AnyValueConverter();

        public static IValueConverter Assign<T>(string identifier, T result)
        {
            return new IdentifierValueConverter<T>(identifier, result);
        }

        public static IValueConverter Toggle(string on, string off)
        {
            return Assign(on, true).Or(off, false);
        }

        #endregion

        #region Order / Unordered

        public static IValueConverter WithOrder(params IValueConverter[] converters)
        {
            return new OrderedOptionsConverter(converters);
        }

        public static IValueConverter WithAny(params IValueConverter[] converters)
        {
            return new UnorderedOptionsConverter(converters);
        }

        public static IValueConverter Continuous(IValueConverter converter)
        {
            return new ContinuousValueConverter(converter);
        }

        #endregion

        #region Helper

        private static IValueConverter Construct(Func<IValueConverter> f)
        {
            return f();
        }

        private static IValueConverter WithArgs(IValueConverter converter, int arguments)
        {
            var converters = Enumerable.Repeat(converter, arguments).ToArray();
            return WithArgs(converters);
        }

        private static IValueConverter WithArgs(IValueConverter converter)
        {
            return new ArgumentsValueConverter(converter);
        }

        private static IValueConverter WithArgs(params IValueConverter[] converters)
        {
            return new ArgumentsValueConverter(converters);
        }

        #endregion
    }
}