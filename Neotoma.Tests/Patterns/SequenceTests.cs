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
    public class SequenceTests
    {
        [TestMethod]
        public void New()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var s1 = new Sequence(a, b);

            Assert.AreEqual(a, s1.Patterns[0]);
            Assert.AreEqual(b, s1.Patterns[1]);
            Assert.IsNull(s1.Name);
            Assert.IsFalse(s1.Memoized);

            var s2 = new Sequence(a, b, "hello");

            Assert.AreEqual("hello", s2.Name);
            Assert.IsTrue(s2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException1()
        {
            var s = new Sequence(null, new AnySingleCharacter());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException2()
        {
            var s = new Sequence(new AnySingleCharacter(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException3()
        {
            var s = new Sequence(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewException4()
        {
            var s = new Sequence(new List<Pattern>());
        }

        [TestMethod]
        public void Match()
        {
            var a = new Literal("a");
            var b = new Literal("b").Memoize("B");

            var str1 = "ab";

            var s1 = new Sequence(a, b);

            var node1 = s1.Match(str1) as ParseNode;

            Assert.IsNotNull(node1);
            Assert.AreEqual(0, node1.Children.Count);
            Assert.AreEqual(1, node1.Length);
            Assert.AreEqual(new Position(str1).Advance(), node1.Position);
            Assert.AreEqual(new Position(str1).Advance().Advance(), node1.NextPosition);
            Assert.AreEqual("b", node1.Value);
            Assert.AreEqual("B", node1.Name);

            var str2 = "bb";

            var s2 = new Sequence(b, b);

            var node2 = s2.Match(str2) as ParseNode;

            Assert.IsNotNull(node2);
            Assert.AreEqual(2, node2.Children.Count);
            Assert.AreEqual(2, node2.Length);
            Assert.AreEqual(new Position(str2), node2.Position);
            Assert.AreEqual(new Position(str2).Advance().Advance(), node2.NextPosition);
            Assert.IsNull(node2.Value);

            var str3 = "aa";

            var s3 = new Sequence(a, a);

            var node3 = s3.Match(str3) as ParseNode;

            Assert.IsNotNull(node3);
            Assert.AreEqual(0, node3.Children.Count);
            Assert.AreEqual(2, node3.Length);
            Assert.AreEqual(new Position(str3), node3.Position);
            Assert.AreEqual(new Position(str3).Advance().Advance(), node3.NextPosition);
            Assert.AreEqual("aa", node3.Value);
            Assert.AreEqual(null, node3.Name);

            var err = s3.Match(str1) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(a, err.Pattern);
            //Assert.AreEqual("Found only 3 repetitions instead of 4", err.Message);
            Assert.AreEqual(new Position(str1).Advance(), err.Position);

        }

        [TestMethod]
        public void Memoize()
        {
            var a = new Literal("a");
            var dot = new Sequence(a, a);

            var dot2 = dot.Memoize() as Sequence;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Sequence;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod()]
        public new void ToString()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var s1 = new Sequence(a, b);

            Assert.AreEqual(@"""a"" ""b""", s1.ToString());
        }
    }
}