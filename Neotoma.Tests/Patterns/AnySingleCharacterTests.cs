using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neotoma.Tests.Patterns
{
    [TestClass]
    public class AnySingleCharacterTests
    {
        [TestMethod]
        public void New()
        {
            var dot1 = new AnySingleCharacter();

            Assert.IsNull(dot1.Name);
            Assert.IsFalse(dot1.Memoized);

            var dot2 = new AnySingleCharacter("DOT");

            Assert.AreEqual("DOT", dot2.Name);
            Assert.IsTrue(dot2.Memoized);
        }

        [TestMethod]
        public void Match()
        {
            var str = "foo";

            var dot = new AnySingleCharacter();

            var node = dot.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(1, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str).Advance(), node.NextPosition);
            Assert.AreEqual("f", node.Value);

            var err = dot.Match(string.Empty) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(dot, err.Pattern);
            Assert.AreEqual(new Position(string.Empty), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual("EOF reached", err.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new AnySingleCharacter();

            var dot2 = dot.Memoize() as AnySingleCharacter;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as AnySingleCharacter;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod]
        public new void ToString()
        {
            var dot = new AnySingleCharacter();

            Assert.AreEqual(".", dot.ToString());
        }
    }
}
