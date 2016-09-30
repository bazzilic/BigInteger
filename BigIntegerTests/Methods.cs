using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
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
        public void TestModPow()
        {
            var a = new BigInteger("4513022378190195207248111493619814210011122111521314021116172245292421892189133135249253284210917322371331631915863149241442281401995510735118116112172202199102116124234501111031274954151507124570516154178228146", 10);
            var n = new BigInteger("2529589762471071921217177179249254145111191246515169611931940652006643560213582062372288573701152271112332092452431128143210751781625037196701031611573185126122233723864211061301331715378213129937", 10);

            var modulus = new BigInteger(1000000007);
            var res = a.modPow(n, modulus);
            Assert.AreEqual(868041175, res.LongValue());

            modulus = new BigInteger("922305412110716620326228851918622717821243928922818234109110014149250211422482", 10);
            res = a.modPow(n, modulus);
            Assert.AreEqual(new BigInteger("676144631297564803799040568236788209319025642240115630978591468748134664779002", 10), res);
        }

        [TestMethod]
        public void TestModInverse()
        {

            var a = new BigInteger("470782681346529800216759025446747092045188631141622615445464429840250748896490263346676188477401449398784352124574498378830506322639352584202116605974693692194824763263949618703029846313252400361025245824301828641617858127932941468016666971398736792667282916657805322080902778987073711188483372360907612588995664533157503380846449774089269965646418521613225981431666593065726252482995754339317299670566915780168", 10);
            var b = a.modInverse(new BigInteger(1000000007));
            Assert.AreEqual(736445995, b.LongValue());

            b = a.modInverse(new BigInteger(1999));
            Assert.AreEqual(1814, b.LongValue());

            var isExceptionRaised = false;
            try
            {
                b = a.modInverse(new BigInteger(9999998));
            }
            catch (ArithmeticException)
            {
                isExceptionRaised = true;
            }
            Assert.IsTrue(isExceptionRaised);
        }

        [TestMethod]
        public void TestJacobi()
        {
            // Value generated from http://math.fau.edu/richman/jacobi.htm
            int[,] jcbTbl = {
                {1236, 20003, 1},
                {4513022, 3436547, -1},
                {9325399, 4679869, -1},
                {30, 5, 0}
            };

            BigInteger a, b;

            for (int i = 0; i < jcbTbl.Length / 3; i++)
            {
                a = new BigInteger(jcbTbl[i, 0]);
                b = new BigInteger(jcbTbl[i, 1]);
                Assert.AreEqual(jcbTbl[i, 2], BigInteger.Jacobi(a, b));
            }

        }

        [TestMethod]
        public void TestSecuredGenRandomBits()
        {
            { // Test < 32 bits
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();
                var rand = new Random();

                bi.genRandomBits(rand.Next(1, 33), rng);

                var bytes = bi.getBytes();
                Array.Reverse(bytes);
                var new_bytes = new byte[4];
                Array.Copy(bytes, new_bytes, bytes.Length);

                Assert.IsTrue(BitConverter.ToUInt32(new_bytes, 0) < (Math.Pow(2, 32) - 1));
            }

            // Test on random number of bits
            for (int i = 0; i < 99; i++)
            {
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();
                var rand = new Random();
                var bits = rand.Next(1, 70 * 32 + 1);
                bi.genRandomBits(bits, rng);
                Assert.AreEqual(bits, bi.bitCount());
            }

            { // Test upper boundary values
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();

                Exception exception = null;

                try
                {
                    bi.genRandomBits(2241, rng);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.IsNotNull(exception);

                bi.genRandomBits(2240, rng);
                Assert.AreEqual(70, bi.dataLength);

                bi.genRandomBits(2239, rng);
                Assert.AreEqual(70, bi.dataLength);
                Assert.AreEqual(2239, bi.bitCount());
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

            // Test arbitrary values 
            for (int i = 0; i < 99; i++)
            { 
                var bi = new BigInteger();
                var rng = new RNGCryptoServiceProvider();
                var rand = new Random();

                bi.genRandomBits(rand.Next(1, 32 * 69 + 1), rng);

                var coprime = bi.genCoPrime(rand.Next(1, 2241), rng);

                Assert.IsTrue((bi.gcd(coprime)).getBytes()[0] == 1);
            }
        }

        [TestMethod]
        public void TestAbs()
        {
            BigInteger bi;
            int val;
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                val = rand.Next(Int32.MinValue, Int32.MaxValue);
                bi = new BigInteger(val);
                Assert.AreEqual(Math.Abs(val), bi.abs());
            }

            bi = new BigInteger("4809238490238509385094809584086094850909458309580485093485093485094809580945809458340324342343242342343242", 10);
            Assert.AreEqual("4809238490238509385094809584086094850909458309580485093485093485094809580945809458340324342343242342343242", bi.abs().ToString());

            bi = new BigInteger();
            Assert.AreEqual(0, bi.abs());

            bi = new BigInteger("-38265236482749823794237948792386482364236462846234623862368236423764236624762384762384623862376482364823", 10);
            Assert.AreEqual("38265236482749823794237948792386482364236462846234623862368236423764236624762384762384623862376482364823", bi.abs().ToString());
        }

        [TestMethod]
        public void TestMax()
        {
            BigInteger bi1, bi2;
            int val1, val2;
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                val1 = rand.Next();
                val2 = rand.Next();
                bi1 = new BigInteger(val1);
                bi2 = new BigInteger(val2);

                Assert.AreEqual(Math.Max(val1, val2), bi1.max(bi2));
                Assert.AreEqual(bi1.max(bi2), bi2.max(bi1));
                Assert.AreEqual(bi1, bi1.max(bi1));
            }

            bi1 = new BigInteger("49823798573298479823749823798472398479238479823749823749823794837298472398469238649836294862398462398649823649823649823649823694", 10);
            bi2 = new BigInteger("-37209847385984792370497230948093284092384092380958058094809238409328049238094237094723094723984792384792387492379487239847239847923847923879847923847239847", 10);
            Assert.AreEqual(bi1, bi1.max(bi2));
            Assert.AreEqual(bi1, bi2.max(bi1));

            bi2 = new BigInteger();
            Assert.AreEqual(bi1, bi1.max(bi2));

            bi2 = new BigInteger("38274939234793749237498237492374982379872394798237", 10);
            Assert.AreEqual(bi1, bi2.max(bi1));

            bi2 = new BigInteger("49823798573298479823749823798472408479238479823749823749823794837298472398469238649836294862398462398649823649823649823649823694", 10);
            Assert.AreEqual(bi2, bi1.max(bi2));

            bi1 = new BigInteger("-9852375989470234802398402398409238049238094723094709234702387498237498623948623984623984623864237642376482376492386479238749823749237498237498237", 10);
            Assert.AreEqual(0, bi1.max(0));

            bi2 = new BigInteger("287498237498623846236236826386276327638276327632763276382763872688947329847923847982374982379482379847239847392847982374982374982374982374982374982379482379482379487239847239847", 10);
            Assert.AreEqual(bi2, bi2.max(bi1));

            bi2 = new BigInteger("-8979479943434898397", 10);
            Assert.AreEqual(bi2, bi1.max(bi2));

            bi2 = new BigInteger("-9852375989470234802399402398409238049238094723094709234702387498237498623948623984623984623864237642376482376492386479238749823749237498237498237", 10);
            Assert.AreEqual(bi1, bi2.max(bi1));

        }

        [TestMethod]
        public void TestMin()
        {
            BigInteger bi1, bi2;
            int val1, val2;
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                val1 = rand.Next();
                val2 = rand.Next();
                bi1 = new BigInteger(val1);
                bi2 = new BigInteger(val2);

                Assert.AreEqual(Math.Min(val1, val2), bi1.min(bi2));
                Assert.AreEqual(bi1.min(bi2), bi2.min(bi1));
                Assert.AreEqual(bi1, bi1.min(bi1));
            }

            bi1 = new BigInteger("49823798573298479823749823798472398479238479823749823749823794837298472398469238649836294862398462398649823649823649823649823694", 10);
            bi2 = new BigInteger("-37209847385984792370497230948093284092384092380958058094809238409328049238094237094723094723984792384792387492379487239847239847923847923879847923847239847", 10);
            Assert.AreEqual(bi2, bi1.min(bi2));
            Assert.AreEqual(bi2, bi2.min(bi1));

            bi2 = new BigInteger();
            Assert.AreEqual(bi2, bi1.min(bi2));

            bi2 = new BigInteger("38274939234793749237498237492374982379872394798237", 10);
            Assert.AreEqual(bi2, bi2.min(bi1));

            bi2 = new BigInteger("49823798573298479823749823798472408479238479823749823749823794837298472398469238649836294862398462398649823649823649823649823694", 10);
            Assert.AreEqual(bi1, bi1.min(bi2));

            bi1 = new BigInteger("-9852375989470234802398402398409238049238094723094709234702387498237498623948623984623984623864237642376482376492386479238749823749237498237498237", 10);
            Assert.AreEqual(bi1, bi1.min(0));

            bi2 = new BigInteger("287498237498623846236236826386276327638276327632763276382763872688947329847923847982374982379482379847239847392847982374982374982374982374982374982379482379482379487239847239847", 10);
            Assert.AreEqual(bi1, bi2.min(bi1));

            bi2 = new BigInteger("-8979479943434898397", 10);
            Assert.AreEqual(bi1, bi1.min(bi2));

            bi2 = new BigInteger("-9852375989470234802399402398409238049238094723094709234702387498237498623948623984623984623864237642376482376492386479238749823749237498237498237", 10);
            Assert.AreEqual(bi2, bi2.min(bi1));
        }

        [TestMethod]
        public void TestSqrt()
        {
            BigInteger bi;
            int val, sqrtVal;
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                val = rand.Next();
                bi = new BigInteger(val);
                sqrtVal = (int)Math.Floor(Math.Sqrt(val));
                Assert.AreEqual(sqrtVal, bi.sqrt());
            }

            bi = new BigInteger();
            Assert.AreEqual(0, bi.sqrt());

            bi = new BigInteger("48234798239584935745984795837", 10);
            Assert.AreEqual(219624220521291, bi.sqrt());

            bi = new BigInteger("4823479823958493574598479580945895480904590958034958034580948509485094850934095809458408509485094850948509803459834037", 10);
            Assert.AreEqual("69451276618637425696010359184467375646677653070095660334837", bi.sqrt().ToString());

            bi = new BigInteger("902380594730957598498379487239749823749832749823749823759823759823649623984623974627682368236423764823649823749823749823794872398472398479238479382749823794823794823749823794823794872398479238479823749823749823749823749823749823740239480293840923804923804923809482304982", 10);
            Assert.AreEqual("949937153042746085485800690340716910200218535446376464883006159759187016711766033117259286191698487700345112712284215083646265481183724", bi.sqrt().ToString());
        }

        [TestMethod]
        public void TestGCD()
        {
            BigInteger bi1, bi2;
            int val1, val2;
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                val1 = rand.Next();
                val2 = rand.Next();
                bi1 = new BigInteger(val1);
                bi2 = new BigInteger(val2);

                Assert.AreEqual(GCD(val1, val2), bi1.gcd(bi2));
                Assert.AreEqual(bi1.gcd(bi2), bi2.gcd(bi1));
            }

            bi1 = new BigInteger("23479237493274982374983729847392847928347982374983795749598459895479485945984598949799486346632864782376823768236482364862624623864", 10);
            Assert.AreEqual(bi1, bi1.gcd(0));
            Assert.AreEqual(1, bi1.gcd(1));
            Assert.AreEqual(1, bi1.gcd(-1));

            bi2 = new BigInteger("3294823794872398749835984985798575794759834759347593475983475983475949530439", 10);
            Assert.AreEqual(1, bi2.gcd(bi1));

            bi2 = new BigInteger(2839392890293);
            Assert.AreEqual(1, bi1.gcd(bi2));

            bi1 = new BigInteger("4951870740493721842141443925495861658429914087387823242795626852731793395869583123486587097315594003541474986183101777497261582259131154425", 10);
            bi2 = new BigInteger(25208378845650);
            Assert.AreEqual(12604189422825, bi2.gcd(bi1));
            Assert.AreEqual(bi1.gcd(bi2), bi2.gcd(bi1));

            bi2 = -bi2;
            Assert.AreEqual(12604189422825, bi2.gcd(bi1));
            Assert.AreEqual(bi1.gcd(bi2), bi2.gcd(bi1));
        }

        private int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (true)
            {
                int remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            }
        }

        [TestMethod]
        // This assumes MAX_LENGTH of BigInteger is 70
        public void TestToHexString()
        {
            BigInteger bi;
            byte[] buffer = new byte[8];
            long val;
            string valHexString;

            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {

                rand.NextBytes(buffer);
                val = BitConverter.ToInt64(buffer, 0);

                bi = new BigInteger(val);


                valHexString = val.ToString("X");
                if (val < 0)
                {
                    while (valHexString.Length < 8 * 70)
                    {
                        valHexString = "F" + valHexString;
                    }
                }

                Assert.AreEqual(valHexString, bi.ToHexString());
            }

            // Test on big number
            bi = new BigInteger("9329857983749832748932749873298479328748923759347985734985739749327498327492387498237498237493794872394723947923749823759347598475983475943759843759834759834759374975984375984375934759437593", 10);
            Assert.AreEqual("86042e915cdcf19902845ddf57c6b604685c53a01da858573f52219e1c743fc193e5b56aaba29ef308a82cd26da8066d1ae2af170b1443f3b539938966107f8f77263e4f13fb815049d5f746438519".ToUpper(),
                bi.ToHexString());
        }
    }
}
