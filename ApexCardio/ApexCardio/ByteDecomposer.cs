using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ApexCardio
{
    public class DataArrays
    {
        public int[] ECG1Array;
        public int[] ECG2Array;
        public int[] Reo1Array;
        public int[] Reo2Array;
        public int[] Sphigmo1Array;
        public int[] Sphigmo2Array;
        public int[] ApexArray;

        public double[] AverECG1;
        public double[] AverECG2;
        public double[] AverReo1;
        public double[] AverReo2;
        public double[] AverSphigmo1;
        public double[] AverSphigmo2;
        public double[] AverApex;

        public List<double[]> AverList;

        public double[] FirstDerivReo1;
        public double[] FirstDerivReo2;
        public double[] FirstDerivSphigmo1;
        public double[] FirstDerivSphigmo2;
        public double[] FirstDerivApex;

        public List<double[]> FirstDerivList;

        public double[] SecDerivReo1;
        public double[] SecDerivReo2;
        public double[] SecDerivSphigmo1;
        public double[] SecDerivSphigmo2;
        public double[] SecDerivApex;

        public List<double[]> SecDerivList;

        private int AverArrSize = 256;
        
        public int[] ECG1ViewArray;
        public int[] ECG2ViewArray;
        public int[] Reo1ViewArray;
        public int[] Reo2ViewArray;
        public int[] Sphigmo1ViewArray;
        public int[] Sphigmo2ViewArray;
        public int[] ApexViewArray;

        public List<int[]> ViewList;
        
        private const int RRMaxNum = 1000;
        public int[] RArray;
        public int RInd;

    public DataArrays(int size)
        {
            ECG1Array = new int[size];
            ECG2Array = new int[size];
            Reo1Array = new int[size];
            Reo2Array = new int[size];
            Sphigmo1Array = new int[size];
            Sphigmo2Array = new int[size];
            ApexArray = new int[size];

            AverECG1 = new double[AverArrSize];
            AverECG2 = new double[AverArrSize];
            AverReo1 = new double[AverArrSize];
            AverReo2 = new double[AverArrSize];
            AverSphigmo1 = new double[AverArrSize];
            AverSphigmo2 = new double[AverArrSize];
            AverApex = new double[AverArrSize];

            AverList = new List<double[]>();
            AverList.Add(AverECG1);
            AverList.Add(AverECG2);
            AverList.Add(AverReo1);
            AverList.Add(AverReo2);
            AverList.Add(AverSphigmo1);
            AverList.Add(AverSphigmo2);
            AverList.Add(AverApex);

            FirstDerivReo1 = new double[AverArrSize];
            FirstDerivReo2 = new double[AverArrSize];
            FirstDerivSphigmo1 = new double[AverArrSize];
            FirstDerivSphigmo2 = new double[AverArrSize];
            FirstDerivApex = new double[AverArrSize];

            FirstDerivList = new List<double[]>();
            FirstDerivList.Add(null);
            FirstDerivList.Add(null);
            FirstDerivList.Add(FirstDerivReo1);
            FirstDerivList.Add(FirstDerivReo2);
            FirstDerivList.Add(FirstDerivSphigmo1);
            FirstDerivList.Add(FirstDerivSphigmo2);
            FirstDerivList.Add(FirstDerivApex);

            SecDerivReo1 = new double[AverArrSize];
            SecDerivReo2 = new double[AverArrSize];
            SecDerivSphigmo1 = new double[AverArrSize];
            SecDerivSphigmo2 = new double[AverArrSize];
            SecDerivApex = new double[AverArrSize];

            SecDerivList = new List<double[]>();
            SecDerivList.Add(null);
            SecDerivList.Add(null);
            SecDerivList.Add(SecDerivReo1);
            SecDerivList.Add(SecDerivReo2);
            SecDerivList.Add(SecDerivSphigmo1);
            SecDerivList.Add(SecDerivSphigmo2);
            SecDerivList.Add(SecDerivApex);
            
            ECG1ViewArray = new int[size];
            ECG2ViewArray = new int[size];
            Reo1ViewArray = new int[size];
            Reo2ViewArray = new int[size];
            Sphigmo1ViewArray = new int[size];
            Sphigmo2ViewArray = new int[size];
            ApexViewArray = new int[size];

            ViewList = new List<int[]>();
            ViewList.Add(ECG1ViewArray);
            ViewList.Add(ECG2ViewArray);
            ViewList.Add(Reo1ViewArray);
            ViewList.Add(Reo2ViewArray);
            ViewList.Add(Sphigmo1ViewArray);
            ViewList.Add(Sphigmo2ViewArray);
            ViewList.Add(ApexViewArray);

            RArray = new int[RRMaxNum];
            RInd = 0;
        }
    }

    public class BPEventArgs : EventArgs
    {
        public byte status { get; set; }
    }

    class ByteDecomposer
    {
        public bool Calibrate;
        public event EventHandler DecomposeLineEvent;
        public event EventHandler ConnectionBreakdown;
        public event EventHandler<BPEventArgs> BPcompleted;

        public const int SamplingFrequency = 125;

        public DataArrays Data;
        private const byte Marker1 = 0x19;
        public const int DataArrSize = 0x100000;
        public const int DataArrSizeForView = 4000;

        public const byte bp1completed = 1;
        public const byte bp2completed = 2;
        public const byte bp1error = 4;
        public const byte bp2error = 8;
        public const byte bp1progress = 16;
        public const byte bp2progress = 32;
        public const byte pump1progress = 64;
        public const byte pump2progress = 128;

        private int ECG1tmp;
        private int ECG2tmp;
        private int Reo1tmp;
        private int Reo2tmp;
        private int Sphigmo1tmp;
        private int Sphigmo2tmp;
        private int Apextmp;

        private int nextECG1;
        private int nextECG2;
        private int nextReo1;
        private int nextReo2;
        private int nextSphigmo1;
        private int nextSphigmo2;
        private int nextApex;

        private double detrendECG1 = 0;
        private double detrendECG2 = 0;
        private double detrendReo1 = 0;
        private double detrendReo2 = 0;
        private double detrendSphigmo1 = 0;
        private double detrendSphigmo2 = 0;
        private double detrendApex = 0;

        private int prevECG1;
        private int prevECG2;
        private int prevReo1;
        private int prevReo2;
        private int prevSphigmo1;
        private int prevSphigmo2;
        private int prevApex;

        public int PrSYS1;
        public int PrDIA1;
        public int PrSYS2;
        public int PrDIA2;
        public byte Status;

        public uint MainIndex = 0;
        public int LineCounter = 0;
        private const byte AccelZero = 128-105;
        private const int MaxDTOCounter = 10;

        public const int BytesInBlock=27;
        private int byteNum;
        public bool RecordStarted;
        public bool DeviceTurnedOn;
        private int DTOCounter;
        public bool BP1measStarted;
        public bool BP2measStarted;
        public bool Pump1Started;
        public bool Pump2Started;
        private bool Pump1RealyStarted;
        private bool Pump2RealyStarted;

        private int DelayCounter1;
        private int DelayCounter2;
        private const int MaxDelayCounter = 300;
        public int TotalBytes;
        const double filterCoeff = 0.005;
        const double filterCoeffReo = 0.01;


        public ByteDecomposer(DataArrays data)
        {
            Calibrate = false;
            Data = data;
            BP1measStarted = false;
            BP2measStarted = false;
            Pump1Started = false;
            Pump2Started = false;
            Pump1RealyStarted = false;
            Pump2RealyStarted = false;
            RecordStarted = false;
            DeviceTurnedOn = true;
            DTOCounter = 0;
            TotalBytes = 0;
            byteNum = 0;
            MainIndex = 0;
        }

        protected virtual void OnDecomposeLineEvent()
        {
            if (DecomposeLineEvent != null)
                DecomposeLineEvent(this, null);
        }

        protected virtual void OnConnectionBreakdown()
        {
            if (ConnectionBreakdown != null)
                ConnectionBreakdown(this, null);
        }

        protected virtual void OnBPcompleted(BPEventArgs arg)
        {
            if (BPcompleted != null)
                BPcompleted(this, arg);
        }

        public int Decompos(USBserialPort usbport)
        {
            return Decompos(usbport, null, null, null);
        }

        private double FilterRecursDetrend(double filterK, double next, double detrend, double prev)
        {
            return ((1 - filterK) * next - (1 - filterK) * prev + (1 - 2 * filterK) * detrend);
        }

        private void ProcessData(int[] inData, int[] outData, int size, bool fOn, double[] coeff)
        {
            int Aver = 0;
            var tmpBuf = new int[size];
            for (int i = 0; i < size; i++)
            {
                Aver = Aver + inData[i];
            }
            Aver = Aver / size;
            for (int i = 0; i < size; i++)
            {
                tmpBuf[i] = inData[i] - Aver;
            }
            for (int i = 0; i < size; i++)
            {
                if (fOn)
                    outData[i] = Filter.filterForView(coeff, tmpBuf, i);
                else outData[i] = tmpBuf[i];
            }
        }


        public void CountViewArrays(int size, bool f)
        {
            ProcessData(Data.ECG1Array, Data.ECG1ViewArray , size, f, Filter.coeff50);
            ProcessData(Data.ECG2Array, Data.ECG2ViewArray, size, f, Filter.coeff50);
            ProcessData(Data.Reo1Array, Data.Reo1ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.Reo2Array, Data.Reo2ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.Sphigmo1Array, Data.Sphigmo1ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.Sphigmo2Array, Data.Sphigmo2ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.ApexArray, Data.ApexViewArray, size, f, Filter.coeff14);
        }

        public int Decompos(USBserialPort usbport, Stream saveFileStream)
        {
            return Decompos(usbport, saveFileStream, null, null);
        }

        public int Decompos(USBserialPort usbport, Stream saveFileStream, StreamWriter txtFileStream, ApexConfig cfg)
        {
                int bytes = usbport.BytesRead;
                if (bytes==0)
                {
                    DTOCounter++;
                    if (DTOCounter > MaxDTOCounter)
                    {
                        DeviceTurnedOn = false;
                    }
                    return 0;
                }
                DeviceTurnedOn = true;
                if (saveFileStream != null & RecordStarted)
                {
                    try
                    {
                        saveFileStream.Write(usbport.PortBuf, 0, bytes);
                        TotalBytes = TotalBytes + bytes;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                for (int i = 0; i < bytes; i++)
                {
                    switch (byteNum)
                    {
                        case 0:// Marker
                            if (usbport.PortBuf[i] == Marker1)
                                byteNum = 1;
                            break;
                        case 1:// ECG1_0
                            ECG1tmp = (int)usbport.PortBuf[i];
                            byteNum = 2;
                            break;
                        case 2:// ECG1_1
                            ECG1tmp = ECG1tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 3;
                            break;
                        case 3:// ECG1_2
                            ECG1tmp = ECG1tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((ECG1tmp & 0x800000) != 0)
                                ECG1tmp = ECG1tmp - 0x1000000;

                            Data.ECG1Array[MainIndex] = ECG1tmp;

                            nextECG1 = ECG1tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextECG1 = Filter.filter(Filter.coeff50, Data.ECG1Array, MainIndex);
                            }
                            detrendECG1 = FilterRecursDetrend(filterCoeff, nextECG1, detrendECG1, prevECG1);
                            Data.ECG1ViewArray[MainIndex] = (int)Math.Round(detrendECG1);
                            
                            prevECG1 = nextECG1;
                            byteNum = 4;
                            break;

                        case 4:// ECG2_0
                            ECG2tmp = (int)usbport.PortBuf[i];
                            byteNum = 5;
                            break;
                        case 5:// ECG2_1
                            ECG2tmp = ECG2tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 6;
                            break;
                        case 6:// ECG2_2
                            ECG2tmp = ECG2tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((ECG2tmp & 0x800000) != 0)
                                ECG2tmp = ECG2tmp - 0x1000000;

                            Data.ECG2Array[MainIndex] = ECG2tmp;

                            nextECG2 = ECG2tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextECG2 = Filter.filter(Filter.coeff50, Data.ECG2Array, MainIndex);
                            }

                            detrendECG2 = FilterRecursDetrend(filterCoeff, nextECG2, detrendECG2, prevECG2);
                            Data.ECG2ViewArray[MainIndex] = (int)Math.Round(detrendECG2);

                            prevECG2 = nextECG2;
                            byteNum = 7;
                            break;

                        case 7:// Reo1_0
                            Reo1tmp = (int)usbport.PortBuf[i];
                            byteNum = 8;
                            break;
                        case 8:// Reo1_1
                            Reo1tmp = Reo1tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 9;
                            break;
                        case 9:// Reo1_2
                            Reo1tmp = Reo1tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Reo1tmp & 0x800000) != 0)
                                Reo1tmp = Reo1tmp - 0x1000000;
                            if (Reo1tmp > ApexConstants.ReoMaxVal) Reo1tmp = ApexConstants.ReoMaxVal;
                            Data.Reo1Array[MainIndex] = - Reo1tmp;

                            nextReo1 = Reo1tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextReo1 = Filter.filter(Filter.coeff14, Data.Reo1Array, MainIndex);
                            }

                            detrendReo1 = FilterRecursDetrend(filterCoeffReo, nextReo1, detrendReo1, prevReo1);
                            Data.Reo1ViewArray[MainIndex] = (int)Math.Round(detrendReo1);

                            prevReo1 = nextReo1;
                            byteNum = 10;
                            break;

                        case 10:// Reo2_0
                            Reo2tmp = (int)usbport.PortBuf[i];
                            byteNum = 11;
                            break;
                        case 11:// Reo2_1
                            Reo2tmp = Reo2tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 12;
                            break;
                        case 12:// Reo2_2
                            Reo2tmp = Reo2tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Reo2tmp & 0x800000) != 0)
                                Reo2tmp = Reo2tmp - 0x1000000;

                            if (Reo2tmp > ApexConstants.ReoMaxVal) Reo2tmp = ApexConstants.ReoMaxVal;
                            Data.Reo2Array[MainIndex] = Reo2tmp;

                            nextReo2 = Reo2tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextReo2 = Filter.filter(Filter.coeff14, Data.Reo2Array, MainIndex);
                            }

                            detrendReo2 = FilterRecursDetrend(filterCoeffReo, nextReo2, detrendReo2, prevReo2);
                            Data.Reo2ViewArray[MainIndex] = -(int)Math.Round(detrendReo2);

                            prevReo2 = nextReo2;
                            byteNum = 13;
                            break;

                        case 13:// Sphigmo1_0
                            Sphigmo1tmp = (byte)usbport.PortBuf[i];
                            byteNum = 14;
                            break;
                        case 14:// Sphigmo1_1
                            Sphigmo1tmp = Sphigmo1tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 15;
                            break;
                        case 15:// Sphigmo1_2
                            Sphigmo1tmp = Sphigmo1tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Sphigmo1tmp & 0x800000) != 0)
                                Sphigmo1tmp = Sphigmo1tmp - 0x1000000;

                            Data.Sphigmo1Array[MainIndex] = Sphigmo1tmp;

                            nextSphigmo1 = Sphigmo1tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextSphigmo1 = Filter.filter(Filter.coeff14, Data.Sphigmo1Array, MainIndex);
                            }

                            detrendSphigmo1 = FilterRecursDetrend(filterCoeff, nextSphigmo1, detrendSphigmo1, prevSphigmo1);
                            Data.Sphigmo1ViewArray[MainIndex] = (int)Math.Round(detrendSphigmo1);
                            prevSphigmo1 = nextSphigmo1;
                            byteNum = 16;
                            break;

                        case 16:// Sphigmo2_0
                            Sphigmo2tmp = (byte)usbport.PortBuf[i];
                            byteNum = 17;
                            break;
                        case 17:// Sphigmo2_1
                            Sphigmo2tmp = Sphigmo2tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 18;
                            break;
                        case 18:// Sphigmo2_2
                            Sphigmo2tmp = Sphigmo2tmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Sphigmo2tmp & 0x800000) != 0)
                                Sphigmo2tmp = Sphigmo2tmp - 0x1000000;

                            Data.Sphigmo2Array[MainIndex] = Sphigmo2tmp;

                            nextSphigmo2 = Sphigmo2tmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextSphigmo2 = Filter.filter(Filter.coeff14, Data.Sphigmo2Array, MainIndex);
                            }

                            detrendSphigmo2 = FilterRecursDetrend(filterCoeff, nextSphigmo2, detrendSphigmo2, prevSphigmo2);
                            Data.Sphigmo2ViewArray[MainIndex] = (int)Math.Round(detrendSphigmo2);
                            prevSphigmo2 = nextSphigmo2;
                            byteNum = 19;
                            break;

                        case 19: //Apex 0
                            Apextmp = (int)usbport.PortBuf[i];
                            byteNum = 20;
                            break;
                        case 20: //Apex 1
                            Apextmp = Apextmp + 0x100 * (int)(usbport.PortBuf[i]);
                            byteNum = 21;
                            break;
                        case 21: //Apex 2
                            Apextmp = Apextmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Apextmp & 0x800000) != 0)
                                Apextmp = Apextmp - 0x1000000;

                            Data.ApexArray[MainIndex] = Apextmp;

                            nextApex = Apextmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextApex = Filter.filter(Filter.coeff14, Data.ApexArray, MainIndex);
                            }

                            detrendApex = FilterRecursDetrend(filterCoeff, nextApex, detrendApex, prevApex);
                            Data.ApexViewArray[MainIndex] = (int)Math.Round(detrendApex);
                            prevApex = nextApex;

                            byteNum = 22;
                            break;

                        case 22: //Press 1 SYS 
                            PrSYS1 = usbport.PortBuf[i];
                            byteNum = 23;
                            break;
                        case 23: //Press 1 DIA
                            PrDIA1 = usbport.PortBuf[i];
                            byteNum = 24;
                            break;
                        case 24: //Press 2 SYS 
                            PrSYS2 = usbport.PortBuf[i];
                            byteNum = 25;
                            break;
                        case 25: //Press 2 DIA
                            PrDIA2 = usbport.PortBuf[i];
                            byteNum = 26;
                            break;
                        case 26: //Status
                            Status = usbport.PortBuf[i];
                            BP1measStarted = ((Status & bp1progress) != 0);
                            BP2measStarted = ((Status & bp2progress) != 0);

                            if (Pump1Started & (Status & pump1progress) != 0)
                                Pump1RealyStarted = true;
                            if (Pump1RealyStarted)
                            {
//                                detrendSphigmo1 = 0;
                                Data.Sphigmo1ViewArray[MainIndex] = 0;// (int)Math.Round(detrendSphigmo1);
                            }
                            if (Pump1RealyStarted & (Status & pump1progress) == 0)
                            {
                                DelayCounter1++;
                                if (DelayCounter1 > MaxDelayCounter)
                                {
                                    Pump1Started = false;
                                    Pump1RealyStarted = false;
                                    detrendSphigmo1 = 0;
                                    DelayCounter1 = 0;
                                }
                            }

                            if (Pump2Started & (Status & pump2progress) != 0)
                                Pump2RealyStarted = true;
                            if (Pump2RealyStarted)
                            {
                                detrendSphigmo2 = 0;
                                Data.Sphigmo2ViewArray[MainIndex] = (int)Math.Round(detrendSphigmo2);
                            }
                            if (Pump2RealyStarted & (Status & pump2progress) == 0)
                            {
                                DelayCounter2++;
                                if (DelayCounter2 > MaxDelayCounter)
                                {
                                    Pump2Started = false;
                                    Pump2RealyStarted = false;
                                    detrendSphigmo2 = 0;
                                    DelayCounter1 = 0;
                                }
                            }

                            if (Calibrate)
                            {
                                var args = new BPEventArgs();
                                args.status = Status;
                                OnBPcompleted(args);
                            }
                            else
                            if (Status > 0) 
                            {
                                var args = new BPEventArgs();
                                args.status = Status;
                                OnBPcompleted(args);
                            }
                            byteNum = 0;

                            if (RecordStarted)
                                txtFileStream.WriteLine(GetDataString(MainIndex));
                            OnDecomposeLineEvent();
                            LineCounter++;
                            MainIndex++;
                            MainIndex = MainIndex & (DataArrSize - 1);
                            break;
                    }
                }
                usbport.BytesRead = 0;

            return /*usbport.PortBuf.Length -*/ bytes;
        }


        private String GetDataString(uint index)
        {
            //"Dd:mm:yyyy HH:mm:ss:fff"
            return String.Concat(DateTime.Now.ToString(), Convert.ToChar(9),
                                 Data.ECG1Array[index].ToString(), Convert.ToChar(9),
                                 Data.ECG2Array[index].ToString(), Convert.ToChar(9),
                                 Data.Reo1Array[index].ToString(), Convert.ToChar(9),
                                 Data.Reo2Array[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo1Array[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo2Array[index].ToString(), Convert.ToChar(9),
                                 Data.ApexArray[index].ToString());
/*            return String.Concat(DateTime.Now.ToString(), Convert.ToChar(9),
                                 Data.ECG1ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.ECG2ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.Reo1ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.Reo2ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo1ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo2ViewArray[index].ToString(), Convert.ToChar(9),
                                 Data.ApexViewArray[index].ToString(), Convert.ToChar(9));*/
        }
    }
}
