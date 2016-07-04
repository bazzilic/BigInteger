using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigIntegerTests
{
    [TestClass]
    public class Constructors
    {
        [TestMethod]
        public void ctor__Byte_Int32_Int32()
        {
            var rnd = new Random();

            for (var i = 0; i < 500; i++)
            {
                var n = new BigInteger(1);
                n = n * rnd.Next(1, 1000);
                var bytes = n.getBytes();
                var test_bi = new BigInteger(bytes);
                Assert.AreEqual(n, test_bi);
            }
        }
    }
}
