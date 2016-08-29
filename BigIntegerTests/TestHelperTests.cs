using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigIntegerTests
{
    /// <summary>
    /// Summary description for TestHelperTests
    /// </summary>
    [TestClass]
    public class TestHelperTests
    {
        public TestHelperTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCompareNumStr()
        {
            Assert.AreEqual(TestHelper.CompareNumStr("102", "102"), 0);
            Assert.AreEqual(TestHelper.CompareNumStr("-102", "102"), -1);
            Assert.AreEqual(TestHelper.CompareNumStr("108", "102"), 1);
            Assert.AreEqual(TestHelper.CompareNumStr("1025", "102"), 1);
            Assert.AreEqual(TestHelper.CompareNumStr("102", "1223"), -1);
            Assert.AreEqual(TestHelper.CompareNumStr("-102", "-102"), 0);
            Assert.AreEqual(TestHelper.CompareNumStr("1", "-102"), 1);

            int num1 = 0, num2 = 0;
            var rand = new Random();
            for (int i = 0; i < TestHelper.NUM_OF_TIMES_FOR_RANDOM; i++)
            {
                num1 = rand.Next();
                if (rand.Next(100) > 50)
                    num1 = -num1;
                num2 = rand.Next();
                if (rand.Next(100) > 50)
                    num2 = -num2;

                switch(TestHelper.CompareNumStr(num1.ToString(), num2.ToString()))
                {
                    case 1:
                        Assert.IsTrue(num1 > num2);
                        break;
                    case -1:
                        Assert.IsTrue(num1 < num2);
                        break;
                    case 0:
                        Assert.IsTrue(num1 == num2);
                        break;
                    default:
                        Assert.IsFalse(true);
                        break;
                }

            }
        }

        [TestMethod]
        public void TestGenRandNumStr()
        {
            Random rand = new Random();
            string str1;
            Int64 num1 = 9223372036854775807;
            for (int i = 0; i < TestHelper.NUM_OF_TIMES_FOR_RANDOM; i++)
            {
                str1 = TestHelper.GenRandNumStr(rand.Next(18));
                Assert.IsTrue(Int64.TryParse(str1, out num1));
                Assert.IsTrue(str1 == num1.ToString());
            }
        }
    }
}
