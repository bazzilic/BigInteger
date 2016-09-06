using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

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

        [TestMethod]
        public void TestSecuredGenRandomBits()
        {
            { // Test < 32 bits
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();
                var rand = new Random();

                bi.genRandomBits(rand.Next(33), rng);

                var bytes = bi.getBytes();
                Array.Reverse(bytes);
                var new_bytes = new byte[4];
                Array.Copy(bytes, new_bytes, bytes.Length);

                Assert.IsTrue(BitConverter.ToUInt32(new_bytes, 0) < (Math.Pow(2, 32) - 1));
            }

            { // Test upper boundary values
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();

                Exception exception = null;

                try
                {
                    bi.genRandomBits(2241, rng);
                }
                catch(Exception ex)
                {
                    exception = ex;
                }

                Assert.IsNotNull(exception);

                bi.genRandomBits(2240, rng);
                Assert.AreEqual(70, bi.dataLength);

                bi.genRandomBits(2239, rng);
                Assert.AreEqual(70, bi.dataLength);
            }

            { // Test lower boudary value
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();

                bi.genRandomBits(1, rng);
                Assert.IsTrue(bi.getBytes()[0] == 1 || bi.getBytes()[0] == 0);
            }
        }

        [TestMethod]
        public void TestGenCoPrime()
        {
            { // Test small values
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();

                bi.genRandomBits(100, rng);

                var coprime = bi.genCoPrime(10, rng);

                Assert.IsTrue((bi.gcd(coprime)).getBytes()[0] == 1);
            }

            { // Test arbitrary values 
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();
                var rand = new Random();

                bi.genRandomBits(rand.Next(2241), rng);

                var coprime = bi.genCoPrime(rand.Next(2241), rng);

                Assert.IsTrue((bi.gcd(coprime)).getBytes()[0] == 1);
            }
        }
    }
}
