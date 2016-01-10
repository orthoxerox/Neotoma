using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neotoma.Tests
{
    [TestClass]
    public class ParseNodeTests
    {
        [TestMethod]
        public void NewLeaf()
        {
            var str = "string";
            var pos = new Position(str);
            var nextPos = pos.Advance().Advance();

            var node = new ParseNode(pos, nextPos);

            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(2, node.Length);
            Assert.AreEqual(pos, node.Position);
            Assert.AreEqual(nextPos, node.NextPosition);
            Assert.AreEqual("st", node.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLeafException1()
        {
            var node = new ParseNode(
                new Position("foo"), 
                new Position("bar"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLeafException2()
        {
            var pos = new Position("foo");
            var node = new ParseNode(
                pos.Advance(),
                pos);
        }

        [TestMethod]
        public void NewBranch1()
        {
            var str = "string";
            var pos1 = new Position(str);
            var pos2 = pos1.Advance();
            var pos3 = pos2.Advance().Advance();

            var ch1 = new ParseNode(pos1, pos2);
            var ch2 = new ParseNode(pos2, pos3);

            var node = new ParseNode(ch1, ch2);

            Assert.AreEqual(2, node.Children.Count);
            Assert.AreEqual(ch1, node.Children[0]);
            Assert.AreEqual(ch2, node.Children[1]);
            Assert.AreEqual(3, node.Length);
            Assert.AreEqual(pos1, node.Position);
            Assert.AreEqual(pos3, node.NextPosition);
            Assert.AreEqual(null, node.Value);
        }

        [TestMethod]
        public void NewBranch2()
        {
            var str = "string";
            var pos1 = new Position(str);
            var pos2 = pos1.Advance();
            var pos3 = pos2.Advance().Advance();

            var ch1 = new ParseNode(pos1, pos2);
            var ch2 = new ParseNode(pos2, pos3);

            var node = new ParseNode(new List<ParseNode> { ch1, ch2 });

            Assert.AreEqual(2, node.Children.Count);
            Assert.AreEqual(ch1, node.Children[0]);
            Assert.AreEqual(ch2, node.Children[1]);
            Assert.AreEqual(3, node.Length);
            Assert.AreEqual(pos1, node.Position);
            Assert.AreEqual(pos3, node.NextPosition);
            Assert.AreEqual(null, node.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewBranchException1()
        {
            var node = new ParseNode((IReadOnlyList<ParseNode>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewBranchException2()
        {
            var node = new ParseNode(new List<ParseNode>());
        }
    }
}
