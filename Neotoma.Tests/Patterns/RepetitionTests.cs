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
    public class RepetitionTests
    {
        [TestMethod]
        public void New()
        {
            var a = new Literal("a");
            var r1 = new Repetition(a, 1, 2);

            Assert.AreEqual(a, r1.Pattern);
            Assert.AreEqual(1, r1.Minimum);
            Assert.AreEqual(2, r1.Maximum);

            Assert.IsNull(r1.Name);
            Assert.IsFalse(r1.Memoized);

            var r2 = new Repetition(a, 1, 2, "ch");

            Assert.AreEqual("ch", r2.Name);
            Assert.IsTrue(r2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException1()
        {
            var r = new Repetition(null, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewException2()
        {
            var r = new Repetition(new AnySingleCharacter(), -1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewException3()
        {
            var r = new Repetition(new AnySingleCharacter(), 1, 0);
        }

        [TestMethod]
        public void Match()
        {
            var a = new Literal("a");
            var r1 = new Repetition(a, 0, 1);

            var str1 = "bbb";

            var node1 = r1.Match(str1) as ParseNode;

            Assert.IsNotNull(node1);
            Assert.AreEqual(0, node1.Children.Count);
            Assert.AreEqual(0, node1.Length);
            Assert.AreEqual(new Position(str1), node1.Position);
            Assert.AreEqual(new Position(str1), node1.NextPosition);
            Assert.AreEqual("", node1.Value);

            var b = new Literal("b").Memoize("B");
            var r2 = new Repetition(b, 1, 2);

            var node2 = r2.Match(str1) as ParseNode;

            Assert.IsNotNull(node2);
            Assert.AreEqual(2, node2.Children.Count);
            Assert.AreEqual(2, node2.Length);
            Assert.AreEqual(new Position(str1), node2.Position);
            Assert.AreEqual(new Position(str1).Advance().Advance(), node2.NextPosition);
            Assert.IsNull(node2.Value);

            var r3 = new Repetition(b, 0, 1);

            var node3 = r3.Match(str1) as ParseNode;

            Assert.IsNotNull(node3);
            Assert.AreEqual(0, node3.Children.Count);
            Assert.AreEqual(1, node3.Length);
            Assert.AreEqual(new Position(str1), node3.Position);
            Assert.AreEqual(new Position(str1).Advance(), node3.NextPosition);
            Assert.AreEqual("b", node3.Value);
            Assert.AreEqual("B", node3.Name);

            var r4 = new Repetition(b, 4, 4);

            var err = r4.Match(str1) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(r4, err.Pattern);
            Assert.AreEqual("Found only 3 repetitions instead of 4", err.Message);
            Assert.AreEqual(new Position(str1).Advance().Advance().Advance().Advance(), err.Position);

        }

        [TestMethod]
        public void Memoize()
        {
            var a = new Literal("a");
            var dot = new Repetition(a, 1, 1);

            var dot2 = dot.Memoize() as Repetition;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Repetition;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod()]
        public new void ToString()
        {
            var a = new Literal("a");
            var b = new Literal("b");
            var r1 = new Repetition(a, 0, 1);
            var r2 = new Repetition(a, 0, int.MaxValue);
            var r3 = new Repetition(a, 1, int.MaxValue);
            var r4 = new Repetition(a, 2, 3);

            Assert.AreEqual(@"""a""?", r1.ToString());
            Assert.AreEqual(@"""a""*", r2.ToString());
            Assert.AreEqual(@"""a""+", r3.ToString());
            Assert.AreEqual(@"""a""{2,3}", r4.ToString());
        }
    }
}