using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Shrinkify.QueueOperations;

namespace Shrinkify.Tests
{
    [TestClass]
    public class ShrinkMessageTests
    {
        [TestMethod]
        public void SerializeShrinkMessage()
        {
            var s1 = new ShrinkMessage("7F47F842-0CAA-486C-852D-92DF56E80B09", "test.png", "https://www.google.com/test.png");

            var message = SerializeMessage(s1);

            var s2 = DeserializeMessage<ShrinkMessage>(message);

            Assert.AreEqual(s1, s2);
        }
    }
}
