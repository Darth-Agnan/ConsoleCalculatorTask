using NUnit.Framework;

namespace ConsoleCalculator
{
    [TestFixture]
    public class ConsoleMessages_Tests
    {
        [Test]
        public void ConsoleMessagesConstrucor_Test()
        {
            var id = 10;
            var testString = "Test string";
            var testClassObj = new ConsoleMessages(id, testString);
            Assert.AreEqual(id, testClassObj.Id);
            Assert.AreEqual(testString, testClassObj.Description);
        }

        [Test]
        public void ConsoleMessagesToString_Test()
        {
            var id = 10;
            var testString = "Test string";
            var testClassObj = new ConsoleMessages(id, testString);
            Assert.AreEqual(testString, testClassObj.ToString());
        }

        [Test]
        public void ConsoleMessagesGetAll_Test()
        {
            var a = MessagesEnumeration.GetAll<ConsoleMessages>();
            var id = 1;
            foreach (var i in a)
            {
                Assert.AreEqual(id, i.Id);
                id++;
            }
        }

        [Test]
        public void ConsoleMessagesEquals_Test()
        {
            var id = 10;
            var testString = "Test string";
            var testClassObj = new ConsoleMessages(id, testString);
            Assert.AreEqual(false, testClassObj.Equals(ConsoleMessages.DivisionByZero));
            Assert.AreEqual(true, testClassObj.Equals(testClassObj));
        }

        [Test]
        public void ConsoleMessagesGetHashCode_Test()
        {
            var messagesList = MessagesEnumeration.GetAll<ConsoleMessages>();
            var id = 1;
            foreach (var i in messagesList)
            {
                Assert.AreEqual(id.GetHashCode(), i.GetHashCode());
                id++;
            }
        }

        [Test]
        public void ConsoleMessagesCompareTo_Test()
        {
            var equalObjects = ConsoleMessages.Help.CompareTo(ConsoleMessages.Help);
            var objectGreaterThan = ConsoleMessages.Help.CompareTo(ConsoleMessages.DivisionByZero);
            var objectLesserThan = ConsoleMessages.Help.CompareTo(ConsoleMessages.Greeting);
            Assert.AreEqual(0, equalObjects);
            Assert.AreEqual(-1, objectGreaterThan);
            Assert.AreEqual(1, objectLesserThan);
        }
    }
}
