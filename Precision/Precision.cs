using System;

namespace Precision
{
    public struct Precision
    {
        #region Constants



        #endregion

        public UInt64 Integer { get; }
        public UInt64 Fraction { get; }

        #region Constructors

        public Precision(UInt64 integer = 0, UInt64 fraction = 0)
        {
            Integer = integer;
            Fraction = fraction;
        }

        public Precision(float value)
        {
            Integer = 0;
            Fraction = 0;
        }

        public Precision(double value)
        {
            Integer = 0;
            Fraction = 0;
        }

        public Precision(decimal value)
        {
            Integer = 0;
            Fraction = 0;
        }

        public Precision(int value)
        {
            Integer = (uint)value;
            Fraction = 0;
        }

        public Precision(uint value)
        {
            Integer = value;
            Fraction = 0;
        }

        #endregion

        #region Operations
        
        #region Unary

        public static Precision operator +(Precision a)
        {
            return a;
        }
        public static Precision operator -(Precision a)
        {
            return a;
        }
        public static Precision operator !(Precision a)
        {
            return a;
        }
        public static Precision operator ~(Precision a)
        {
            return a;
        }
        public static Precision operator ++(Precision a)
        {
            return a;
        }
        public static Precision operator --(Precision a)
        {
            return a;
        }

        #endregion

        #region Binary

        public static Precision operator +(Precision a, Precision b)
        {
            return a;
        }
        public static Precision operator -(Precision a, Precision b)
        {
            return a;
        }
        public static Precision operator *(Precision a, Precision b)
        {
            return a;
        }
        public static Precision operator /(Precision a, Precision b)
        {
            return a;
        }
        public static Precision operator %(Precision a, Precision b)
        {
            return a;
        }

        #region Conversions

        public static Precision operator +(Precision a, double b)  { return a + new Precision(b); }
        public static Precision operator +(Precision a, float b)   { return a + new Precision(b); }
        public static Precision operator +(Precision a, decimal b) { return a + new Precision(b); }
        public static Precision operator +(Precision a, int b)     { return a + new Precision(b); }
        public static Precision operator +(Precision a, uint b)    { return a + new Precision(b); }
        
        public static Precision operator -(Precision a, double b)  { return a - new Precision(b); }
        public static Precision operator -(Precision a, float b)   { return a - new Precision(b); }
        public static Precision operator -(Precision a, decimal b) { return a - new Precision(b); }
        public static Precision operator -(Precision a, int b)     { return a - new Precision(b); }
        public static Precision operator -(Precision a, uint b)    { return a - new Precision(b); }
        
        public static Precision operator *(Precision a, double b)  { return a * new Precision(b); }
        public static Precision operator *(Precision a, float b)   { return a * new Precision(b); }
        public static Precision operator *(Precision a, decimal b) { return a * new Precision(b); }
        public static Precision operator *(Precision a, int b)     { return a * new Precision(b); }
        public static Precision operator *(Precision a, uint b)    { return a * new Precision(b); }
        
        public static Precision operator /(Precision a, double b)  { return a / new Precision(b); }
        public static Precision operator /(Precision a, float b)   { return a / new Precision(b); }
        public static Precision operator /(Precision a, decimal b) { return a / new Precision(b); }
        public static Precision operator /(Precision a, int b)     { return a / new Precision(b); }
        public static Precision operator /(Precision a, uint b)    { return a / new Precision(b); }
        
        public static Precision operator %(Precision a, double b)  { return a % new Precision(b); }
        public static Precision operator %(Precision a, float b)   { return a % new Precision(b); }
        public static Precision operator %(Precision a, decimal b) { return a % new Precision(b); }
        public static Precision operator %(Precision a, int b)     { return a % new Precision(b); }
        public static Precision operator %(Precision a, uint b)    { return a % new Precision(b); }
        
