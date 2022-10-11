using System;
using System.Numerics;

namespace Circular_Area
{
    public class QuarticBase
    {
        public static Complex[] Quadratic(Complex a0, Complex b0, Complex c0)
        {
            Complex a = b0 / a0;
            Complex b = c0 / a0;

            a0 = -0.5 * a;
            Complex delta = a0.Real * a0.Real - b.Real;
            Complex sqrt_delta = Complex.Sqrt(delta);

            Complex r1 = a0 - sqrt_delta;
            Complex r2 = a0 + sqrt_delta;

            return new Complex[] { r1, r2 };
        }

        public static Complex[] Cubic(Complex a0, Complex b0, Complex c0, Complex d0)
        {
            Complex f = ((3 * c0 / a0) - (Complex.Pow(b0, 2) / Complex.Pow(a0, 2))) / 3;
            Complex g = (2 * Complex.Pow(b0, 3) / Complex.Pow(a0, 3) - (9 * b0 * c0 / Complex.Pow(a0, 2)) + (27 * d0 / a0)) / 27;
            Complex h = (Complex.Pow(g, 2) / 4) + (Complex.Pow(f, 3) / 27);

            if (h.Real > 0)
            {
                Complex R = -(g / 2) + Complex.Pow(h, 0.5);
                Complex S = Complex.Pow(R, 0.333333333);
                Complex T = -(g / 2) - Complex.Pow(h, 0.5);
                Complex U = new Complex(Math.Cbrt(T.Real), Math.Cbrt(T.Imaginary));
                Complex x1 = (S + U) - (b0 / (3 * a0));
                Complex x2 = -(S + U) / 2 - (b0 / (3 * a0)) + Complex.ImaginaryOne * (S - U) * Complex.Sqrt(3) / 2;
                Complex x3 = -(S + U) / 2 - (b0 / (3 * a0)) - Complex.ImaginaryOne * (S - U) * Complex.Sqrt(3) / 2;
                return new Complex[] { x1, x2, x3 };
            }

            if (f == 0 && g == 0 && h == 0)
            {
                Complex x1 = new Complex(Math.Cbrt(d0.Real / a0.Real) * -1, Math.Cbrt(d0.Imaginary / a0.Imaginary) * -1);
                return new Complex[] { x1, x1, x1 };
            }

            if (h.Real <= 0)
            {
                Complex i = Complex.Pow(((Complex.Pow(g, 2) / 4) - h), 0.5);
                Complex j = Complex.Pow(i, 0.33333333);
                Complex k = Complex.Acos(-(g / (2 * i)));
                Complex L = j * -1;
                Complex M = Complex.Cos(k / 3);
                Complex N = Complex.Sqrt(3) * Complex.Sin(k / 3);
                Complex P = (b0 / 3 * a0) * -1;
                Complex x1 = 2 * j * Complex.Cos(k / 3) - (b0 / (3 * a0));
                Complex x2 = L * (M + N) + P;
                Complex x3 = L * (M - N) + P;

                return new Complex[] { x1, x2, x3 };
            }

            return new Complex[] { 0, 0, 0 };
        }

        public static Complex[] Quartic(Complex a0, Complex b0, Complex c0, Complex d0, Complex e0)
        {
            b0 = b0 / a0;
            c0 = c0 / a0;
            d0 = d0 / a0;
            e0 = e0 / a0;
            a0 = a0 / a0;

            Complex f = c0 - ((3 * Complex.Pow(b0, 2)) / 8);
            Complex g = d0 + ((Complex.Pow(b0, 3) / 8) - ((b0 * c0) / 2));
            Complex h = e0 - (3 * Complex.Pow(b0, 4) / 256) + (Complex.Pow(b0, 2) * c0 / 16) - (b0 * d0 / 4);

            Complex Y3 = 1;
            Complex Y2 = (f / 2);
            Complex Y1 = ((Complex.Pow(f, 2) - 4 * h) / 16);
            Complex Y0 = -Complex.Pow(g, 2) / 64;

            Complex[] Cubic_results = Cubic(Y3, Y2, Y1, Y0);

            Complex p = 0;
            Complex q = 0;

            Complex last_result = 0;

            foreach (Complex result in Cubic_results)
            {
                if (Math.Round(result.Real, 5) != 0 && Math.Round(result.Imaginary, 5) != 0 && p == 0 && q == 0)
                {
                    p = Complex.Sqrt(result);
                    last_result = result;
                }

                if (Math.Round(result.Real, 5) != 0 && Math.Round(result.Imaginary, 5) != 0 && q == 0 && result != last_result)
                {
                    q = Complex.Sqrt(result);
                }
            }

            if ((p == 0) || (q == 0))
            {
                foreach (Complex result in Cubic_results)
                {
                    if (Math.Round(result.Real, 5) != 0 && p == 0 && q == 0 && result != last_result)
                    {
                        p = Complex.Sqrt(result);
                        last_result = result;
                    }

                    if (Math.Round(result.Real, 5) != 0 && q == 0 && result != last_result)
                    {
                        q = Complex.Sqrt(result);
                    }
                }
            }

            Complex r = -g / (8 * p * q);
            Complex s = b0 / (4 * a0);

            Complex x1 = p + q + r - s;
            Complex x2 = p - q - r - s;
            Complex x3 = -p + q - r - s;
            Complex x4 = -p - q + r - s;

            return new Complex[] { x1, x2, x3, x4 };
        }
    }
}
