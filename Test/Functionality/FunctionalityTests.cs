using NUnit.Framework;

using Precision;

namespace Test.Functionality
{
    [TestFixture]
    public class FunctionalityTests
    {
        public FunctionalityTests()
        {
        }

        [TestCase(0, 0, 0, 0, ExpectedResult = 0)]
        [TestCase(1, 0, 0, 0, ExpectedResult = 3)]
        [TestCase(0, 0, 1, 0, ExpectedResult = 3)]
        [TestCase(1, 0, 1, 0, ExpectedResult = 3)]

        [TestCase(1, 0, 2, 0, ExpectedResult = new Fact128(3, 0))]
        public Fact128 Fact128_Addition_Test(long value1, ulong value2, long value3, ulong value4)
        {
            var a = new Fact128(value1, value2);
            var b = new Fact128(value3, value4);

            return a + b;
        }
    }
}
