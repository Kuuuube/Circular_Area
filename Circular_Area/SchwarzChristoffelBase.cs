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

            phi /= 1 << i;
            return phi / g;
        }

        private static (double, double, double) agm_jacobi_sn_cn_dn(double u) {
            int MAX_ITER = 64;
            double[] a = new double[MAX_ITER + 1];
            double[] g = new double[MAX_ITER + 1];
            double[] c = new double[MAX_ITER + 1];
            a[0] = 1.0;
            g[0] = Math.Sqrt(1.0 / 2.0);
            c[0] = Math.Sqrt(1.0 / 2.0);
            int i = 0;
            while (true) {
                a[i + 1] = 0.5 * (a[i] + g[i]);
                g[i + 1] = Math.Sqrt(a[i] * g[i]);
                c[i + 1] = 0.5 * (a[i] - g[i]);
                i += 1;
                if (!(i < MAX_ITER && Math.Abs(a[i] - g[i]) > 0.00001)) {
                    break;
                }
            }

            double phi = (1 << i) * a[i] * u;
            while (i > 0){
                phi = 0.5 * (phi + Math.Asin(c[i] * Math.Sin(phi) / a[i]));
                i -= 1;
            }

            double sn = Math.Sin(phi);
            double cn = Math.Cos(phi);
            double dn = Math.Sqrt(1.0 - 0.5 * (sn * sn));
            return (sn, cn, dn);
        }

        public static (double, double) ccn(double re, double im) {
            var (sn_re, cn_re, dn_re) = agm_jacobi_sn_cn_dn(re);
            var (sn_im, cn_im, dn_im) = agm_jacobi_sn_cn_dn(im);
            double t = 1.0 - dn_re * dn_re * sn_im * sn_im;
            double ret_re = cn_re * cn_im / t;
            double ret_im = -sn_re * dn_re * sn_im * dn_im / t;
            return (ret_re, ret_im);
        }
    }
}
