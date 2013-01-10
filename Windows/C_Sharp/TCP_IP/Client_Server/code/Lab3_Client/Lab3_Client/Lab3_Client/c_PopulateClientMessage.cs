using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CST415_Lab3_Client
{
    class PopulateClientMessage
    {
        public const int MESSAGELENGTH_BEGINBYTE = 0;
        public const int MESSAGELENGTH_ENDBYTE = 1;
        
        public const int TIMESTAMP_BEGINBYTE = 6;
        public const int TIMESTAMP_ENDBYTE = 15;

        public const int REQUESTID_BEGINBYTE = 17;
        public const int REQUESTID_ENDBYTE = 36;

        public const int STUDENTNAME_BEGINBYTE = 38;
        public const int STUDENTNAME_ENDBYTE = 57;

        public const int STUDENTID_BEGINBYTE = 59;
        public const int STUDENTID_ENDBYTE = 65;

        public const int SERVERDELAY_BEGINBYTE = 67;
        public const int SERVERDELAY_ENDBYTE = 71;

        public const int CLIENTIPV4_BEGINBYTE = 73;
        public const int CLIENTIPV4_ENDBYTE = 87;

        public const int CLIENTSERVICEPORT_BEGINBYTE = 89;
        public const int CLIENTSERVICEPORT_ENDBYTE = 93;

        public const int CLIENTSOCKETNUMBER_BEGINBYTE = 95;
        public const int CLIENTSOCKETNUMBER_ENDBYTE = 99;

        public const int FOREIGNHOSTIPV4_BEGINBYTE = 101;
        public const int FOREIGNHOSTIPV4_ENDBYTE = 115;

        public const int FOREIGNHOSTSERVICEPORT_BEGINBYTE = 117;
        public const int FOREIGNHOSTSERVICEPORT_ENDBYTE = 121;

        public const int STUDENTDATA_BEGINBYTE = 123;
        public const int STUDENTDATA_ENDBYTE = 142;

        public const int SCENARIO_BEGINBYTE = 144;
        public const int SCENARIO_ENDBYTE = 144;

        public void Fill_Message_Length(ref byte[] bytes, int length)
        {
            byte[] intBytes = BitConverter.GetBytes(length);
            if (BitConverter.IsLittleEndian) { Array.Reverse(intBytes); }

            bytes[MESSAGELENGTH_ENDBYTE] = intBytes[intBytes.Length];
            bytes[MESSAGELENGTH_BEGINBYTE] = intBytes[intBytes.Length - 1];

        }

        public void Fill_Message_Timestamp(ref byte[] bytes, DateTime dt)
        {
            TimeSpan et;
            DateTime now = DateTime.Now;
            int elapsedTime;
            et = now - dt; 
            elapsedTime = (int) et.TotalMilliseconds;
            string asciielapsedTime = elapsedTime.ToString();

            for (int i = TIMESTAMP_ENDBYTE - TIMESTAMP_BEGINBYTE; i >= 0; i--)
            {
                if (asciielapsedTime.Length > i)
                {
                    bytes[TIMESTAMP_ENDBYTE - i] = (byte)asciielapsedTime[asciielapsedTime.Length - 1 - i];
                }
                else
                {
                    bytes[TIMESTAMP_ENDBYTE - i] = (byte)'0';
                }
            }
        }

        public void Fill_Message_RequestID(ref byte[] bytes, ref int RequestID)
        {

            RequestID++;
            string asciiRequestID = RequestID.ToString();

            for (int i = REQUESTID_ENDBYTE - REQUESTID_BEGINBYTE; i >= 0; i--)
            {
                if (asciiRequestID.Length > i)
                {
                    bytes[REQUESTID_ENDBYTE - i] = (byte)asciiRequestID[asciiRequestID.Length - 1 - i];
                }
                else
                {
                    bytes[REQUESTID_ENDBYTE - i] = (byte)'0';
                }
            }
        }
        public void Fill_Message_SeverDelayTime_Milliseconds(ref byte[] bytes, int Delay)
        {

            string asciiDelay = Delay.ToString();

            for (int i = SERVERDELAY_ENDBYTE - SERVERDELAY_BEGINBYTE; i >= 0; i--)
            {
                if (asciiDelay.Length > i)
                {
                    bytes[SERVERDELAY_ENDBYTE - i] = (byte)asciiDelay[asciiDelay.Length - 1 - i];
                }
                else
                {
                    bytes[SERVERDELAY_ENDBYTE - i] = (byte)'0';
                }
            }
        }

        public string GetLocalIP()
        {
            string _IP = null;

            // Resolves a host name or IP address to an IPHostEntry instance.
            // IPHostEntry - Provides a container class for Internet host address information.
            System.Net.IPHostEntry _IPHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            // IPAddress class contains the address of a computer on an IP network.
            foreach (System.Net.IPAddress _IPAddress in _IPHostEntry.AddressList)
            {
                // InterNetwork indicates that an IP version 4 address is expected
                // when a Socket connects to an endpoint
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    _IP = _IPAddress.ToString();
                }
            }
            _IP = "127.0.0.1";                                      //For Testing fix later for live test
            return _IP;
        }



        public void Fill_Message_Client_IP_Address(ref byte[] bytes, string IPAddress)
        {
            while (IPAddress.Length < 15)
            {
                IPAddress += ' ';
            }

            for (int i = CLIENTIPV4_ENDBYTE - CLIENTIPV4_BEGINBYTE; i >= 0; i--)
            {
                if (IPAddress.Length > i)
                {
                    bytes[CLIENTIPV4_ENDBYTE - i] = (byte)IPAddress[IPAddress.Length - 1 - i];
                }
                else
                {
                    bytes[CLIENTIPV4_ENDBYTE - i] = (byte)'0';
                }
            }
        }
        public void Fill_Message_RemoteHost_IP_Address(ref byte[] bytes, string IPAddress)
        {
            while (IPAddress.Length < 15)
            {
                IPAddress += ' ';
            }

            for (int i = FOREIGNHOSTIPV4_ENDBYTE - FOREIGNHOSTIPV4_BEGINBYTE; i >= 0; i--)
            {
                if (IPAddress.Length > i)
                {
                    bytes[FOREIGNHOSTIPV4_ENDBYTE - i] = (byte)IPAddress[IPAddress.Length - 1 - i];
                }
                else
                {
                    bytes[FOREIGNHOSTIPV4_ENDBYTE - i] = (byte)'0';
                }
            }
        }
        public void Fill_Message_ClientServicePort(ref byte[] bytes, string Port)
        {
            while (Port.Length < 5)
            {
                Port += ' ';
            }

            for (int i = CLIENTSERVICEPORT_ENDBYTE - CLIENTSERVICEPORT_BEGINBYTE; i >= 0; i--)
            {
                if (Port.Length > i)
                {
                    bytes[CLIENTSERVICEPORT_ENDBYTE - i] = (byte)Port[Port.Length - 1 - i];
                }
                else
                {
                    bytes[CLIENTSERVICEPORT_ENDBYTE - i] = (byte)'0';
                }
            }
        }
        public void Fill_Message_ClientSocket(ref byte[] bytes, string Socket)
        {
            while (Socket.Length < 5)
            {
                Socket += ' ';
            }

            for (int i = CLIENTSOCKETNUMBER_ENDBYTE - CLIENTSOCKETNUMBER_BEGINBYTE; i >= 0; i--)
            {
                if (Socket.Length > i)
                {
                    bytes[CLIENTSOCKETNUMBER_ENDBYTE - i] = (byte)Socket[Socket.Length - 1 - i];
                }
                else
                {
                    bytes[CLIENTSOCKETNUMBER_ENDBYTE - i] = (byte)'0';
                }
            }
        }
    }
}
