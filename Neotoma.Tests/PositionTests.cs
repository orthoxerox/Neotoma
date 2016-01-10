using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neotoma;

namespace Neotoma.Tests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void New()
        {
            var empty = new Position(string.Empty);
            Assert.AreEqual(string.Empty, empty.String);
            Assert.AreEqual(0, empty.Index);
            Assert.AreEqual(1, empty.Line);
            Assert.AreEqual(1, empty.Column);
            Assert.IsTrue(empty.EOF);

            var str = "Foo";
            var nonEmpty = new Position(str);
            Assert.AreEqual(str, nonEmpty.String);
            Assert.AreEqual(0, nonEmpty.Index);
            Assert.AreEqual(1, nonEmpty.Line);
            Assert.AreEqual(1, nonEmpty.Column);
            Assert.IsFalse(nonEmpty.EOF);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewException()
        {
            var empty = new Position(null);
        }

        [TestMethod]
        public void Advance()
        {
            var empty = new Position(string.Empty).Advance();
            Assert.AreEqual(string.Empty, empty.String);
            Assert.AreEqual(0, empty.Index);
            Assert.AreEqual(1, empty.Line);
            Assert.AreEqual(1, empty.Column);
            Assert.IsTrue(empty.EOF);

            var str = "a\rb\nc\r\nd";
            var p = new Position(str);

            Assert.AreEqual(0, p.Index);
            Assert.AreEqual(1, p.Line);
            Assert.AreEqual(1, p.Column);
            Assert.AreEqual('a', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(1, p.Index);
            Assert.AreEqual(1, p.Line);
            Assert.AreEqual(2, p.Column);
            Assert.AreEqual('\r', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(2, p.Index);
            Assert.AreEqual(2, p.Line);
            Assert.AreEqual(1, p.Column);
            Assert.AreEqual('b', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(3, p.Index);
            Assert.AreEqual(2, p.Line);
            Assert.AreEqual(2, p.Column);
            Assert.AreEqual('\n', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(4, p.Index);
            Assert.AreEqual(3, p.Line);
            Assert.AreEqual(1, p.Column);
            Assert.AreEqual('c', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(5, p.Index);
            Assert.AreEqual(3, p.Line);
            Assert.AreEqual(2, p.Column);
            Assert.AreEqual('\r', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(6, p.Index);
            Assert.AreEqual(3, p.Line);
            Assert.AreEqual(3, p.Column);
            Assert.AreEqual('\n', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(7, p.Index);
            Assert.AreEqual(4, p.Line);
            Assert.AreEqual(1, p.Column);
            Assert.AreEqual('d', p.String[p.Index]);
            Assert.IsFalse(p.EOF);
            p = p.Advance();

            Assert.AreEqual(8, p.Index);
            Assert.AreEqual(4, p.Line);
            Assert.AreEqual(2, p.Column);
            Assert.IsTrue(p.EOF);
            p = p.Advance();

            Assert.AreEqual(8, p.Index);
            Assert.AreEqual(4, p.Line);
            Assert.AreEqual(2, p.Column);
            Assert.IsTrue(p.EOF);
        }
    }
}
