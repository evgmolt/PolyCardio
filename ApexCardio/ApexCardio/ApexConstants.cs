using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexCardio
{
    static class ApexConstants
    {
        public const int NumOfChannels = 7;
        public const byte M1 = 0xE6;
        public const byte ID = 0x61;
        public const byte cmGetID = 0x5A;
        public const byte cmSetPressLevel1 = 0x80;
        public const byte cmSetPressLevel2 = 0x81;
        public const byte cmStartPump1 = 0x02;
        public const byte cmStartPump2 = 0x03;
        public const byte cmStopPump1 = 0x04;
        public const byte cmStopPump2 = 0x05;
        public const byte cmStartBP1 = 0x53;
        public const byte cmStartBP2 = 0x54;
        public const byte cmCancelBP1 = 0x55;
        public const byte cmCancelBP2 = 0x56;
        public const byte cmCalibrate = 0x59;

        public const int USBTimerInterval = 25;
//        public static string DataFileExtension = ".bin";
        public static string ArchiverFileExtension = ".7z";
        public static string TextFileExtension = ".txt";
        public static string RecInfoExtension = ".txt";
        public static string TmpTextFile = "tmp.t";

        public const int DiagnCount = 11;
        public const int ReservedCount = 8;
        public static int PatientFieldsCount = 9;
        public static int HeaderAndVisibleCount = 2;
        public static int HeaderSize = PatientFieldsCount + ReservedCount + DiagnCount + HeaderAndVisibleCount;
        //        public static double ScaleECG_Y = 1;
//        public static double ScalePPG_Y = 1;

        public const string ConfigFileName = "ApexConf.cfg";
        public static string FilesToSendFileName = "fts.ls";

        public const int ReoMaxVal = 7000000;

        public const string ECG1name = "ECG 1";
        public const string ECG2name = "ECG 2";
        public const string Reo1name = "Reo 1";
        public const string Reo2name = "Reo 2";
        public const string Sphigmo1name = "Sphigmo 1";
        public const string Sphigmo2name = "Sphigmo 2";
        public const string Apexname = "Apex";
    }
}
