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
    public class RangeTests
    {
        [TestMethod]
        public void New()
        {
            var r1 = new Range('a', 'z');

            Assert.AreEqual('a', r1.Low);
            Assert.AreEqual('z', r1.High);
            Assert.IsNull(r1.Name);
            Assert.IsFalse(r1.Memoized);

            var r2 = new Range('0', '9', "0-9");

            Assert.AreEqual("0-9", r2.Name);
            Assert.IsTrue(r2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewException()
        {
            var r = new Range('b', 'a');
        }

        [TestMethod]
        public void Match()
        {
            var r = new Range('a', 'z');
            var str = "foo";

            var node = r.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(1, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str).Advance(), node.NextPosition);
            Assert.AreEqual("f", node.Value);

            var str2 = "123";

            var err = r.Match(str2) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(r, err.Pattern);
            Assert.AreEqual(new Position(str2), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual($"Character '1' not in set [a-z]", err.Message);

            var err2 = r.Match(string.Empty) as ParsingError;

            Assert.IsNotNull(err2);
            Assert.AreEqual(r, err2.Pattern);
            Assert.AreEqual(new Position(string.Empty), err2.Position);
            Assert.IsNull(err2.InnerError);
            Assert.AreEqual("EOF reached", err2.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new Range('a','z');

            var dot2 = dot.Memoize() as Range;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Range;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod]
        public new void ToString()
        {
            var r = new Range('\0', 'z');

            Assert.AreEqual("[\\0-z]", r.ToString());
        }
    }
}