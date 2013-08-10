using System;

namespace ExCSS.Model
{
    sealed class Rect
    {
        #region Members

        PrimitiveValue top;
        PrimitiveValue right;
        PrimitiveValue bottom;
        PrimitiveValue left;

        #endregion

        #region ctor

        internal Rect()
        {
        }

        #endregion

        #region Properties

        public PrimitiveValue Top
        {
            get { return top; }
            set { top = value; }
        }

        public PrimitiveValue Right
        {
            get { return right; }
            set { right = value; }
        }

        public PrimitiveValue Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public PrimitiveValue Left
        {
            get { return left; }
            set { left = value; }
        }

        #endregion
    }
}
