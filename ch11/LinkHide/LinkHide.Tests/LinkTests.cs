using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinkHide.Tests
{
    [TestClass]
    public class LinkTests
    {
        [TestMethod]
        public void Link_LinkType_Consistency()
        {
            Assert.AreEqual((int)LinkType.Unknown, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Link_NullToken_Consistency()
        {
            new Link(null, LinkType.Unknown, "https://somelink");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Link_NullLink_Consistency()
        {
            new Link(new Token("ABC12"), LinkType.Unknown, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Link_EmptyLink_Consistency()
        {
            new Link(new Token("ABC12"), LinkType.Unknown, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Link_WhiteSpaceLink_Consistency()
        {
            new Link(new Token("ABC12"), LinkType.Unknown, "    ");
        }
    }
}
