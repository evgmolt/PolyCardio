using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PolyCardio
{
    class ByteDecomposer
    {
        public DataArrays Data;
        public event EventHandler DecomposeLineEvent;
        public event EventHandler ConnectionBreakdown;
        public const int SamplingFrequency = 125;
        public const int DataArrSize = 0x100000;
        public const int DataArrSizeForView = 4000;
        public const int BytesInBlock = 27;

        public const byte pump1progress = 64;
        public const byte pump2progress = 128;

        public byte Status;

        public uint MainIndex = 0;
        public int LineCounter = 0;
        public int TotalBytes;

        public bool RecordStarted;
        public bool DeviceTurnedOn;
        public bool Pump1Started;
        public bool Pump2Started;

        private const byte _marker1 = 0x19;

        private int _ecg1Tmp;
        private int _ecg2Tmp;
        private int _reoTmp;
        private int _sphigmo1Tmp;
        private int _sphigmo2Tmp;
        private int _apexTmp;

        private int _nextECG1;
        private int _nextECG2;
        private int _nextReo;
        private int _nextSphigmo1;
        private int _nextSphigmo2;
        private int _nextApex;

        private double _detrendECG1 = 0;
        private double _detrendECG2 = 0;
        private double _detrendReo = 0;
        private double _detrendSphigmo1 = 0;
        private double _detrendSphigmo2 = 0;
        private double _detrendApex = 0;

        private int _prevECG1;
        private int _prevECG2;
        private int _prevReo;
        private int _prevSphigmo1;
        private int _prevSphigmo2;
        private int _prevApex;

        private const int _maxNoDataCounter = 10;
        private int _noDataCounter;

        private int _byteNum;
        private bool _pump1RealyStarted;
        private bool _pump2RealyStarted;

        private int _delayCounter1;
        private int _delayCounter2;
        private const int MaxDelayCounter = 300;
        const double filterCoeff = 0.005;
        const double filterCoeffReo = 0.01;

        public ByteDecomposer(DataArrays data)
        {
            Data = data;
            Pump1Started = false;
            Pump2Started = false;
            RecordStarted = false;
            DeviceTurnedOn = true;
            TotalBytes = 0;
            MainIndex = 0;
            _pump1RealyStarted = false;
            _pump2RealyStarted = false;
            _noDataCounter = 0;
            _byteNum = 0;
        }

        protected virtual void OnDecomposeLineEvent()
        {
            DecomposeLineEvent?.Invoke(this, null);
        }

        protected virtual void OnConnectionBreakdown()
        {
            ConnectionBreakdown?.Invoke(this, null);
        }

        public int Decompos(USBserialPort usbport)
        {
            return Decompos(usbport, null, null, null);
        }

        private double FilterRecursDetrend(double filterK, double next, double detrend, double prev)
        {
            return ((1 - filterK) * next - (1 - filterK) * prev + (1 - 2 * filterK) * detrend);
        }

        public void CountViewArrays(int size, bool f)
        {
            DataProcessing.Process(Data.ECG1Array, Data.ECG1ViewArray , size, f, Filter.coeff50);
            DataProcessing.Process(Data.ECG2Array, Data.ECG2ViewArray, size, f, Filter.coeff50);
            DataProcessing.Process(Data.ReoArray, Data.ReoViewArray, size, f, Filter.coeff14);
            DataProcessing.Process(Data.Sphigmo1Array, Data.Sphigmo1ViewArray, size, f, Filter.coeff14);
            DataProcessing.Process(Data.Sphigmo2Array, Data.Sphigmo2ViewArray, size, f, Filter.coeff14);
            DataProcessing.Process(Data.ApexArray, Data.ApexViewArray, size, f, Filter.coeff14);
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
                _noDataCounter++;
                if (_noDataCounter > _maxNoDataCounter)
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
                    TotalBytes += bytes;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            for (int i = 0; i < bytes; i++)
            {
                switch (_byteNum)
                {
                    case 0:// Marker
                        if (usbport.PortBuf[i] == _marker1)
                        {
                            _byteNum = 1;
                        }
                        break;
                    case 1:// ECG1_0
                        _ecg1Tmp = (int)usbport.PortBuf[i];
                        _byteNum = 2;
                        break;
                    case 2:// ECG1_1
                        _ecg1Tmp += 0x100 * (int)usbport.PortBuf[i];
                        _byteNum = 3;
                        break;
                    case 3:// ECG1_2
                        _ecg1Tmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_ecg1Tmp & 0x800000) != 0)
                            _ecg1Tmp -= 0x1000000;

                        Data.ECG1Array[MainIndex] = _ecg1Tmp;

                        _nextECG1 = _ecg1Tmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextECG1 = Filter.FilterForRun(Filter.coeff50, Data.ECG1Array, MainIndex);
                        }
                        _detrendECG1 = FilterRecursDetrend(filterCoeff, _nextECG1, _detrendECG1, _prevECG1);
                        Data.ECG1ViewArray[MainIndex] = (int)Math.Round(_detrendECG1);
                            
                        _prevECG1 = _nextECG1;
                        _byteNum = 4;
                        break;
                    case 4:// ECG2_0
                        _ecg2Tmp = (int)usbport.PortBuf[i];
                        _byteNum = 5;
                        break;
                    case 5:// ECG2_1
                        _ecg2Tmp += 0x100 * (int)usbport.PortBuf[i];
                        _byteNum = 6;
                        break;
                    case 6:// ECG2_2
                        _ecg2Tmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_ecg2Tmp & 0x800000) != 0)
                            _ecg2Tmp -= 0x1000000;

                        Data.ECG2Array[MainIndex] = _ecg2Tmp;

                        _nextECG2 = _ecg2Tmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextECG2 = Filter.FilterForRun(Filter.coeff50, Data.ECG2Array, MainIndex);
                        }
                        _detrendECG2 = FilterRecursDetrend(filterCoeff, _nextECG2, _detrendECG2, _prevECG2);
                        Data.ECG2ViewArray[MainIndex] = (int)Math.Round(_detrendECG2);

                        _prevECG2 = _nextECG2;
                        _byteNum = 7;
                        break;
                    case 7:// Reo_0
                        _reoTmp = (int)usbport.PortBuf[i];
                        _byteNum = 8;
                        break;
                    case 8:// Reo_1
                        _reoTmp += 0x100 * (int)usbport.PortBuf[i];
                        _byteNum = 9;
                        break;
                    case 9:// Reo_2
                        _reoTmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_reoTmp & 0x800000) != 0)
                            _reoTmp -= 0x1000000;
                        if (_reoTmp > PolyConstants.ReoMaxVal) _reoTmp = PolyConstants.ReoMaxVal;
                        Data.ReoArray[MainIndex] = - _reoTmp;

                        _nextReo = _reoTmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextReo = Filter.FilterForRun(Filter.coeff14, Data.ReoArray, MainIndex);
                        }

                        _detrendReo = FilterRecursDetrend(filterCoeffReo, _nextReo, _detrendReo, _prevReo);
                        Data.ReoViewArray[MainIndex] = (int)Math.Round(_detrendReo);

                        _prevReo = _nextReo;
                        _byteNum = 10;
                        break;
                    case 10:// Sphigmo1_0
                        _sphigmo1Tmp = (byte)usbport.PortBuf[i];
                        _byteNum = 11;
                        break;
                    case 11:// Sphigmo1_1
                        _sphigmo1Tmp += 0x100 * (int)usbport.PortBuf[i];
                        _byteNum = 12;
                        break;
                    case 12:// Sphigmo1_2
                        _sphigmo1Tmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_sphigmo1Tmp & 0x800000) != 0)
                            _sphigmo1Tmp = _sphigmo1Tmp - 0x1000000;

                        Data.Sphigmo1Array[MainIndex] = _sphigmo1Tmp;

                        _nextSphigmo1 = _sphigmo1Tmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextSphigmo1 = Filter.FilterForRun(Filter.coeff14, Data.Sphigmo1Array, MainIndex);
                        }

                        _detrendSphigmo1 = FilterRecursDetrend(filterCoeff, _nextSphigmo1, _detrendSphigmo1, _prevSphigmo1);
                        Data.Sphigmo1ViewArray[MainIndex] = (int)Math.Round(_detrendSphigmo1);
                        _prevSphigmo1 = _nextSphigmo1;
                        _byteNum = 13;
                        break;
                    case 13:// Sphigmo2_0
                        _sphigmo2Tmp = (byte)usbport.PortBuf[i];
                        _byteNum = 14;
                        break;
                    case 14:// Sphigmo2_1
                        _sphigmo2Tmp += 0x100 * (int)usbport.PortBuf[i];
                        _byteNum = 15;
                        break;
                    case 15:// Sphigmo2_2
                        _sphigmo2Tmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_sphigmo2Tmp & 0x800000) != 0)
                            _sphigmo2Tmp = _sphigmo2Tmp - 0x1000000;

                        Data.Sphigmo2Array[MainIndex] = _sphigmo2Tmp;

                        _nextSphigmo2 = _sphigmo2Tmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextSphigmo2 = Filter.FilterForRun(Filter.coeff14, Data.Sphigmo2Array, MainIndex);
                        }

                        _detrendSphigmo2 = FilterRecursDetrend(filterCoeff, _nextSphigmo2, _detrendSphigmo2, _prevSphigmo2);
                        Data.Sphigmo2ViewArray[MainIndex] = (int)Math.Round(_detrendSphigmo2);
                        _prevSphigmo2 = _nextSphigmo2;
                        _byteNum = 16;
                        break;
                    case 16: //Apex 0
                        _apexTmp = (int)usbport.PortBuf[i];
                        _byteNum = 17;
                        break;
                    case 17: //Apex 1
                        _apexTmp += 0x100 * (int)(usbport.PortBuf[i]);
                        _byteNum = 18;
                        break;
                    case 18: //Apex 2
                        _apexTmp += 0x10000 * (int)usbport.PortBuf[i];
                        if ((_apexTmp & 0x800000) != 0)
                            _apexTmp = _apexTmp - 0x1000000;

                        Data.ApexArray[MainIndex] = _apexTmp;

                        _nextApex = _apexTmp;
                        if (cfg != null)
                        {
                            if (cfg.FilterOn)
                                _nextApex = Filter.FilterForRun(Filter.coeff14, Data.ApexArray, MainIndex);
                        }

                        _detrendApex = FilterRecursDetrend(filterCoeff, _nextApex, _detrendApex, _prevApex);
                        Data.ApexViewArray[MainIndex] = (int)Math.Round(_detrendApex);
                        _prevApex = _nextApex;

                        _byteNum = 19;
                        break;
                    case 19: //Status
                        Status = usbport.PortBuf[i];

                        if (Pump1Started & (Status & pump1progress) != 0)
                            _pump1RealyStarted = true;
                        if (_pump1RealyStarted)
                        {
//                                detrendSphigmo1 = 0;
                            Data.Sphigmo1ViewArray[MainIndex] = 0;// (int)Math.Round(detrendSphigmo1);
                        }
                        if (_pump1RealyStarted & (Status & pump1progress) == 0)
                        {
                            _delayCounter1++;
                            if (_delayCounter1 > MaxDelayCounter)
                            {
                                Pump1Started = false;
                                _pump1RealyStarted = false;
                                _detrendSphigmo1 = 0;
                                _delayCounter1 = 0;
                            }
                        }

                        if (Pump2Started & (Status & pump2progress) != 0)
                            _pump2RealyStarted = true;
                        if (_pump2RealyStarted)
                        {
                            _detrendSphigmo2 = 0;
                            Data.Sphigmo2ViewArray[MainIndex] = (int)Math.Round(_detrendSphigmo2);
                        }
                        if (_pump2RealyStarted & (Status & pump2progress) == 0)
                        {
                            _delayCounter2++;
                            if (_delayCounter2 > MaxDelayCounter)
                            {
                                Pump2Started = false;
                                _pump2RealyStarted = false;
                                _detrendSphigmo2 = 0;
                                _delayCounter1 = 0;
                            }
                        }

                        _byteNum = 0;

                        if (RecordStarted)
                            txtFileStream.WriteLine(Data.GetDataString(MainIndex));
                        OnDecomposeLineEvent();
                        LineCounter++;
                        MainIndex++;
                        MainIndex &= (DataArrSize - 1);
                        break;
                }
            }
            usbport.BytesRead = 0;
            return bytes;
        }
    }
}
