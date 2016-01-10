using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Neotoma.Tests.Patterns
{
    [TestClass]
    public class SingleCharacterTests
    {
        [TestMethod]
        public void New()
        {
            var x = new SingleCharacter(UnicodeCategory.DecimalDigitNumber);

            Assert.AreEqual(UnicodeCategory.DecimalDigitNumber, x.Category);
            Assert.IsNull(x.Name);
            Assert.IsFalse(x.Memoized);

            var y = new SingleCharacter(
                UnicodeCategory.DecimalDigitNumber, 
                true, 
                "DOT");

            Assert.AreEqual("DOT", y.Name);
            Assert.IsTrue(y.Memoized);
        }

        [TestMethod]
        public void Match()
        {
            var str = "123";

            var dot = new SingleCharacter(UnicodeCategory.DecimalDigitNumber);

            var node = dot.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(1, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str).Advance(), node.NextPosition);
            Assert.AreEqual("1", node.Value);

            var str2 = "foo";

            var err = dot.Match(str2) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(dot, err.Pattern);
            Assert.AreEqual(new Position(str2), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual($"Expected {UnicodeCategory.DecimalDigitNumber}, got {UnicodeCategory.LowercaseLetter}", err.Message);

            var err2 = dot.Match(string.Empty) as ParsingError;

            Assert.IsNotNull(err2);
            Assert.AreEqual(dot, err2.Pattern);
            Assert.AreEqual(new Position(string.Empty), err2.Position);
            Assert.IsNull(err2.InnerError);
            Assert.AreEqual("EOF reached", err2.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new SingleCharacter(UnicodeCategory.DecimalDigitNumber);

            var dot2 = dot.Memoize() as SingleCharacter;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.IsNull(dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as SingleCharacter;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod]
        public new void ToString()
        {
            var dot = new SingleCharacter(UnicodeCategory.DecimalDigitNumber);

            Assert.AreEqual($"[[{UnicodeCategory.DecimalDigitNumber}]]", dot.ToString());
        }
    }
}
