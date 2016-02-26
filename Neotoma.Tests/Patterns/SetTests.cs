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
    public class SetTests
    {
        [TestMethod]
        public void New()
        {
            var s1 = new Set('a', 'z');

            Assert.IsTrue(s1.Values.Contains('a'));
            Assert.IsNull(s1.Name);
            Assert.IsFalse(s1.Memoized);

            var s2 = new Set(new HashSet<char>() {'0', '9'}, "0-9");

            Assert.AreEqual("0-9", s2.Name);
            Assert.IsTrue(s2.Memoized);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException1()
        {
            var r = new Set(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewException2()
        {
            var r = new Set(new HashSet<char>());
        }

        [TestMethod]
        public void Match()
        {
            var s = new Set('x', 'f');
            var str = "foo";

            var node = s.Match(str) as ParseNode;

            Assert.IsNotNull(node);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(1, node.Length);
            Assert.AreEqual(new Position(str), node.Position);
            Assert.AreEqual(new Position(str).Advance(), node.NextPosition);
            Assert.AreEqual("f", node.Value);

            var str2 = "123";

            var err = s.Match(str2) as ParsingError;

            Assert.IsNotNull(err);
            Assert.AreEqual(s, err.Pattern);
            Assert.AreEqual(new Position(str2), err.Position);
            Assert.IsNull(err.InnerError);
            Assert.AreEqual($"Character '1' not in set [xf]", err.Message);

            var err2 = s.Match(string.Empty) as ParsingError;

            Assert.IsNotNull(err2);
            Assert.AreEqual(s, err2.Pattern);
            Assert.AreEqual(new Position(string.Empty), err2.Position);
            Assert.IsNull(err2.InnerError);
            Assert.AreEqual("EOF reached", err2.Message);
        }

        [TestMethod]
        public void Memoize()
        {
            var dot = new Set('a', 'z');

            var dot2 = dot.Memoize() as Set;

            Assert.IsNotNull(dot2);
            Assert.IsTrue(dot2.Memoized);
            Assert.AreEqual("", dot2.Name);

            var dot3 = dot2.Memoize();
            Assert.AreEqual(dot2, dot3);

            var dot4 = dot2.Memoize("Test") as Set;

            Assert.IsNotNull(dot4);
            Assert.IsTrue(dot4.Memoized);
            Assert.AreEqual("Test", dot4.Name);
        }

        [TestMethod]
        public new void ToString()
        {
            var r = new Set('\0', 'z');

            Assert.AreEqual("[\\0z]", r.ToString());
        }
    }
}