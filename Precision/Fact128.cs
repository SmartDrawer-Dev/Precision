using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Precision
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
    public struct Fact128
        : IComparable<Fact128>
        , IComparable
        , IEquatable<Fact128>
        , IFormattable
    {
        #region Constants & Pre-Calculated Values

        private static readonly ulong f = 121645100408832000;

        private static readonly ulong OverflowOffset =
              0x23443B2A19083DCD;

        private static readonly ulong OverflowMask =
              0x23443B2A19083DCD;

        private static readonly ulong OverflowCheck =
              0x23443B2A19083DCD;

        /// <summary>
        /// 
        /// </summary>
        private static readonly uint[] DigitMaxValue =
        {
              0x00000001, 0x00000002, 0x00000003, 0x00000004, 0x00000005, 0x00000006
            , 0x00000007, 0x00000008, 0x00000009, 0x0000000A, 0x0000000B, 0x0000000C
            , 0x0000000D, 0x0000000E, 0x0000000F, 0x00000010, 0x00000011, 0x00000012
        };
        
        /// <summary>
        /// 
        /// </summary>
        private static readonly uint[] DigitBits  = 
        {
              0x00000001, 0x00000002, 0x00000002, 0x00000003, 0x00000003, 0x00000003
            , 0x00000003, 0x00000004, 0x00000004, 0x00000004, 0x00000004, 0x00000004
            , 0x00000004, 0x00000004, 0x00000004, 0x00000005, 0x00000005, 0x00000005
        };

        /// <summary>
        /// 
        /// </summary>
        private static readonly ulong[] DigitMask =
        {
              0x8000000000000000, 0x6000000000000000, 0x1800000000000000, 0x0700000000000000, 0x00E0000000000000, 0x001C000000000000
            , 0x0003800000000000, 0x0000780000000000, 0x0000078000000000, 0x0000007800000000, 0x0000000780000000, 0x0000000078000000
            , 0x0000000007800000, 0x0000000000780000, 0x0000000000078000, 0x0000000000007C00, 0x00000000000003E0, 0x000000000000001F

            //  0x0000000000000001, 0x0000000000000006, 0x0000000000000018, 0x00000000000000E0, 0x0000000000000700, 0x0000000000003800
            //, 0x000000000001C000, 0x00000000001E0000, 0x0000000001E00000, 0x000000001E000000, 0x00000001E0000000, 0x0000001E00000000
            //, 0x000001E000000000, 0x00001E0000000000, 0x0001E00000000000, 0x003E000000000000, 0x07C0000000000000, 0xF800000000000000
        };

        /// <summary>
        /// 
        /// </summary>
        private static readonly ulong[] DigitOverflow =
        {
              0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
        };

        /// <summary>
        /// 
        /// </summary>
        private static readonly ulong[] FractionalDenominatorValue =
        {
              0x0000000000000002, 0x0000000000000006, 0x0000000000000018, 0x0000000000000078, 0x00000000000002D0, 0x00000000000013B0
            , 0x0000000000009D80, 0x0000000000058980, 0x0000000000375F00, 0x0000000002611500, 0x000000001C8CFC00, 0x000000017328CC00
            , 0x000000144C3B2800, 0x0000013077775800, 0x0000130777758000, 0x0001437EEECD8000, 0x0016BEECCA730000, 0x01B02B9306890000
        };

        /// <summary>
        /// 
        /// </summary>
        private static readonly ulong[] FractionalNumeratorValue =
        {
              0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
        };

        /// <summary>
        /// 
        /// </summary>
        private static readonly ulong[,] FractionalNumeratorDigitValue =
        {
            { 0x0000000000000001, 0x0000000000000003, 0x000000000000000C, 0x000000000000003C, 0x0000000000000168, 0x00000000000009D8
            , 0x0000000000004EC0, 0x000000000002C4C0, 0x00000000001BAF80, 0x0000000001308A80, 0x000000000E467E00, 0x00000000B9946600
            , 0x0000000A261D9400, 0x000000983BBBAC00, 0x00000983BBBAC000, 0x0000A1BF7766C000, 0x00B5F76653980000, 0x00D815C983448000 },

            { 0x0000000000000000, 0x0000000000000001, 0x0000000000000004, 0x0000000000000014, 0x0000000000000078, 0x0000000000000348
            , 0x0000000000001A40, 0x000000000000EC40, 0x0000000000093A80, 0x0000000000658380, 0x0000000004C22A00, 0x000000003DDC2200
            , 0x000000036209DC00, 0x00000032BE93E400, 0x0000032BE93E4000, 0x000035EA7D224000, 0x0003CA7CCC688000, 0x004807432BC18000 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000005, 0x000000000000001E, 0x00000000000000D2
            , 0x0000000000000690, 0x0000000000003B10, 0x0000000000024EA0, 0x00000000001960E0, 0x0000000001308A80, 0x000000000F770880
            , 0x00000000D8827700, 0x0000000CAFA4F900, 0x000000CAFA4F9000, 0x00000E4599982000, 0x000100E4CCB24000, 0x001310FB313AC000 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000006, 0x000000000000002A
            , 0x0000000000000150, 0x0000000000000BD0, 0x0000000000007620, 0x0000000000051360, 0x00000000003CE880, 0x000000000317CE80
            , 0x000000002B4D4B00, 0x0000000289876500, 0x0000002898765000, 0x000002B21FDB5000, 0x000030863D6BA000, 0x000399F68EFCE000 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000007
            , 0x0000000000000038, 0x00000000000001F8, 0x00000000000013B0, 0x000000000000D890, 0x00000000000A26C0, 0x000000000083F7C0
            , 0x0000000007378C80, 0x000000006C413B80, 0x00000006C413B800, 0x00000073054F3800, 0x000008165F91F000, 0x000099A917D4D000 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001
            , 0x0000000000000008, 0x0000000000000048, 0x00000000000002D0, 0x0000000000001EF0, 0x0000000000017340, 0x000000000012DA40
            , 0x000000000107EF80, 0x000000000F770880, 0x00000000F7708800, 0x000000106E790800, 0x00000127C4829000, 0x000015F395B0B000 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000001, 0x0000000000000009, 0x000000000000005A, 0x00000000000003DE, 0x0000000000002E68, 0x0000000000025B48
            , 0x000000000020FDF0, 0x0000000001EEE110, 0x000000001EEE1100, 0x00000022EAC13100, 0x0000027481957200, 0x00002EA59E177600 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000001, 0x000000000000000A, 0x000000000000006E, 0x0000000000000528, 0x0000000000004308
            , 0x000000000003AA70, 0x000000000036FC90, 0x00000000036FC900, 0x000000003A6C5900, 0x000000041B9E4200, 0x0000004E0CBEE600 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x000000000000000B, 0x0000000000000084, 0x00000000000006B4
            , 0x0000000000005DD8, 0x0000000000057FA8, 0x000000000057FA80, 0x0000000005D7A280, 0x0000000069296D00, 0x00000007CE131700 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x000000000000000C, 0x000000000000009C
            , 0x0000000000000888, 0x0000000000007FF8, 0x000000000007FF80, 0x000000000087F780, 0x00000000098F6700, 0x00000000B5A4A500 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x000000000000000D
            , 0x00000000000000B6, 0x0000000000000AAA, 0x000000000000AAA0, 0x00000000000B54A0, 0x0000000000CBF340, 0x000000000F230DC0 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001
            , 0x000000000000000E, 0x00000000000000D2, 0x0000000000000D20, 0x000000000000DF20, 0x00000000000FB040, 0x00000000012A14C0 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000001, 0x000000000000000F, 0x00000000000000F0, 0x0000000000000FF0, 0x0000000000011EE0, 0x0000000000154AA0 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000001, 0x0000000000000010, 0x0000000000000110, 0x0000000000001320, 0x0000000000016B60 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000011, 0x0000000000000132, 0x00000000000016B6 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000012, 0x0000000000000156 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000013 },

            { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000
            , 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001 }
        };

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private long Integer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(8)]
        private ulong Fraction;

        #region Constructors

        public Fact128(long integer = 0, ulong fraction = 0)
        {
            Integer = integer;
            Fraction = fraction;
        }

        public Fact128(float value)
        {
            Integer = Convert.ToInt64(value);
            Fraction = 0;
        }

        public Fact128(double value)
        {
            Integer = Convert.ToInt64(value);
            Fraction = 0;
        }

        public Fact128(decimal value)
        {
            Integer = Convert.ToInt64(value);
            Fraction = 0;
        }

        public Fact128(int value)
        {
            Integer = value;
            Fraction = 0;
        }

        public Fact128(uint value)
        {
            Integer = Convert.ToInt64(value);
            Fraction = 0;
        }

        #endregion

        #region Operations
        
        #region Unary

        public static Fact128 operator +(Fact128 a)
        {
            return new Fact128(+a.Integer, a.Fraction);
        }

        public static Fact128 operator -(Fact128 a)
        {
            return new Fact128(-a.Integer, a.Fraction);
        }

        public static Fact128 operator ++(Fact128 a)
        {
            return new Fact128(a.Integer + 1, a.Fraction);
        }

        public static Fact128 operator --(Fact128 a)
        {
            return new Fact128(a.Integer - 1, a.Fraction);
        }

        #endregion

        #region Binary
        
        /// <summary>
        /// Perform addition between a and b
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns></returns>
        public static Fact128 operator +(Fact128 a, Fact128 b)
        {
            a.Integer += b.Integer;
            a.Fraction = Add(a.Fraction, b.Fraction);

            if (a.Fraction < b.Fraction)
                a.Integer++;

            return a;
        }

        private static ulong Add(ulong a, ulong b)
        {
            a += b;
            b = (a + OverflowOffset) & OverflowMask;

            if (b > 0)
                Add(a, b);

            return a;
        }

        public static Fact128 operator -(Fact128 a, Fact128 b)
        {
            var fraction = a.Fraction - b.Fraction;

            return new Fact128(a.Integer - b.Integer, fraction);
        }

        public static Fact128 operator *(Fact128 a, Fact128 b)
        {
            var fraction = a.Fraction * b.Fraction;

            return new Fact128(a.Integer * b.Integer, fraction);
        }

        public static Fact128 operator /(Fact128 a, Fact128 b)
        {
            var fraction = a.Fraction / b.Fraction;

            return new Fact128(a.Integer / b.Integer, fraction);
        }

        public static Fact128 operator %(Fact128 a, Fact128 b)
        {
            var fraction = a.Fraction % b.Fraction;

            return new Fact128(a.Integer % b.Integer, fraction);
        }

        #region Conversions
        
        public static Fact128 operator +(Fact128 a, double b)  { return a + new Fact128(b); }
        public static Fact128 operator +(Fact128 a, float b)   { return a + new Fact128(b); }
        public static Fact128 operator +(Fact128 a, decimal b) { return a + new Fact128(b); }
        public static Fact128 operator +(Fact128 a, int b)     { return a + new Fact128(b); }
        public static Fact128 operator +(Fact128 a, uint b)    { return a + new Fact128(b); }
        
        public static Fact128 operator -(Fact128 a, double b)  { return a - new Fact128(b); }
        public static Fact128 operator -(Fact128 a, float b)   { return a - new Fact128(b); }
        public static Fact128 operator -(Fact128 a, decimal b) { return a - new Fact128(b); }
        public static Fact128 operator -(Fact128 a, int b)     { return a - new Fact128(b); }
        public static Fact128 operator -(Fact128 a, uint b)    { return a - new Fact128(b); }
        
        public static Fact128 operator *(Fact128 a, double b)  { return a * new Fact128(b); }
        public static Fact128 operator *(Fact128 a, float b)   { return a * new Fact128(b); }
        public static Fact128 operator *(Fact128 a, decimal b) { return a * new Fact128(b); }
        public static Fact128 operator *(Fact128 a, int b)     { return a * new Fact128(b); }
        public static Fact128 operator *(Fact128 a, uint b)    { return a * new Fact128(b); }
        
        public static Fact128 operator /(Fact128 a, double b)  { return a / new Fact128(b); }
        public static Fact128 operator /(Fact128 a, float b)   { return a / new Fact128(b); }
        public static Fact128 operator /(Fact128 a, decimal b) { return a / new Fact128(b); }
        public static Fact128 operator /(Fact128 a, int b)     { return a / new Fact128(b); }
        public static Fact128 operator /(Fact128 a, uint b)    { return a / new Fact128(b); }
        
        public static Fact128 operator %(Fact128 a, double b)  { return a % new Fact128(b); }
        public static Fact128 operator %(Fact128 a, float b)   { return a % new Fact128(b); }
        public static Fact128 operator %(Fact128 a, decimal b) { return a % new Fact128(b); }
        public static Fact128 operator %(Fact128 a, int b)     { return a % new Fact128(b); }
        public static Fact128 operator %(Fact128 a, uint b)    { return a % new Fact128(b); }
        
        public static Fact128 operator +(double a, Fact128 b)  { return new Fact128(a) + b; }
        public static Fact128 operator +(float a, Fact128 b)   { return new Fact128(a) + b; }
        public static Fact128 operator +(decimal a, Fact128 b) { return new Fact128(a) + b; }
        public static Fact128 operator +(int a, Fact128 b)     { return new Fact128(a) + b; }
        public static Fact128 operator +(uint a, Fact128 b)    { return new Fact128(a) + b; }

        public static Fact128 operator -(double a, Fact128 b)  { return new Fact128(a) - b; }
        public static Fact128 operator -(float a, Fact128 b)   { return new Fact128(a) - b; }
        public static Fact128 operator -(decimal a, Fact128 b) { return new Fact128(a) - b; }
        public static Fact128 operator -(int a, Fact128 b)     { return new Fact128(a) - b; }
        public static Fact128 operator -(uint a, Fact128 b)    { return new Fact128(a) - b; }

        public static Fact128 operator *(double a, Fact128 b)  { return new Fact128(a) * b; }
        public static Fact128 operator *(float a, Fact128 b)   { return new Fact128(a) * b; }
        public static Fact128 operator *(decimal a, Fact128 b) { return new Fact128(a) * b; }
        public static Fact128 operator *(int a, Fact128 b)     { return new Fact128(a) * b; }
        public static Fact128 operator *(uint a, Fact128 b)    { return new Fact128(a) * b; }

        public static Fact128 operator /(double a, Fact128 b)  { return new Fact128(a) / b; }
        public static Fact128 operator /(float a, Fact128 b)   { return new Fact128(a) / b; }
        public static Fact128 operator /(decimal a, Fact128 b) { return new Fact128(a) / b; }
        public static Fact128 operator /(int a, Fact128 b)     { return new Fact128(a) / b; }
        public static Fact128 operator /(uint a, Fact128 b)    { return new Fact128(a) / b; }

        public static Fact128 operator %(double a, Fact128 b)  { return new Fact128(a) % b; }
        public static Fact128 operator %(float a, Fact128 b)   { return new Fact128(a) % b; }
        public static Fact128 operator %(decimal a, Fact128 b) { return new Fact128(a) % b; }
        public static Fact128 operator %(int a, Fact128 b)     { return new Fact128(a) % b; }
        public static Fact128 operator %(uint a, Fact128 b)    { return new Fact128(a) % b; }

        #endregion

        #endregion

        #region Comparison

        public static bool operator !=(Fact128 a, Fact128 b)
        {
            return a.Integer  != b.Integer
                || a.Fraction != b.Fraction;
        }
        public static bool operator ==(Fact128 a, Fact128 b)
        {
            return a.Integer  == b.Integer
                && a.Fraction == b.Fraction;
        }
        public static bool operator <(Fact128 a, Fact128 b)
        {
            return a.Integer  < b.Integer
                || a.Integer == b.Integer
                && a.Fraction < b.Fraction;
        }
        public static bool operator >(Fact128 a, Fact128 b)
        {
            return a.Integer  > b.Integer
                || a.Integer == b.Integer
                && a.Fraction > b.Fraction;
        }
        public static bool operator <=(Fact128 a, Fact128 b)
        {
            return a.Integer   < b.Integer
                || a.Integer  == b.Integer
                && a.Fraction <= b.Fraction;
        }
        public static bool operator >=(Fact128 a, Fact128 b)
        {
            return a.Integer   > b.Integer
                || a.Integer  == b.Integer
                && a.Fraction >= b.Fraction;
        }

        #region Conversions

        public static bool operator !=(Fact128 a, double b)  { return a != new Fact128(b); }
        public static bool operator !=(Fact128 a, float b)   { return a != new Fact128(b); }
        public static bool operator !=(Fact128 a, decimal b) { return a != new Fact128(b); }
        public static bool operator !=(Fact128 a, int b)     { return a != new Fact128(b); }
        public static bool operator !=(Fact128 a, uint b)    { return a != new Fact128(b); }

        public static bool operator ==(Fact128 a, double b)  { return a == new Fact128(b); }
        public static bool operator ==(Fact128 a, float b)   { return a == new Fact128(b); }
        public static bool operator ==(Fact128 a, decimal b) { return a == new Fact128(b); }
        public static bool operator ==(Fact128 a, int b)     { return a == new Fact128(b); }
        public static bool operator ==(Fact128 a, uint b)    { return a == new Fact128(b); }

        public static bool operator  <(Fact128 a, double b)  { return a < new Fact128(b); }
        public static bool operator  <(Fact128 a, float b)   { return a < new Fact128(b); }
        public static bool operator  <(Fact128 a, decimal b) { return a < new Fact128(b); }
        public static bool operator  <(Fact128 a, int b)     { return a < new Fact128(b); }
        public static bool operator  <(Fact128 a, uint b)    { return a < new Fact128(b); }

        public static bool operator  >(Fact128 a, double b)  { return a > new Fact128(b); }
        public static bool operator  >(Fact128 a, float b)   { return a > new Fact128(b); }
        public static bool operator  >(Fact128 a, decimal b) { return a > new Fact128(b); }
        public static bool operator  >(Fact128 a, int b)     { return a > new Fact128(b); }
        public static bool operator  >(Fact128 a, uint b)    { return a > new Fact128(b); }

        public static bool operator <=(Fact128 a, double b)  { return a <= new Fact128(b); }
        public static bool operator <=(Fact128 a, float b)   { return a <= new Fact128(b); }
        public static bool operator <=(Fact128 a, decimal b) { return a <= new Fact128(b); }
        public static bool operator <=(Fact128 a, int b)     { return a <= new Fact128(b); }
        public static bool operator <=(Fact128 a, uint b)    { return a <= new Fact128(b); }

        public static bool operator >=(Fact128 a, double b)  { return a >= new Fact128(b); }
        public static bool operator >=(Fact128 a, float b)   { return a >= new Fact128(b); }
        public static bool operator >=(Fact128 a, decimal b) { return a >= new Fact128(b); }
        public static bool operator >=(Fact128 a, int b)     { return a >= new Fact128(b); }
        public static bool operator >=(Fact128 a, uint b)    { return a >= new Fact128(b); }
        
        public static bool operator !=(double a, Fact128 b)  { return new Fact128(a) != b; }
        public static bool operator !=(float a, Fact128 b)   { return new Fact128(a) != b; }
        public static bool operator !=(decimal a, Fact128 b) { return new Fact128(a) != b; }
        public static bool operator !=(int a, Fact128 b)     { return new Fact128(a) != b; }
        public static bool operator !=(uint a, Fact128 b)    { return new Fact128(a) != b; }

        public static bool operator ==(double a, Fact128 b)  { return new Fact128(a) == b; }
        public static bool operator ==(float a, Fact128 b)   { return new Fact128(a) == b; }
        public static bool operator ==(decimal a, Fact128 b) { return new Fact128(a) == b; }
        public static bool operator ==(int a, Fact128 b)     { return new Fact128(a) == b; }
        public static bool operator ==(uint a, Fact128 b)    { return new Fact128(a) == b; }

        public static bool operator  <(double a, Fact128 b)  { return new Fact128(a) < b; }
        public static bool operator  <(float a, Fact128 b)   { return new Fact128(a) < b; }
        public static bool operator  <(decimal a, Fact128 b) { return new Fact128(a) < b; }
        public static bool operator  <(int a, Fact128 b)     { return new Fact128(a) < b; }
        public static bool operator  <(uint a, Fact128 b)    { return new Fact128(a) < b; }

        public static bool operator  >(double a, Fact128 b)  { return new Fact128(a) > b; }
        public static bool operator  >(float a, Fact128 b)   { return new Fact128(a) > b; }
        public static bool operator  >(decimal a, Fact128 b) { return new Fact128(a) > b; }
        public static bool operator  >(int a, Fact128 b)     { return new Fact128(a) > b; }
        public static bool operator  >(uint a, Fact128 b)    { return new Fact128(a) > b; }

        public static bool operator <=(double a, Fact128 b)  { return new Fact128(a) <= b; }
        public static bool operator <=(float a, Fact128 b)   { return new Fact128(a) <= b; }
        public static bool operator <=(decimal a, Fact128 b) { return new Fact128(a) <= b; }
        public static bool operator <=(int a, Fact128 b)     { return new Fact128(a) <= b; }
        public static bool operator <=(uint a, Fact128 b)    { return new Fact128(a) <= b; }

        public static bool operator >=(double a, Fact128 b)  { return new Fact128(a) >= b; }
        public static bool operator >=(float a, Fact128 b)   { return new Fact128(a) >= b; }
        public static bool operator >=(decimal a, Fact128 b) { return new Fact128(a) >= b; }
        public static bool operator >=(int a, Fact128 b)     { return new Fact128(a) >= b; }
        public static bool operator >=(uint a, Fact128 b)    { return new Fact128(a) >= b; }

        #endregion

        #endregion

        #endregion

        #region Misc

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get { return "0x" + ToString("X1"); }
        }

        public bool Equals(Fact128 other)
        {
            return Integer  == other.Integer
                && Fraction == other.Fraction;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Fact128 && Equals((Fact128)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Integer.GetHashCode() * 397) ^ Fraction.GetHashCode();
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Fact128 other)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format)
        {
            return base.ToString();
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return base.ToString();
        }

        #endregion
    }
}
