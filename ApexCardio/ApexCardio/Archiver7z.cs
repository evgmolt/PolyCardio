using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCardio
{
    class Archiver7z
    {
        private static string Archiver;
        public Archiver7z(string archiver)
        {
            Archiver = archiver;
            if (!File.Exists(archiver))
                throw new Exception("Archiver "+archiver+ " not found");
        }

        public void AddToArchive(string archiveName, string fileNames)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Archiver;
                startInfo.Arguments = " a -mx9 ";
                startInfo.Arguments += archiveName+" ";
                startInfo.Arguments += fileNames;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                int sevenZipExitCode = 0;
                using (Process sevenZip = Process.Start(startInfo))
                {
                    sevenZip.WaitForExit();
                    sevenZipExitCode = sevenZip.ExitCode;
                }
                if (sevenZipExitCode > 1) throw new Exception("Archiver error " + sevenZipExitCode.ToString()); 
            }
            catch (Exception ex)
            {
                throw new Exception("SevenZip.AddToArchive: " + ex.Message);
            }
        }

    }
}
