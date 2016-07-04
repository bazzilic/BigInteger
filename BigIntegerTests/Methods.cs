using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;

namespace BigIntegerTests
{
    [TestClass]
    public class Methods
    {
        [TestMethod]
        public void TestBitCount()
        {
            { // Randomized tests
                var r = new Random();

                for (int j = 0; j < 512; j++)
                {
                    var length = r.Next(70 * sizeof(uint) * 8 - 2); // TODO: 70 - current maxLength, remove hardcoded value

                    var sb = new StringBuilder("1");
                    for (var i = 0; i < length; i++)
                    {
                        sb.Append(r.Next() % 2 == 0 ? "1" : "0");
                    }

                    var bits = sb.Length;

                    var bi = new BigInteger(sb.ToString(), 2);

                    var bitsCounted = bi.bitCount();

                    Assert.AreEqual(bits, bitsCounted);
                }
            }

            { // Special cases - zero, one
                var z = new BigInteger(0);
                var bitsCounted = z.bitCount();
                Assert.AreEqual(bitsCounted, 1);

                var o = new BigInteger(1);
                bitsCounted = o.bitCount();
                Assert.AreEqual(bitsCounted, 1);
            }
        }

        [TestMethod]
        public void TestGetBytes()
        {
            { // Randomized tests
                var r = new Random();

                for (int j = 0; j < 512; j++)
                {
                    var length = r.Next(1, 256);
                    var byte_array = new byte[length];
                    r.NextBytes(byte_array);
                    if (byte_array[0] == 0)
                        byte_array[0] = (byte)r.Next(1, 255);

                    var bi = new BigInteger(byte_array);

                    var bytes_got_back = bi.getBytes();

                    Assert.AreEqual(bytes_got_back.Length, byte_array.Length);

                    for (var i = 0; i < byte_array.Length; i++)
                        Assert.AreEqual(bytes_got_back[i], byte_array[i]);
                }
            }

            { // Special case - zero
                var z = new BigInteger(0);

                var zero_bytes = z.getBytes();

                Assert.AreNotEqual(zero_bytes.Length, 0);
                Assert.AreEqual(zero_bytes[0], (byte)0);
            }
        }
    }
}