        public static Precision operator +(double a, Precision b)  { return new Precision(a) + b; }
        public static Precision operator +(float a, Precision b)   { return new Precision(a) + b; }
        public static Precision operator +(decimal a, Precision b) { return new Precision(a) + b; }
        public static Precision operator +(int a, Precision b)     { return new Precision(a) + b; }
        public static Precision operator +(uint a, Precision b)    { return new Precision(a) + b; }

        public static Precision operator -(double a, Precision b)  { return new Precision(a) - b; }
        public static Precision operator -(float a, Precision b)   { return new Precision(a) - b; }
        public static Precision operator -(decimal a, Precision b) { return new Precision(a) - b; }
        public static Precision operator -(int a, Precision b)     { return new Precision(a) - b; }
        public static Precision operator -(uint a, Precision b)    { return new Precision(a) - b; }

        public static Precision operator *(double a, Precision b)  { return new Precision(a) * b; }
        public static Precision operator *(float a, Precision b)   { return new Precision(a) * b; }
        public static Precision operator *(decimal a, Precision b) { return new Precision(a) * b; }
        public static Precision operator *(int a, Precision b)     { return new Precision(a) * b; }
        public static Precision operator *(uint a, Precision b)    { return new Precision(a) * b; }

        public static Precision operator /(double a, Precision b)  { return new Precision(a) / b; }
        public static Precision operator /(float a, Precision b)   { return new Precision(a) / b; }
        public static Precision operator /(decimal a, Precision b) { return new Precision(a) / b; }
        public static Precision operator /(int a, Precision b)     { return new Precision(a) / b; }
        public static Precision operator /(uint a, Precision b)    { return new Precision(a) / b; }

        public static Precision operator %(double a, Precision b)  { return new Precision(a) % b; }
        public static Precision operator %(float a, Precision b)   { return new Precision(a) % b; }
        public static Precision operator %(decimal a, Precision b) { return new Precision(a) % b; }
        public static Precision operator %(int a, Precision b)     { return new Precision(a) % b; }
        public static Precision operator %(uint a, Precision b)    { return new Precision(a) % b; }

        #endregion

        #endregion

        #region Comparison

        public static bool operator !=(Precision a, Precision b)
        {
            return false;
        }
        public static bool operator ==(Precision a, Precision b)
        {
            return false;
        }
        public static bool operator <(Precision a, Precision b)
        {
            return a.Integer < b.Integer;
        }
        public static bool operator >(Precision a, Precision b)
        {
            return false;
        }
        public static bool operator <=(Precision a, Precision b)
        {
            return false;
        }
        public static bool operator >=(Precision a, Precision b)
        {
            return false;
        }

        #region Conversions

        public static bool operator !=(Precision a, double b)  { return a != new Precision(b); }
        public static bool operator !=(Precision a, float b)   { return a != new Precision(b); }
        public static bool operator !=(Precision a, decimal b) { return a != new Precision(b); }
        public static bool operator !=(Precision a, int b)     { return a != new Precision(b); }
        public static bool operator !=(Precision a, uint b)    { return a != new Precision(b); }

        public static bool operator ==(Precision a, double b)  { return a == new Precision(b); }
        public static bool operator ==(Precision a, float b)   { return a == new Precision(b); }
        public static bool operator ==(Precision a, decimal b) { return a == new Precision(b); }
        public static bool operator ==(Precision a, int b)     { return a == new Precision(b); }
        public static bool operator ==(Precision a, uint b)    { return a == new Precision(b); }

        public static bool operator  <(Precision a, double b)  { return a < new Precision(b); }
        public static bool operator  <(Precision a, float b)   { return a < new Precision(b); }
        public static bool operator  <(Precision a, decimal b) { return a < new Precision(b); }
        public static bool operator  <(Precision a, int b)     { return a < new Precision(b); }
        public static bool operator  <(Precision a, uint b)    { return a < new Precision(b); }

