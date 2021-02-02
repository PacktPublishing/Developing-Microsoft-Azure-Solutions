using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace LinkHide.Tests
{
    [TestClass]
    public class TokenTests
    {
        private const string TEST_VALUE = "ABC12";
        private const string TEST_VALUE2 = "89XYZ";

        [TestMethod]
        public void Token_Value_Consistency()
        {

            var t = new Token(TEST_VALUE);
            var v = t.Value;

            Assert.IsNotNull(v);
            Assert.AreEqual(TEST_VALUE, v);
        }

        [TestMethod]
        public void Token_Equals_Consistency()
        {
            var t1 = new Token(TEST_VALUE);
            var t2 = new Token(TEST_VALUE); 

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void Token_NotEquals_Consistency()
        {
            var t1 = new Token(TEST_VALUE);
            var t2 = new Token(TEST_VALUE2);

            Assert.IsFalse(t1.Equals(t2));
        }

        [TestMethod]
        public void Token_HashCode_Consistency()
        {
            var t1 = new Token(TEST_VALUE);
            var t2 = new Token(TEST_VALUE);

            Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void Token_GenerateOneHundred_Consistency()
        {
            var h = new HashSet<Token>();
            var g = new DefaultTokenGenerator();

            for (int i = 0; i < 100; i++)
            {
                h.Add(g.Generate());
            }

            var h2 = new HashSet<string>();

            foreach (var t in h)
            {
                h2.Add(t.Value);
            }
        }
    }
}
