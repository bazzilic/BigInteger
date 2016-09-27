BigInteger class for C#
=======================
![AppVeyor Build Badge](https://ci.appveyor.com/api/projects/status/4vvqx57s6owe507u?svg=true)

This is a continuation of work on the BigInteger implementation for C# initially created by Chew Keong TAN.
This implementation was compared to the .NET implementation (available in `System.Numerics` since .NET 4.0) and to the [Bouncy Castle implementation](http://www.bouncycastle.org/csharp/).
This library appeared to be significantly faster than other implementations and is compatible with .NET 2.0+.

Pull requests are welcome and appreciated.

Original description of the class is located here: http://www.codeproject.com/Articles/2728/C-BigInteger-Class

Release NuGet packages feed is availbale at [NuGet gallery](https://www.nuget.org/packages/BigInteger/).
Developer versions are available through this feed [https://ci.appveyor.com/nuget/bazzilic-biginteger](https://ci.appveyor.com/nuget/bazzilic-biginteger).
Developer versions are matched with `master` branch of this repository.
You can install the package using Package Manager Console by executing `Install-Package BigInteger`.

Refer to [Changelog.txt](https://raw.githubusercontent.com/bazzilic/BigInteger/master/Changelog.txt) for the changes to the current version.

TODO
----
* ~~Fix known bugs~~
* ~~Add testing suite~~
* ~~Create a NuGet package~~
* ~~Do more or less full code coverage~~
* Sign the DLL for NuGet (?)
* Analyze performance

Old README (as of version 1.0.3)
--------------------------------

BigInteger Class Version 1.03

Copyright (c) 2002 Chew Keong TAN
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, and/or sell copies of the Software, and to permit persons
to whom the Software is furnished to do so, provided that the above
copyright notice(s) and this permission notice appear in all copies of
the Software and that both the above copyright notice(s) and this
permission notice appear in supporting documentation.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
OF THIRD PARTY RIGHTS. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
HOLDERS INCLUDED IN THIS NOTICE BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL
INDIRECT OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING
FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT,
NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION
WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.


### Disclaimer ###

Although reasonable care has been taken to ensure the correctness of this
implementation, this code should never be used in any application without
proper verification and testing.  I disclaim all liability and responsibility
to any person or entity with respect to any loss or damage caused, or alleged
to be caused, directly or indirectly, by the use of this BigInteger class.

Comments, bugs and suggestions to
(http:www.codeproject.com/csharp/biginteger.asp)


Overloaded Operators +, -, *, /, %, >>, <<, ==, !=, >, <, >=, <=, &, |, ^, ++, --, ~

### Features ###

1. Arithmetic operations involving large signed integers (2's complement).
2. Primality test using Fermat little theorm, Rabin Miller's method,
   Solovay Strassen's method and Lucas strong pseudoprime.
3. Modulo exponential with Barrett's reduction.
4. Inverse modulo.
5. Pseudo prime generation.
6. Co-prime generation.


### Known Problem ###

This pseudoprime passes my implementation of
primality test but failed in JDK's isProbablePrime test.

      byte[] pseudoPrime1 = { (byte)0x00,
            (byte)0x85, (byte)0x84, (byte)0x64, (byte)0xFD, (byte)0x70, (byte)0x6A,
            (byte)0x9F, (byte)0xF0, (byte)0x94, (byte)0x0C, (byte)0x3E, (byte)0x2C,
            (byte)0x74, (byte)0x34, (byte)0x05, (byte)0xC9, (byte)0x55, (byte)0xB3,
            (byte)0x85, (byte)0x32, (byte)0x98, (byte)0x71, (byte)0xF9, (byte)0x41,
            (byte)0x21, (byte)0x5F, (byte)0x02, (byte)0x9E, (byte)0xEA, (byte)0x56,
            (byte)0x8D, (byte)0x8C, (byte)0x44, (byte)0xCC, (byte)0xEE, (byte)0xEE,
            (byte)0x3D, (byte)0x2C, (byte)0x9D, (byte)0x2C, (byte)0x12, (byte)0x41,
            (byte)0x1E, (byte)0xF1, (byte)0xC5, (byte)0x32, (byte)0xC3, (byte)0xAA,
            (byte)0x31, (byte)0x4A, (byte)0x52, (byte)0xD8, (byte)0xE8, (byte)0xAF,
            (byte)0x42, (byte)0xF4, (byte)0x72, (byte)0xA1, (byte)0x2A, (byte)0x0D,
            (byte)0x97, (byte)0xB1, (byte)0x31, (byte)0xB3,
      };


### Change Log ###

1. September 23, 2002 (Version 1.03)
   - Fixed operator- to give correct data length.
   - Added Lucas sequence generation.
   - Added Strong Lucas Primality test.
   - Added integer square root method.
   - Added setBit/unsetBit methods.
   - New isProbablePrime() method which do not require the
     confident parameter.

2. August 29, 2002 (Version 1.02)
   - Fixed bug in the exponentiation of negative numbers.
   - Faster modular exponentiation using Barrett reduction.
   - Added getBytes() method.
   - Fixed bug in ToHexString method.
   - Added overloading of ^ operator.
   - Faster computation of Jacobi symbol.

3. August 19, 2002 (Version 1.01)
   - Big integer is stored and manipulated as unsigned integers (4 bytes) instead of
     individual bytes this gives significant performance improvement.
   - Updated Fermat's Little Theorem test to use a^(p-1) mod p = 1
   - Added isProbablePrime method.
   - Updated documentation.

4. August 9, 2002 (Version 1.0)
   - Initial Release.


### References ###

[1] D. E. Knuth, "Seminumerical Algorithms", The Art of Computer Programming Vol. 2,
    3rd Edition, Addison-Wesley, 1998.

[2] K. H. Rosen, "Elementary Number Theory and Its Applications", 3rd Ed,
    Addison-Wesley, 1993.

[3] B. Schneier, "Applied Cryptography", 2nd Ed, John Wiley & Sons, 1996.

[4] A. Menezes, P. van Oorschot, and S. Vanstone, "Handbook of Applied Cryptography",
    CRC Press, 1996, www.cacr.math.uwaterloo.ca/hac

[5] A. Bosselaers, R. Govaerts, and J. Vandewalle, "Comparison of Three Modular
    Reduction Functions," Proc. CRYPTO'93, pp.175-186.

[6] R. Baillie and S. S. Wagstaff Jr, "Lucas Pseudoprimes", Mathematics of Computation,
    Vol. 35, No. 152, Oct 1980, pp. 1391-1417.

[7] H. C. Williams, "�douard Lucas and Primality Testing", Canadian Mathematical
    Society Series of Monographs and Advance Texts, vol. 22, John Wiley & Sons, New York,
    NY, 1998.

[8] P. Ribenboim, "The new book of prime number records", 3rd edition, Springer-Verlag,
    New York, NY, 1995.

[9] M. Joye and J.-J. Quisquater, "Efficient computation of full Lucas sequences",
    Electronics Letters, 32(6), 1996, pp 537-538.
