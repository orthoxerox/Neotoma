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
    public class BacktrackTests
    {
        [TestMethod]
        public void New()
        {
            var dot = new AnySingleCharacter();

            var a1 = new Backtrack(dot);

            Assert.AreEqual(dot, a1.Pattern);

            Assert.IsNull(a1.Name);
            Assert.IsFalse(a1.Memoized);

            var a2 = new Backtrack(dot, "bt");

            Assert.AreEqual("bt", a2.Name);
            Assert.IsTrue(a2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException()
        {
            var a = new Backtrack(null);
        }

        [TestMethod]
        public void Match()
        {
            var ap = new Backtrack(new AnySingleCharacter());

            var str = "x";

            var node = ap.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str), node.NextPosition);
            Assert.AreEqual("", node.Value);


            var err = ap.Match("") as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(ap, err.Pattern);
            Assert.AreEqual(new Position(""), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual($"Couldn't match pattern .", err.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new Backtrack(new AnySingleCharacter());

            var dot2 = dot.Memoize() as Backtrack;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Backtrack;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod()]
        public new void ToString()
        {
            var ap = new Backtrack(new Range('a', 'z'));

            Assert.AreEqual("&[a-z]", ap.ToString());
        }
    }
}