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
    public class ParsingErrorTests
    {
        [TestMethod]
        public void New()
        {
            var message = "foo";
            var position = new Position(message);
            var pattern1 = new Literal("foo", true, "Hello");
            var pattern2 = new Literal("foo");

            var err1 = new ParsingError(message, position, pattern1);
            Assert.AreEqual(message, err1.Message);
            Assert.AreEqual(position, err1.Position);
            Assert.AreEqual(pattern1, err1.Pattern);
            Assert.AreEqual(null, err1.InnerError);

            var err2 = new ParsingError(message, position, pattern2);
            Assert.AreEqual(pattern2, err2.Pattern);
            Assert.AreEqual(null, err2.InnerError);

            var err3 = new ParsingError(message, position, pattern1, err1);
            Assert.AreEqual(err1, err3.InnerError);

            var err4 = new ParsingError(message, position, pattern1, err2);
            Assert.AreEqual(null, err4.InnerError);
        }


    }
}