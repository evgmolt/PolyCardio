using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyCardio
{
    public static class DataProcessing
    {
        public static int CorrPattLen = 14;
        public static double GetCorr(int[] inArr, int ind)
        {
            int[] CorrPattern = {0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
            double patmean = 0;
            for (int i = 0; i < CorrPattern.Length; i++)
            {
                patmean = patmean + inArr[ind + i];
            }
            double smean = 0;
            for (int i = 0; i < CorrPattern.Length; i++)
            {
                smean = smean + inArr[ind + i];
            }
            smean = smean / CorrPattern.Length;
            double corr = 0;
            for (int i = 0; i < CorrPattern.Length; i++)
            {
                corr = corr + (inArr[ind + i] - smean) * (CorrPattern[i] - patmean);
            }
            return corr;
        }

        public static void CountAver(int[] aver, int[] data, int[] rrarray, int numofqrs, int shift)
        {
            if (numofqrs < 1) return;
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = 0;
            }
            int AverArrayCenter = aver.Length / 2;
            int AverSize = rrarray[1] - rrarray[0];
            AverSize = AverSize * 2;
            for (int i = 0; i < numofqrs; i++)
            {
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter - j] += data[rrarray[i + shift] - j];
                }
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter + 1 + j] += data[rrarray[i + shift] + 1 + j];
                }
            }
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = aver[i] / numofqrs;
            }
        }

        public static void CountAver(double[] aver, double[] data, int[] rrarray, int numofqrs, int shift)
        {
            if (numofqrs < 1) return;
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = 0;
            }
            int AverArrayCenter = aver.Length / 2;
            int AverSize = rrarray[1] - rrarray[0];
            AverSize = AverSize * 2;
            for (int i = 0; i < numofqrs; i++)
            {
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter - j] += data[rrarray[i + shift] - j];
                }
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter + 1 + j] += data[rrarray[i + shift] + 1 + j];
                }
            }
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = aver[i] / numofqrs;
            }
        }

        public static double[] Norm(double[] data)
        {
            if (data == null) return null;
            double[] res = new double[data.Length];
            double max = -100000;
            double min = 100000;
            for (int i = 0; i < data.Length; i++)
            {
                max = Math.Max(data[i], max);
                min = Math.Max(data[i], min);
            }
            for (int i = 0; i < data.Length; i++)
            {
                res[i] = (data[i] - min) / (max - min);
            }
            return res;
        }

        public static void CountAver(double[] aver, int[] data, int[] rrarray, int numofqrs, int shift)
        {
            if (numofqrs < 1) return;
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = 0;
            }
            int AverArrayCenter = aver.Length / 2;
            int AverSize = rrarray[1] - rrarray[0];
            AverSize = AverSize * 2;
            for (int i = 0; i < numofqrs; i++)
            {
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter - j] += data[rrarray[i + shift] - j];
                }
                for (int j = 0; j < AverSize / 2; j++)
                {
                    aver[AverArrayCenter + 1 + j] += data[rrarray[i + shift] + 1 + j];
                }
            }
            for (int i = 0; i < aver.Length; i++)
            {
                aver[i] = aver[i] / numofqrs;
            }
        }

        public static int GetRange(int[] Data, int size)
        {
            int min = 10000000;
            int max = 0;
            foreach (int elem in Data)
            {
                min = Math.Min(min, elem);
                max = Math.Max(max, elem);
            }
            return (max - min);
        }

        public static void CountDerivative(double[] inarr, double[] outarr, int size)
        {
            for (int i = 2; i < size - 2; i++)
            {
                outarr[i+2] = -2 * inarr[i - 2] - inarr[i - 1] + inarr[i + 1] + 2 * inarr[i + 2];
            }

        }

        public static void CutArray(int[] data, int cutsize)
        {
            if (data == null) return;
            for (int i = 0; i < data.Length / 2; i++)
            {
                if (data[i] != 0)
                {
                    for (int j = 0; j < cutsize; j++)
                    {
                        data[i + j] = 0;
                    }
                    break;
                }
            }
            for (int i = data.Length - 1; i > data.Length / 2; i--)
            {
                if (data[i] != 0)
                {
                    for (int j = 0; j < cutsize; j++)
                    {
                        data[i - j] = 0;
                    }
                    break;
                }
            }
        }

        public static void CutArray(double[] data, int cutsize)
        {
            if (data == null) return;
            for (int i = 0; i < data.Length / 2; i++)
            {
                if (data[i] != 0)
                {
                    for (int j = 0; j < cutsize; j++)
                    {
                        data[i + j] = 0;
                    }
                    break;
                }
            }
            for (int i = data.Length - 1; i > data.Length / 2; i--)
            {
                if (data[i] != 0)
                {
                    for (int j = 0; j < cutsize; j++)
                    {
                        data[i - j] = 0;
                    }
                    break;
                }
            }
        }
    }
}