        public static bool operator  >(Precision a, double b)  { return a > new Precision(b); }
        public static bool operator  >(Precision a, float b)   { return a > new Precision(b); }
        public static bool operator  >(Precision a, decimal b) { return a > new Precision(b); }
        public static bool operator  >(Precision a, int b)     { return a > new Precision(b); }
        public static bool operator  >(Precision a, uint b)    { return a > new Precision(b); }

        public static bool operator <=(Precision a, double b)  { return a <= new Precision(b); }
        public static bool operator <=(Precision a, float b)   { return a <= new Precision(b); }
        public static bool operator <=(Precision a, decimal b) { return a <= new Precision(b); }
        public static bool operator <=(Precision a, int b)     { return a <= new Precision(b); }
        public static bool operator <=(Precision a, uint b)    { return a <= new Precision(b); }

        public static bool operator >=(Precision a, double b)  { return a >= new Precision(b); }
        public static bool operator >=(Precision a, float b)   { return a >= new Precision(b); }
        public static bool operator >=(Precision a, decimal b) { return a >= new Precision(b); }
        public static bool operator >=(Precision a, int b)     { return a >= new Precision(b); }
        public static bool operator >=(Precision a, uint b)    { return a >= new Precision(b); }
        
        public static bool operator !=(double a, Precision b)  { return new Precision(a) != b; }
        public static bool operator !=(float a, Precision b)   { return new Precision(a) != b; }
        public static bool operator !=(decimal a, Precision b) { return new Precision(a) != b; }
        public static bool operator !=(int a, Precision b)     { return new Precision(a) != b; }
        public static bool operator !=(uint a, Precision b)    { return new Precision(a) != b; }

        public static bool operator ==(double a, Precision b)  { return new Precision(a) == b; }
        public static bool operator ==(float a, Precision b)   { return new Precision(a) == b; }
        public static bool operator ==(decimal a, Precision b) { return new Precision(a) == b; }
        public static bool operator ==(int a, Precision b)     { return new Precision(a) == b; }
        public static bool operator ==(uint a, Precision b)    { return new Precision(a) == b; }

        public static bool operator  <(double a, Precision b)  { return new Precision(a) < b; }
        public static bool operator  <(float a, Precision b)   { return new Precision(a) < b; }
        public static bool operator  <(decimal a, Precision b) { return new Precision(a) < b; }
        public static bool operator  <(int a, Precision b)     { return new Precision(a) < b; }
        public static bool operator  <(uint a, Precision b)    { return new Precision(a) < b; }

        public static bool operator  >(double a, Precision b)  { return new Precision(a) > b; }
        public static bool operator  >(float a, Precision b)   { return new Precision(a) > b; }
        public static bool operator  >(decimal a, Precision b) { return new Precision(a) > b; }
        public static bool operator  >(int a, Precision b)     { return new Precision(a) > b; }
        public static bool operator  >(uint a, Precision b)    { return new Precision(a) > b; }

        public static bool operator <=(double a, Precision b)  { return new Precision(a) <= b; }
        public static bool operator <=(float a, Precision b)   { return new Precision(a) <= b; }
        public static bool operator <=(decimal a, Precision b) { return new Precision(a) <= b; }
        public static bool operator <=(int a, Precision b)     { return new Precision(a) <= b; }
        public static bool operator <=(uint a, Precision b)    { return new Precision(a) <= b; }

        public static bool operator >=(double a, Precision b)  { return new Precision(a) >= b; }
        public static bool operator >=(float a, Precision b)   { return new Precision(a) >= b; }
        public static bool operator >=(decimal a, Precision b) { return new Precision(a) >= b; }
        public static bool operator >=(int a, Precision b)     { return new Precision(a) >= b; }
        public static bool operator >=(uint a, Precision b)    { return new Precision(a) >= b; }

        #endregion

        #endregion

        #endregion

        #region Misc

        public bool Equals(Precision other)
        {
            return Integer == other.Integer && Fraction == other.Fraction;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Precision && Equals((Precision)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Integer.GetHashCode() * 397) ^ Fraction.GetHashCode();
            }
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(IFormatProvider provider)
        {
            return base.ToString();
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
