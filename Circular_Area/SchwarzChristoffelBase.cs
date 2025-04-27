using System;
using System.Numerics;

namespace Circular_Area
{
    public class SchwarzChristoffelBase
    {
        public static double landen_elliptic_f(double input_phi) {
            double phi = input_phi;
            double a = 1.0;
            double g = Math.Sqrt(1.0 / 2.0);
            double last_a;
            double last_g;
            double tan_2n_phi;

            int MAX_ITER = 63;
            int i = 0;
            while (true) {
                tan_2n_phi = Math.Tan(phi);
                i += 1;
                phi += phi - Math.Atan((a - g) * tan_2n_phi / (a + g * tan_2n_phi * tan_2n_phi));
                last_a = a;
                last_g = g;
                a = 0.5 * (last_a + last_g);
                g = Math.Sqrt(last_a * last_g);
                if (!(i < MAX_ITER && Math.Abs(a - g) > 0.00001)) {
                    break;
                }
            }

            int longlong1 = 1;
            phi /= longlong1 << i;
            return phi / g;
        }
    }
}
