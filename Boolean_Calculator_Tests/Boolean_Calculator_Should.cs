using APD_BooleanCalculator;
using NUnit.Framework;

namespace Boolean_Calculator_Tests
{
    public class Boolean_Calculator_Should
    {
        [Test]
        [TestCase("TRUE", true)]
        [TestCase("FALSE", false)]
        [TestCase("NOT FALSE", true)]
        [TestCase("NOT TRUE", false)]
        [TestCase("TRUE AND TRUE", true)]
        [TestCase("TRUE AND TRUE AND TRUE", true)]
        [TestCase("TRUE AND TRUE AND FALSE", false)]
        [TestCase("TRUE AND FALSE", false)]
        [TestCase("FALSE AND FALSE", false)]
        [TestCase("TRUE OR TRUE", true)]
        [TestCase("TRUE OR FALSE", true)]
        [TestCase("FALSE OR FALSE", false)]
        [TestCase("TRUE OR TRUE AND FALSE", true)]
        [TestCase("TRUE OR TRUE OR TRUE AND FALSE", true)]
        [TestCase("TRUE OR FALSE AND NOT FALSE", true)]
        [TestCase("(TRUE)", true)]
        [TestCase("(FALSE)", false)]
        [TestCase("((FALSE))", false)]
        [TestCase("(FALSE) OR TRUE", true)]
        [TestCase("(FALSE) AND TRUE", false)]
        [TestCase("(FALSE OR TRUE) AND TRUE", true)]
        [TestCase("(FALSE OR TRUE) AND (TRUE OR FALSE)", true)]
        [TestCase("(TRUE OR TRUE OR TRUE) AND FALSE", false)]
        [TestCase("NOT (TRUE AND TRUE)", false)]
        public void return_expected_boolean_for_string_input(string boolInput, bool expected)
        {
            Assert.That(BooleanCalculator.Parse(boolInput), Is.EqualTo(expected));
        }
    }
}