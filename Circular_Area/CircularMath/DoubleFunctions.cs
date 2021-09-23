using System;
using System.Runtime.CompilerServices;

namespace Circular_Area.CircularMath
{
    public static partial class CarlsonSymmetricForm
    {
        public static double RF(double X, double Y, double Z)
        {
            const double MinError = 0.0000001;

            double a, dx, dy, dz;

            do
            {
                var lambda = Math.Sqrt(X * Y)
                    + Math.Sqrt(Y * Z)
                    + Math.Sqrt(Z * X);

                X = (X + lambda) / 4;
                Y = (Y + lambda) / 4;
                Z = (Z + lambda) / 4;

                a = (X + Y + Z) / 3;

                dx = 1 - X / a;
                dy = 1 - Y / a;
                dz = 1 - Z / a;
            }
            while (Math.Max(Math.Max(Math.Abs(dx), Math.Abs(dy)), Math.Abs(dz)) > MinError);

            double e2, e3;
            e2 = dx * dy + dy * dz + dz * dx;
            e3 = dy * dx * dz;

            //http://dlmf.nist.gov/19.36#E1
            double e2_2 = Math.Pow(e2, 2);
            double result = 1
                - (1 / 10 * e2)
                + (1 / 14 * e3)
                + (1 / 24 * e2_2)
                - (3 / 44 * e2 * e3)
                - (5 / 208 * Math.Pow(e2, 3))
                + (3 / 104 * Math.Pow(e3, 2))
                + (1 / 16 * (e2_2 * e3));

            result *= 1 / Math.Sqrt(a);
            return result;
        }
    }

    public static partial class SpecialFunctions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double F(double angle, double k)
        {
            var radians = Math.PI * angle / 180;
            return Math.Sin(radians) * CarlsonSymmetricForm.RF(Math.Pow(Math.Cos(radians), 2), 1 - k * Math.Pow(Math.Sin(radians), 2), 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cn(double input, double modulus)
        {
            return F(Math.Acos(input), modulus);
        }
    }
}