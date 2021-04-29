using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PolyCardio
{
    public class RecordInfoData
    {
        public DateTime Date;   //0
        public decimal Length;  //1
        public string Name;     //2
        public string Sex;      //3
        public decimal Age;     //4
        public decimal Height;  //5
        public decimal Weight;  //6
        public int[] ABP;         //8
        public string Note;     //8
        public bool[] DiagnArray; //9

        public string[] ItemNames;


        public RecordInfoData()
        {
            ItemNames = new string[]
                {
                    "Date : ",
                    "Length : ",
                    "Name : ",
                    "Sex : ",
                    "Age : ",
                    "Height : ",
                    "Weight : ",
                    "ABP : ",
                    "Note : ",
                    "CHD",
                    "SA",
                    "PIC",
                    "CHF I class",
                    "CHF II class.",
                    "CHF III class",
                    "CHF IV class",
                    "AH 1 degree",
                    "AH 2 degree",
                    "AH 3 degree",
                    "DM"};
            DiagnArray = new bool[PolyConstants.DiagnCount];
            ABP = new int[2];

        }
    }

    public class RecordInfo
    {
        public int NumOfProperties = PolyConstants.PatientFieldsCount + PolyConstants.DiagnCount;
        private string SignSelected = "V";
        private char ABPseparator = '/';
        public RecordInfoData Data;
        public string FileName;
        private string[] ColumnNames = { "Date/time", "ECG1", "ECG2", "Reo 1", "Reo 2", "Sphigmo 1", "Sphigmo 2", "Poly" };


        public RecordInfo(PolyConfig cfg, string fNameArg, string[] lines)
        {
            FileName = cfg.DataDir + fNameArg;
            Data = new RecordInfoData();
            lines = lines.Select(RemoveItemName).ToArray();
            Data.Date = Convert.ToDateTime(lines[0]);
            Data.Length = Convert.ToInt16(lines[1]);
            Data.Name = lines[2];
            Data.Sex = lines[3];
            Data.Age = Convert.ToInt16(lines[4]);
            Data.Height = Convert.ToInt16(lines[5]);
            Data.Weight = Convert.ToInt16(lines[6]);
            string[] s = lines[7].Split(ABPseparator);
            Data.ABP[0] = Convert.ToInt16(s[0]);
            Data.ABP[1] = Convert.ToInt16(s[1]);
            Data.Note = lines[8];
            for (int i = PolyConstants.PatientFieldsCount; i < PolyConstants.PatientFieldsCount+PolyConstants.DiagnCount; i++)
            {
               Data.DiagnArray[i - PolyConstants.PatientFieldsCount] = lines[i] == SignSelected;
            }
        }


        public RecordInfo(PolyConfig cfg, string fNameArg)
        {
            FileName = cfg.DataDir + fNameArg;
            Data=new RecordInfoData();
            if (File.Exists(FileName))
            try
            {
                var lines = System.IO.File.ReadAllLines(FileName, Encoding.Default);
                lines = lines.Select(RemoveItemName).ToArray();
                Data.Date = Convert.ToDateTime(lines[0]);
                Data.Length = Convert.ToInt16(lines[1]);
                Data.Name = lines[2];
                Data.Sex = lines[3];
                Data.Age = Convert.ToInt16(lines[4]);
                Data.Height = Convert.ToInt16(lines[5]);
                Data.Weight = Convert.ToInt16(lines[6]);
                string[] s = lines[7].Split(ABPseparator);
                Data.ABP[0] = Convert.ToInt16(s[0]);
                Data.ABP[1] = Convert.ToInt16(s[1]);
                Data.Note = lines[8];
                for (int i = PolyConstants.PatientFieldsCount; i < PolyConstants.PatientFieldsCount + PolyConstants.DiagnCount; i++)
                {
                    Data.DiagnArray[i - PolyConstants.PatientFieldsCount] = lines[i] == SignSelected;
                }
            }
            catch (Exception)
            {
            }
        }

        
        public string[] GetRecInfo(bool insertTabs)
        {
            var lines = new string[NumOfProperties];
            lines[0] = AddItemName(insertTabs, 0, Data.Date.ToString());
            lines[1] = AddItemName(insertTabs, 1, Data.Length.ToString());
            lines[2] = AddItemName(insertTabs, 2, Data.Name);
            lines[3] = AddItemName(insertTabs, 3, Data.Sex);
            lines[4] = AddItemName(insertTabs, 4, Data.Age.ToString());
            lines[5] = AddItemName(insertTabs, 5, Data.Height.ToString());
            lines[6] = AddItemName(insertTabs, 6, Data.Weight.ToString());
            lines[7] = AddItemName(insertTabs, 7, Data.ABP[0].ToString() + ABPseparator + Data.ABP[1].ToString());
            lines[8] = AddItemName(insertTabs, 8, Data.Note);
            for (int i = 0; i < PolyConstants.DiagnCount; i++)
            {
                string s;
                s = Data.DiagnArray[i] ? SignSelected : "";
                lines[i + PolyConstants.PatientFieldsCount] = AddItemName(insertTabs, i + PolyConstants.PatientFieldsCount, s); 
            }
            return lines;
        }

        public void Save(PolyConfig cfg)
        {
            string[] lines = GetRecInfo(true);
            System.IO.File.WriteAllLines(FileName, lines, Encoding.Default);
            string[] reserved = new string[PolyConstants.ReservedCount];
            for (int i = 0; i < reserved.Length; i++)
            {
                reserved[i] = PolyStrings.txReserved;
            }
            System.IO.File.AppendAllLines(FileName, reserved, Encoding.Default);
            string[] snames = new string[1];
            for (int i = 0; i < PolyConstants.NumOfChannels + 1; i++)
            {
                snames[0] = snames[0] + ColumnNames[i] + Convert.ToChar(9);
            }
            File.AppendAllLines(FileName, snames, Encoding.Default);
            string[] sEnabled = new string[1];
            sEnabled[0] =  "" + Convert.ToChar(9);
            for (int i = 0; i < PolyConstants.NumOfChannels; i++)
            {
                string s = cfg.VisibleGraphs[i] ? "V" : "X";
                sEnabled[0] = sEnabled[0] + s + Convert.ToChar(9);
            }
            File.AppendAllLines(FileName, sEnabled, Encoding.Default);
        }

        private string AddItemName(bool tabs, int num, string s)
        {
            char c;
            if (tabs) 
            {
                c = Convert.ToChar(9);
            }
            else 
            {
                c = Convert.ToChar(0x20);
            }
            return Data.ItemNames[num]+c+s;
        }

        private string RemoveItemName(string s)
        {
            int pos9 = s.IndexOf(Convert.ToChar(9));
            return s.Substring(pos9+1);
        }
        
    }

}
