using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PolyCardio
{
    public interface IMessageHandler
    {
        event Action<Message> WindowsMessage;
    }

    public class USBserialPort
    {
        public SerialPort PortHandle;
        public string[] PortNames;
        public byte[] PortBuf;
        public int CurrentPort;
        public int BytesRead;
        public Boolean ReadEnabled;
        readonly System.Threading.Timer ReadTimer;

        private readonly int _portBufSize = 10000;
        private readonly int _baudRate;

        public event Action<Exception> ConnectionFailure;

        private void ReadPort(object state)
        {
            if (BytesRead > 0) return;
            if (!ReadEnabled) return;
            if (PortHandle == null) return;
            if (PortHandle.IsOpen)
            {
                if (PortHandle.BytesToRead > 0)
                {
                    int bytesToRead = PortHandle.BytesToRead;
                    if (bytesToRead > PortBuf.Length)
                    {
                        bytesToRead = PortBuf.Length;
                    }
                    try
                    {
                        BytesRead = PortHandle.Read(PortBuf, 0, bytesToRead);
                    }
                    catch (Exception)
                    {
                    }
                }
                    
            }
        }
        
        public USBserialPort(IMessageHandler messageHandler, int baudrate)
        {
            _baudRate = baudrate;
            messageHandler.WindowsMessage += OnMessage;
            ReadEnabled = false;
            PortBuf = new byte[_portBufSize];
            ReadTimer = new System.Threading.Timer(ReadPort, null, 0, Timeout.Infinite);
        }

        public bool WriteByte(byte b)
        {
            byte[] buf = new byte[1];
            buf[0] = b;
            try
            {
                if (PortHandle != null)
                {
                    PortHandle.Write(buf, 0, 1);
                }
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
                PortHandle = new SerialPort(PortNames[i], _baudRate)
                {
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One
                };
                ReadEnabled = true;
                try
                {
                    PortHandle.Open();
                }
                catch (Exception e)
                {
                    ConnectionFailure?.Invoke(e);
                }

                ReadTimer.Change(0, PolyConstants.USBTimerInterval);
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

        private void OnMessage(Message m)
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
