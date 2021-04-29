using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ApexCardio
{

    public interface IMessageHandler
    {
        event Action<Message> WindowsMessage;
    }

    public class USBserialPort
    {
        public string[] PortNames;
        public int CurrentPort;
        public SerialPort PortHandle;
        public byte[] PortBuf;
        public int BytesRead;
        public Boolean ReadEnabled;
        System.Threading.Timer ReadTimer;
        private int BaudRate;

        public event Action<Exception> ConnectionFailure;

        private void ReadPort(object state)
        {
            if (BytesRead > 0) return;
            if (!ReadEnabled) return;
            if (PortHandle == null) return;
            if (PortHandle.IsOpen)
            {
                if (PortHandle.BytesToRead > 0)
                    BytesRead = PortHandle.Read(PortBuf, 0, PortHandle.BytesToRead);
            }
        }
        
        public USBserialPort(IMessageHandler messageHandler, int baudrate)
        {
            BaudRate = baudrate;
            messageHandler.WindowsMessage += onMessage;
            ReadEnabled = false;
            PortBuf = new byte[10000];
            ReadTimer = new System.Threading.Timer(ReadPort, null, 0, Timeout.Infinite);
        }

        public bool WriteByte(byte b)
        {
            byte[] buf = new byte[1];
            buf[0] = b;
            try
            {
                if (PortHandle != null)
                    PortHandle.Write(buf, 0, 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Connect()
        {
            PortNames = GetPortsNames();
            if (PortNames == null) return;
            for (int i = 0; i < PortNames.Count(); i++)
            {
                PortHandle = new SerialPort(PortNames[i], BaudRate);
                PortHandle.DataBits = 8;
                PortHandle.Parity = Parity.None;
                PortHandle.StopBits = StopBits.One;
                ReadEnabled = true;
                try
                {
                    PortHandle.Open();
                }
                catch (Exception e)
                {
                    if (ConnectionFailure != null)
                    {
                        ConnectionFailure(e);
                    }
                }

                ReadTimer.Change(0, ApexConstants.USBTimerInterval);
                CurrentPort = i;
                break;
            }
        }


        //Return array of port names with VCP string;
        private string[] GetPortsNames()
        {
            const string VCP = @"\Device\VCP";

            RegistryKey r_hklm = Registry.LocalMachine;
            RegistryKey r_hard = r_hklm.OpenSubKey("HARDWARE");
            RegistryKey r_device = r_hard.OpenSubKey("DEVICEMAP");
            RegistryKey r_port = r_device.OpenSubKey("SERIALCOMM");
            if (r_port == null) return null;
            string[] portvalues = r_port.GetValueNames();
            int numOfVCP = 0;
            for (int i = 0; i < portvalues.Count(); i++)
            {
                if (portvalues[i].IndexOf(VCP) >= 0) numOfVCP++;
            }
            string[] pn = new string[numOfVCP];
            int Ind = 0;
            for (int i = 0; i < portvalues.Count(); i++)
            {
                if (portvalues[i].IndexOf(VCP) >= 0)
                {
                    pn[Ind] = (string)r_port.GetValue(portvalues[i]);
                    Ind++;
                }
            }
            return pn;
        }

        private void onMessage(Message m)
        {
            if (PortHandle == null)
            {
                BytesRead = 0;
                Connect();
            }
            else
            if (!PortHandle.IsOpen)
            {
                BytesRead = 0;
                Connect();
            }
        }


    }
}
