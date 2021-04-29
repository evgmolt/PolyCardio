using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCardio
{

    public static class Filter
    {

        public static readonly double[] coeff50 =  {+5.8055825294890924e-003, 
                                   -5.6110169116486645e-003, 
                                   -1.3295678981616035e-002, 
                                   +1.9963872393967758e-002, 
                                   -5.5108638151477171e-003, 
                                   -3.6636514465905511e-002, 
                                   +5.7692568525026300e-002, 
                                   -6.1187785202242596e-003, 
                                   -1.2758195775567843e-001, 
                                   +2.7500984976154336e-001, 
                                   +6.6022919178123562e-001, 
                                   +2.7500984976154336e-001, 
                                   -1.2758195775567843e-001, 
                                   -6.1187785202242596e-003, 
                                   +5.7692568525026300e-002, 
                                   -3.6636514465905511e-002, 
                                   -5.5108638151477171e-003, 
                                   +1.9963872393967758e-002, 
                                   -1.3295678981616035e-002, 
                                   -5.6110169116486645e-003, 
                                   +5.8055825294890924e-003 };

        //LPF_Fs125Hz_Fpass8Hz_Fstop14Hz
        public static readonly double[] coeff14 = {+1.4955781797938609e-002, 
                                    +2.4870721049448024e-002, 
                                    +4.1110724254332091e-002, 
                                    +5.9668660137318891e-002, 
                                    +7.8249451320493643e-002, 
                                    +9.4172718549901807e-002, 
                                    +1.0493367973445666e-001, 
                                    +1.0874215210149615e-001, 
                                    +1.0493367973445666e-001, 
                                    +9.4172718549901807e-002, 
                                    +7.8249451320493643e-002, 
                                    +5.9668660137318891e-002, 
                                    +4.1110724254332091e-002, 
                                    +2.4870721049448024e-002, 
                                    +1.4955781797938609e-002 };


        public static int filter(double[] coeff, int[] inArr, uint ind)
        {
            double sum = 0;
            for (int i = 0; i < coeff.Length; i++)
            {
                double a;
                a = inArr[(ind - coeff.Length + i + 1) & (ByteDecomposer.DataArrSize - 1)];
                a = a * coeff[i];
                sum = sum + a;
            }
            return (int)Math.Round(sum);
        }

        public static int filterForView(double[] coeff, int[] inArr, int ind)
        {
            if (ind < coeff.Length) return 0;
            double sum = 0;
            for (int i = 0; i < coeff.Length; i++)
            {
                double a;
                a = inArr[ind - coeff.Length + i + 1];
                a = a * coeff[i];
                sum = sum + a;
            }
            return (int)Math.Round(sum);
        }
    }
}
