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


        [TestMethod()]
        public void modPowTest()
        {
            BigInteger a = new BigInteger("4513022378190195207248111493619814210011122111521314021116172245292421892189133135249253284210917322371331631915863149241442281401995510735118116112172202199102116124234501111031274954151507124570516154178228146", 10);
            BigInteger n = new BigInteger("2529589762471071921217177179249254145111191246515169611931940652006643560213582062372288573701152271112332092452431128143210751781625037196701031611573185126122233723864211061301331715378213129937", 10);

            BigInteger modulus = new BigInteger(1000000007);
            BigInteger res = a.modPow(n, modulus);
            Assert.AreEqual(868041175, res.LongValue());

            modulus = new BigInteger("922305412110716620326228851918622717821243928922818234109110014149250211422482", 10);
            res = a.modPow(n, modulus);
            Assert.AreEqual(new BigInteger("676144631297564803799040568236788209319025642240115630978591468748134664779002", 10), res);
        }

        [TestMethod()]
        public void modInverseTest()
        {

            BigInteger a = new BigInteger("470782681346529800216759025446747092045188631141622615445464429840250748896490263346676188477401449398784352124574498378830506322639352584202116605974693692194824763263949618703029846313252400361025245824301828641617858127932941468016666971398736792667282916657805322080902778987073711188483372360907612588995664533157503380846449774089269965646418521613225981431666593065726252482995754339317299670566915780168", 10);
            BigInteger b = a.modInverse(new BigInteger(1000000007));
            Assert.AreEqual(736445995, b.LongValue());

            b = a.modInverse(new BigInteger(1999));
            Assert.AreEqual(1814, b.LongValue());

            try
            {
                b = a.modInverse(new BigInteger(9999998));
            }
            catch (ArithmeticException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void JacobiTest()
        {
            // Value generated from http://math.fau.edu/richman/jacobi.htm
            int[,] jcbTbl = { 
                { 1236, 20003, 1 }, 
                { 4513022, 3436547, -1 },  
                { 9325399, 4679869, -1 },
                { 30, 5, 0 }
            };

            BigInteger a, b;

            for (int i = 0; i < jcbTbl.Length / 3; i++)
            {
                a = new BigInteger(jcbTbl[i, 0]);
                b = new BigInteger(jcbTbl[i, 1]);
                Assert.AreEqual(jcbTbl[i, 2], BigInteger.Jacobi(a, b));
            }

        }
    }
}
