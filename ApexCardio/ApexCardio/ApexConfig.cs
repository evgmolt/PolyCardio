using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ApexCardio
{
    public class ApexConfig
    {
        public string DataDir { get; set; } 
        public int DataFileNum { get; set; }
        public bool CreateTextFile { get; set; } 
        public string SMTPHost { get; set; } 
        public string Password { get; set; } 
        public string Port { get; set; }   
        public string MailTo { get; set; }   
        public string MailFrom { get; set; } 
        public bool Maximized { get; set; }  
        public int WindowWidth { get; set; } 
        public int WindowHeight { get; set; } 
        public string ArchiverPath { get; set; }  
        public byte PressLevel1 { get; set; }  
        public byte PressLevel2 { get; set; } 
        public int RecordLength { get; set; }
        public bool FilterOn { get; set; }
        public int StartDelay { get; set; }
        public bool[] VisibleGraphs { get; set; }


        public ApexConfig()
        {
            VisibleGraphs = new bool[ApexConstants.NumOfChannels];
            DataDir = Directory.GetCurrentDirectory() + @"\Data\";
            DataFileNum = 0;
            CreateTextFile = true;
            SMTPHost = "smtp.yandex.ru";
            Password = "rfatlhck,c";
            Port = "25";
            MailTo = "lbs.data@yandex.ru";
            MailFrom = "lbs.data@yandex.ru";
            Maximized = true;
            WindowWidth = 800;
            WindowHeight = 500;
            ArchiverPath = Directory.GetCurrentDirectory() + @"\";
            PressLevel1 = 50;
            PressLevel2 = 50;
            RecordLength = 10;
            FilterOn = true;
            StartDelay = 10;
            for (int i = 0; i < ApexConstants.NumOfChannels; i++)
            {
                VisibleGraphs[i] = true;
            }
        }

        public static ApexConfig GetConfig()
        {
            ApexConfig ac;
            JsonSerializer serializer = new JsonSerializer();
                try
                {
                    using (StreamReader sr = new StreamReader(ApexConstants.ConfigFileName))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        ac = (ApexConfig)serializer.Deserialize(reader, typeof(ApexConfig));
                        return ac;
                    }
                }
                catch (Exception)
                {
                    ac = new ApexConfig();
                    SaveConfig(ac);
                    return ac;
                }
        }

        public static void SaveConfig(ApexConfig cfg)
        {
            StreamWriter sw = new StreamWriter(ApexConstants.ConfigFileName);
            JsonWriter writer = new JsonTextWriter(sw);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, cfg);
            writer.Close();
            sw.Close();
        }

    }
}
