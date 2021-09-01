using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyCardio
{
    public class DataArrays
    {
        public const int DataArrSize = 0x100000;
        public const int DataArrSizeForView = 4000;

        public int[] ECG1Array;
        public int[] ECG2Array;
        public int[] ReoArray;
        public int[] Sphigmo1Array;
        public int[] Sphigmo2Array;
        public int[] ApexArray;

        public int[] ECG1ViewArray;
        public int[] ECG2ViewArray;
        public int[] ReoViewArray;
        public int[] Sphigmo1ViewArray;
        public int[] Sphigmo2ViewArray;
        public int[] ApexViewArray;

        public List<int[]> ViewList;

        public DataArrays(int size)
        {
            ECG1Array = new int[size];
            ECG2Array = new int[size];
            ReoArray = new int[size];
            Sphigmo1Array = new int[size];
            Sphigmo2Array = new int[size];
            ApexArray = new int[size];

            ECG1ViewArray = new int[size];
            ECG2ViewArray = new int[size];
            ReoViewArray = new int[size];
            Sphigmo1ViewArray = new int[size];
            Sphigmo2ViewArray = new int[size];
            ApexViewArray = new int[size];

            ViewList = new List<int[]>
            {
                ECG1ViewArray,
                ECG2ViewArray,
                ReoViewArray,
                Sphigmo1ViewArray,
                Sphigmo2ViewArray,
                ApexViewArray
            };
        }

        public String GetDataString(uint index)
        {
            //"Dd:mm:yyyy HH:mm:ss:fff"
            return String.Concat(DateTime.Now.ToString(), '\t',
                                 ECG1Array[index].ToString(), '\t',
                                 ECG2Array[index].ToString(), '\t',
                                 ReoArray[index].ToString(), '\t',
                                 Sphigmo1Array[index].ToString(), '\t',
                                 Sphigmo2Array[index].ToString(), '\t',
                                 ApexArray[index].ToString());
        }
    }
}
