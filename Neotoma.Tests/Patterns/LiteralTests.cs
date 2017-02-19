using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neotoma.Tests
{
    [TestClass]
    public class LiteralTests
    {
        [TestMethod]
        public void New()
        {
            var lit = new Literal("foo", "Test");

            Assert.AreEqual("foo", lit.Value);
            Assert.AreEqual("Test", lit.Name);
            Assert.IsTrue(lit.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException1()
        {
            var lit = new Literal(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException2()
        {
            var lit = new Literal("");
        }

        [TestMethod]
        public void Memoize()
        {
            var lit = new Literal("foo");

            var lit2 = lit.Memoize() as Literal;

            Assert.IsNotNull(lit2);
            Assert.AreEqual(lit.Value, lit2.Value);
            Assert.IsTrue(lit2.Memoized);
            Assert.AreEqual("", lit2.Name);

            var lit3 = lit2.Memoize();
            Assert.AreEqual(lit2, lit3);

            var lit4 = lit2.Memoize("Test") as Literal;

            Assert.IsNotNull(lit4);
            Assert.AreEqual(lit.Value, lit4.Value);
            Assert.IsTrue(lit4.Memoized);
            Assert.AreEqual("Test", lit4.Name);
        }

        [TestMethod]
        public void Match()
        {
            var lit = new Literal("foo");

            var res1 = lit.Match("football") as ParseNode;

            Assert.IsNotNull(res1);
            Assert.AreEqual(new Position("football"), res1.Position);
            Assert.AreEqual("foo", res1.Value);
            Assert.AreEqual(3, res1.Length);
            Assert.AreEqual(0, res1.Children.Count);
            Assert.AreEqual(
                new Position("football")
                .Advance()
                .Advance()
                .Advance(), 
                res1.NextPosition);

            var res2 = lit.Match("foreclosure") as ParsingError;

            Assert.IsNotNull(res2);
            Assert.AreEqual(lit, res2.Pattern);
            Assert.IsNull(res2.InnerError);
            Assert.AreEqual(
                new Position("foreclosure")
                .Advance()
                .Advance(),
                res2.Position);
            Assert.AreEqual(@"Expected ""foo"", got 'r' instead of 'o'", res2.Message);

            var res3 = lit.Match("fo") as ParsingError;

            Assert.IsNotNull(res3);
            Assert.AreEqual(lit, res3.Pattern);
            Assert.IsNull(res3.InnerError);
            Assert.AreEqual(
                new Position("fo")
                .Advance()
                .Advance(),
                res3.Position);
            Assert.AreEqual(@"Expected ""foo"", got EOF", res3.Message);
        }

        [TestMethod]
        public new void ToString()
        {
            var lit = new Literal("XYZ\0\a\b\f\r\n\t\v\"\\");

            Assert.AreEqual(@"""XYZ\0\a\b\f\r\n\t\v\""\\""", lit.ToString());
        }
    }
}
