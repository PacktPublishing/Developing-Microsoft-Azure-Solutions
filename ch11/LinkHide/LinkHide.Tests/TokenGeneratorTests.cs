using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkHide.Tests
{
    [TestClass]
    public class TokenGeneratorTests
    {
        [TestMethod]
        public void DefaultTokenGenerator_GenerateOneToken()
        {
            ITokenGenerator g = new DefaultTokenGenerator();
            var t = g.Generate();
            Assert.IsNotNull(t);

            var v = t.Value;

            Assert.IsNotNull(v);
            Assert.IsTrue(v.Length == 5);
        }
    }
}
