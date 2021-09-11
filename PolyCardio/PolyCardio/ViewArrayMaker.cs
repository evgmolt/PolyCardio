using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PolyCardio
{
    static class ViewArrayMaker
    {
        public static Point[] MakeArray(Control view, int[] DataSource, uint index, int max)
        {
            return MakeArray(view, DataSource, index, max, 1, 1);
        }

        public static Point[] MakeArray(Control view, int[] DataSource, uint index, int max, double scaleY)
        {
            return MakeArray(view, DataSource, index, max, scaleY, 1);
        }

        public static Point[] MakeArray(Control view, int[] DataSource, uint index, int max, double scaleY, int scaleX)
        {
            var size = ByteDecomposer.DataArrSize;
            var h = view.Height;
            var w = view.Width;
            var arr = new Point[w/scaleX];
            for (int i = 0; i < w/scaleX; i++)
            {
                arr[i].X = i*scaleX;
                double res = h/2-(int)Math.Round(scaleY*(h*DataSource[(index - w/scaleX + i*scaleX) & (size - 1)])/(max));
                arr[i].Y = (int)Math.Round(res);
            }
            return arr;
        }

        public static Point[] MakeArrayForView(Control view, int[] dataSource, int shift, int max)
        {
            return MakeArrayForView(view, dataSource, shift, max, 1, 1);
        }

        public static Point[] MakeArrayForView(Control view, int[] dataSource, int shift, int max, double scaleY)
        {
            return MakeArrayForView(view, dataSource, shift, max, scaleY, 1);
        }

        public static Point[] MakeArrayForView(Control view, int[] dataSource, int shift, int max, double scaleY, int scaleX)
        {
            int H = view.Height;
            int W = view.Width;
            var arr = new Point[W / scaleX];
            for (int i = 0; i < W / scaleX; i++)
            {
                if (i > W-1) break;
                arr[i].X = i * scaleX;
                double res;
                if (shift + i > dataSource.Length - 1)
                {
                    res = H / 2;
                }
                else
                {
                    res = H / 2 - scaleY * (H * dataSource[shift + i]) / max;
                }
                arr[i].Y = (int)Math.Round(res);
            }
            return arr;
        }

        public static Point[] MakeArrayForView(Control view, double[] dataSource, int shift, double max, double scaleY, double scaleX)
        {
            double H = view.Height;
            int W = view.Width;
            var arr = new Point[W / (int)scaleX];
            for (int i = 0; i < W / scaleX; i++)
            {
                if (i > W - 1) break;
                arr[i].X = i * (int)scaleX;
                double res;
                if (i > dataSource.Length - 1) res = H / 2;
                else res = H / 2 - scaleY * (H * dataSource[shift + i]) / max;
                arr[i].Y = (int)Math.Round(res);
            }
            return arr;
        }
    }
}
