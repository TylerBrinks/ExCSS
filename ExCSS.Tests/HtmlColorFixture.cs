using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class HtmlColorFixture
    {
        [Test]
        public void Null_color_does_not_throw()
        {
            bool result = true;
            Assert.DoesNotThrow(() =>
            {
                HtmlColor color;
                result = HtmlColor.TryFromHex(null, out color);
            });
            Assert.IsFalse(result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_color_throws()
        {
            HtmlColor.FromHex(null);
        }

        [Test]
        public void Color_with_leading_hashtag_can_be_parsed_by_try()
        {
            HtmlColor color;
            bool result = HtmlColor.TryFromHex("#FF0000", out color);
            Assert.IsTrue(result);
            Assert.AreEqual(new HtmlColor(255,0,0), color);
        }

        [Test]
        public void Color_with_leading_hashtag_can_be_parsed()
        {
            HtmlColor color = HtmlColor.FromHex("#FF0000");
            Assert.AreEqual(new HtmlColor(255, 0, 0), color);
        }
    }
}
