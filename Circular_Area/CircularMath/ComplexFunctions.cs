using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Circular_Area.CircularMath
{
    public static partial class CarlsonSymmetricForm
    {
        const double MinError = 0.0000001;

        public static Complex RF(Complex X, Complex Y, Complex Z)
        {
            Complex a, dx, dy, dz;

            do
            {
                var lambda = Complex.Sqrt(X * Y)
                    + Complex.Sqrt(Y * Z)
                    + Complex.Sqrt(Z * X);

                X = (X + lambda) / 4;
                Y = (Y + lambda) / 4;
                Z = (Z + lambda) / 4;

                a = (X + Y + Z) / 3;

                dx = 1 - X / a;
                dy = 1 - Y / a;
                dz = 1 - Z / a;
            }
            while (Math.Max(Math.Max(Complex.Abs(dx), Complex.Abs(dy)), Complex.Abs(dz)) > MinError);

            Complex e2, e3;
            e2 = dx * dy + dy * dz + dz * dx;
            e3 = dy * dx * dz;

            //http://dlmf.nist.gov/19.36#E1
            Complex e2_2 = Complex.Pow(e2, 2);
            Complex result = 1
                - (1 / 10 * e2)
                + (1 / 14 * e3)
                + (1 / 24 * e2_2)
                - (3 / 44 * e2 * e3)
                - (5 / 208 * Complex.Pow(e2, 3))
                + (3 / 104 * Complex.Pow(e3, 2))
                + (1 / 16 * (e2_2 * e3));

            result *= 1 / Complex.Sqrt(a);
            return result;
        }
    }

    public static partial class SpecialFunctions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex F(Complex angle, double k)
        {
            var radians = Math.PI * angle / 180;
            return Complex.Sin(radians) * CarlsonSymmetricForm.RF(Complex.Pow(Complex.Cos(radians), 2), 1 - k * Complex.Pow(Complex.Sin(radians), 2), 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Cn(Complex input, double modulus)
        {
            return F(Complex.Acos(input), modulus);
        }
    }
}