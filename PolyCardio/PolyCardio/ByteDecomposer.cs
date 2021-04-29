using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PolyCardio
{
    public class DataArrays
    {
        public int[] ECGArray;
        public int[] ReoArray;
        public int[] Sphigmo1Array;
        public int[] Sphigmo2Array;
        public int[] ApexArray;

        public double[] AverECG;
        public double[] AverReo;
        public double[] AverSphigmo1;
        public double[] AverSphigmo2;
        public double[] AverApex;

        public List<double[]> AverList;

        private int AverArrSize = 256;
        
        public int[] ECGViewArray;
        public int[] ReoViewArray;
        public int[] Sphigmo1ViewArray;
        public int[] Sphigmo2ViewArray;
        public int[] ApexViewArray;

        public List<int[]> ViewList;
        
        private const int RRMaxNum = 1000;
        public int[] RArray;
        public int RInd;

    public DataArrays(int size)
        {
            ECGArray = new int[size];
            ReoArray = new int[size];
            Sphigmo1Array = new int[size];
            Sphigmo2Array = new int[size];
            ApexArray = new int[size];

            AverECG = new double[AverArrSize];
            AverReo = new double[AverArrSize];
            AverSphigmo1 = new double[AverArrSize];
            AverSphigmo2 = new double[AverArrSize];
            AverApex = new double[AverArrSize];

            AverList = new List<double[]>();
            AverList.Add(AverECG);
            AverList.Add(AverReo);
            AverList.Add(AverSphigmo1);
            AverList.Add(AverSphigmo2);
            AverList.Add(AverApex);

            ECGViewArray = new int[size];
            ReoViewArray = new int[size];
            Sphigmo1ViewArray = new int[size];
            Sphigmo2ViewArray = new int[size];
            ApexViewArray = new int[size];

            ViewList = new List<int[]>();
            ViewList.Add(ECGViewArray);
            ViewList.Add(ReoViewArray);
            ViewList.Add(Sphigmo1ViewArray);
            ViewList.Add(Sphigmo2ViewArray);
            ViewList.Add(ApexViewArray);

            RArray = new int[RRMaxNum];
            RInd = 0;
        }
    }

    class ByteDecomposer
    {
        public bool Calibrate;
        public event EventHandler DecomposeLineEvent;
        public event EventHandler ConnectionBreakdown;

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

        private int ECGtmp;
        private int Reotmp;
        private int Sphigmo1tmp;
        private int Sphigmo2tmp;
        private int Apextmp;

        private int nextECG;
        private int nextReo;
        private int nextSphigmo1;
        private int nextSphigmo2;
        private int nextApex;

        private double detrendECG = 0;
        private double detrendReo = 0;
        private double detrendSphigmo1 = 0;
        private double detrendSphigmo2 = 0;
        private double detrendApex = 0;

        private int prevECG;
        private int prevReo;
        private int prevSphigmo1;
        private int prevSphigmo2;
        private int prevApex;

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
            ProcessData(Data.ECGArray, Data.ECGViewArray , size, f, Filter.coeff50);
            ProcessData(Data.ReoArray, Data.ReoViewArray, size, f, Filter.coeff14);
            ProcessData(Data.Sphigmo1Array, Data.Sphigmo1ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.Sphigmo2Array, Data.Sphigmo2ViewArray, size, f, Filter.coeff14);
            ProcessData(Data.ApexArray, Data.ApexViewArray, size, f, Filter.coeff14);
        }

        public int Decompos(USBserialPort usbport, Stream saveFileStream)
        {
            return Decompos(usbport, saveFileStream, null, null);
        }

        public int Decompos(USBserialPort usbport, Stream saveFileStream, StreamWriter txtFileStream, PolyConfig cfg)
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
                            ECGtmp = (int)usbport.PortBuf[i];
                            byteNum = 2;
                            break;
                        case 2:// ECG1_1
                            ECGtmp = ECGtmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 3;
                            break;
                        case 3:// ECG1_2
                            ECGtmp = ECGtmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((ECGtmp & 0x800000) != 0)
                                ECGtmp = ECGtmp - 0x1000000;

                            Data.ECGArray[MainIndex] = ECGtmp;

                            nextECG = ECGtmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextECG = Filter.filter(Filter.coeff50, Data.ECGArray, MainIndex);
                            }
                            detrendECG = FilterRecursDetrend(filterCoeff, nextECG, detrendECG, prevECG);
                            Data.ECGViewArray[MainIndex] = (int)Math.Round(detrendECG);
                            
                            prevECG = nextECG;
                            byteNum = 4;
                            break;

                        case 4:// Reo_0
                            Reotmp = (int)usbport.PortBuf[i];
                            byteNum = 5;
                            break;
                        case 5:// Reo_1
                            Reotmp = Reotmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 6;
                            break;
                        case 6:// Reo_2
                            Reotmp = Reotmp + 0x10000 * (int)usbport.PortBuf[i];
                            if ((Reotmp & 0x800000) != 0)
                                Reotmp = Reotmp - 0x1000000;
                            if (Reotmp > PolyConstants.ReoMaxVal) Reotmp = PolyConstants.ReoMaxVal;
                            Data.ReoArray[MainIndex] = - Reotmp;

                            nextReo = Reotmp;
                            if (cfg != null)
                            {
                                if (cfg.FilterOn)
                                    nextReo = Filter.filter(Filter.coeff14, Data.ReoArray, MainIndex);
                            }

                            detrendReo = FilterRecursDetrend(filterCoeffReo, nextReo, detrendReo, prevReo);
                            Data.ReoViewArray[MainIndex] = (int)Math.Round(detrendReo);

                            prevReo = nextReo;
                            byteNum = 7;
                            break;
                        case 7:// Sphigmo1_0
                            Sphigmo1tmp = (byte)usbport.PortBuf[i];
                            byteNum = 8;
                            break;
                        case 8:// Sphigmo1_1
                            Sphigmo1tmp = Sphigmo1tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 9;
                            break;
                        case 9:// Sphigmo1_2
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
                            byteNum = 10;
                            break;
                        case 10:// Sphigmo2_0
                            Sphigmo2tmp = (byte)usbport.PortBuf[i];
                            byteNum = 11;
                            break;
                        case 11:// Sphigmo2_1
                            Sphigmo2tmp = Sphigmo2tmp + 0x100 * (int)usbport.PortBuf[i];
                            byteNum = 12;
                            break;
                        case 12:// Sphigmo2_2
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
                            byteNum = 13;
                            break;

                        case 13: //Apex 0
                            Apextmp = (int)usbport.PortBuf[i];
                            byteNum = 14;
                            break;
                        case 14: //Apex 1
                            Apextmp = Apextmp + 0x100 * (int)(usbport.PortBuf[i]);
                            byteNum = 15;
                            break;
                        case 15: //Apex 2
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

                            byteNum = 16;
                            break;
                        case 16: //Status
                            Status = usbport.PortBuf[i];

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
                                 Data.ECGArray[index].ToString(), Convert.ToChar(9),
                                 Data.ReoArray[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo1Array[index].ToString(), Convert.ToChar(9),
                                 Data.Sphigmo2Array[index].ToString(), Convert.ToChar(9),
                                 Data.ApexArray[index].ToString());
        }
    }
}
