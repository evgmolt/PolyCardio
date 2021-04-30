using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolyCardio
{
    static class PolyConstants
    {
        public const int NumOfChannels = 5;
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

        public const string ConfigFileName = "PolyConf.cfg";
        public static string FilesToSendFileName = "fts.ls";

        public const int ReoMaxVal = 7000000;

        public const string txErrorInitPort = "Error initialize virtual serial port";
        public const string txError = "Error";
        public const string txReserved = "RESERVED";
    }
}
