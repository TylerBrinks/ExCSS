
namespace ExCSS.Model
{
    abstract class Function : Term
    {
        private Function()
        {
        }

        internal static Function Create(string name, TermList arguments)
        {
            //TODO
            return null;
        }

        class CalcFunction : Function
        {

        }

        class AttrFunction : Function
        {

        }

        class RgbFunction : Function
        {

        }

        class RgbaFunction : Function
        {

        }

        class HslFunction : Function
        {

        }

        class ToggleFunction : Function
        {

        }

        class RotateFunction : Function
        {

        }

        class TransformFunction : Function
        {

        }

        class SkewFunction : Function
        {

        }

        class LinearGradientFunction : Function
        {

        }
    }
}
