using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neotoma;
using static Neotoma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neotoma.Tests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void pTest()
        {
            var l = p("lol");

            Assert.IsInstanceOfType(l, typeof(Literal));
            Assert.AreEqual("lol", l.Value);
        }

        [TestMethod]
        public void sTest()
        {
            var set = s("abc");

            Assert.IsInstanceOfType(set, typeof(Set));
            Assert.AreEqual(3, set.Values.Count);
            Assert.IsTrue(set.Values.Contains('a'));
            Assert.IsTrue(set.Values.Contains('b'));
            Assert.IsTrue(set.Values.Contains('c'));
        }

        [TestMethod]
        public void rTest()
        {
            var ran = r('a', 'z');

            Assert.IsInstanceOfType(ran, typeof(Range));
            Assert.AreEqual('a', ran.Low);
            Assert.AreEqual('z', ran.High);
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsInstanceOfType(Any, typeof(AnySingleCharacter));
        }

        [TestMethod]
        public void EOFTest()
        {
            Assert.IsInstanceOfType(EOF, typeof(Antipattern));
            Assert.IsInstanceOfType(((Antipattern)EOF).Pattern, typeof(AnySingleCharacter));
        }
    }
}