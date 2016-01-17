using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neotoma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neotoma.Tests
{
    [TestClass]
    public class ChoiceTests
    {
        [TestMethod]
        public void New()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var c1 = new Choice(a, b);

            Assert.AreEqual(a, c1.Patterns[0]);
            Assert.AreEqual(b, c1.Patterns[1]);

            Assert.IsNull(c1.Name);
            Assert.IsFalse(c1.Memoized);

            var c2 = new Choice(a, b, true, "ch");

            Assert.AreEqual("ch", c2.Name);
            Assert.IsTrue(c2.Memoized);

            var c = new Literal("c");

            var c3 = new Choice(a, b, c);

            Assert.AreEqual(a, c3.Patterns[0]);
            Assert.AreEqual(b, c3.Patterns[1]);
            Assert.AreEqual(c, c3.Patterns[2]);

            Assert.IsNull(c3.Name);
            Assert.IsFalse(c3.Memoized);

            var c4 = new Choice(new[] { a, b, c }, true, "c4");

            Assert.AreEqual(a, c4.Patterns[0]);
            Assert.AreEqual(b, c4.Patterns[1]);
            Assert.AreEqual(c, c4.Patterns[2]);

            Assert.AreEqual("c4", c4.Name);
            Assert.IsTrue(c4.Memoized);

            var c5 = new Choice(c1, c);

            Assert.AreEqual(a, c5.Patterns[0]);
            Assert.AreEqual(b, c5.Patterns[1]);
            Assert.AreEqual(c, c5.Patterns[2]);

            var c6 = new Choice(c1, a);

            Assert.AreEqual(a, c6.Patterns[0]);
            Assert.AreEqual(b, c6.Patterns[1]);
            Assert.AreEqual(a, c6.Patterns[2]);

            var c7 = new Choice(new Pattern[] { c1, c });

            Assert.AreEqual(a, c7.Patterns[0]);
            Assert.AreEqual(b, c7.Patterns[1]);
            Assert.AreEqual(c, c7.Patterns[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException1()
        {
            var c = new Choice(null, new AnySingleCharacter());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException2()
        {
            var c = new Choice(new AnySingleCharacter(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException3()
        {
            var c = new Choice(new[] { new AnySingleCharacter(), null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException4()
        {
            var c = new Choice((IReadOnlyList<Pattern>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewException5()
        {
            var c = new Choice(new AnySingleCharacter[0]);
        }

        [TestMethod]
        public void New2()
        {

        }

        [TestMethod]
        public void Match()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var c1 = new Choice(a, b);

            var str = "b";

            var node = c1.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(1, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str).Advance(), node.NextPosition);
            Assert.AreEqual("b", node.Value);

            var str2 = "x";

            var err1 = c1.Match(str2) as ParsingError;

            Assert.IsNotNull(err1);
            Assert.AreEqual(c1, err1.Pattern);
            Assert.AreEqual(new Position("x"), err1.Position);
            Assert.IsNull(err1.InnerError);
            Assert.AreEqual($"Failed to match any patern", err1.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var dot = new Choice(a, b);

            var dot2 = dot.Memoize() as Choice;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.IsNull(dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Choice;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod()]
        public new void ToString()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var ap = new Choice(a, b);

            Assert.AreEqual(@"""a"" / ""b""", ap.ToString());
        }
    }
}