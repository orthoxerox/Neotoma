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
    public class AntipatternTests
    {
        [TestMethod]
        public void New()
        {
            var dot = new AnySingleCharacter();

            var a1 = new Antipattern(dot);

            Assert.AreEqual(dot, a1.Pattern);

            Assert.IsNull(a1.Name);
            Assert.IsFalse(a1.Memoized);

            var a2 = new Antipattern(dot, "ap");

            Assert.AreEqual("ap", a2.Name);
            Assert.IsTrue(a2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException()
        {
            var a = new Antipattern(null);
        }

        [TestMethod]
        public void Match()
        {
            var ap = new Antipattern(new AnySingleCharacter());

            var node = ap.Match("") as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0, node.Length);
            Assert.AreEqual(new Position(""), node.Position);
            Assert.AreEqual(new Position(""), node.NextPosition);
            Assert.AreEqual("", node.Value);

            var str = "x";

            var err = ap.Match(str) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(ap, err.Pattern);
            Assert.AreEqual(new Position(str), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual($"Successfully matched antipattern", err.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new Antipattern(new AnySingleCharacter());

            var dot2 = dot.Memoize() as Antipattern;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Antipattern;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod()]
        public new void ToString()
        {
            var ap = new Antipattern(new Range('a', 'z'));

            Assert.AreEqual("![a-z]", ap.ToString());
        }
    }
}