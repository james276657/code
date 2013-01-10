using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CST415_Lab3_Server
{
    class Program
    {

        static TcpListener tcpListener;
        static Thread listenThread;

        static DateTime StartTime = DateTime.Now;
        const int MessageCount = 100;
        const int rcvMessageCount = 200;
        static int rcvMessageCounter = 0;
        const int LocalDelayRequest_Milliseconds = 50;
        const int RemoteDelayRequest_Milliseconds = 0;
        static List<txmessage> outgoingMessages = new List<txmessage>();
        static List<rcmessage> incomingMessages = new List<rcmessage>();

        static MessageConstructor mc = new MessageConstructor();
        static PopulateClientMessage pc = new PopulateClientMessage();

        static System.Text.Encoding enc = System.Text.Encoding.ASCII;

        static Socket socket;

        static int k;

        static void Main(string[] args)
        {

            //string sHostName = Dns.GetHostName();
            //IPAddress[] ipAddress = Dns.GetHostAddresses(sHostName);
            //for (k = 0; k < ipAddress.Length; k++)
            //{
            //    Console.WriteLine("IP Address {0}: {1} ", k, pc.GetLocalIP());
            //}


            int remoteHostPort = 2605;

            tcpListener = new TcpListener(IPAddress.Any, remoteHostPort);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
            listenThread.Join();
        }


        public static void ListenForClients()
        {
            tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                Console.WriteLine("Client Listener Started");
                TcpClient client = tcpListener.AcceptTcpClient();

                //create a thread to handle communication
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
                clientThread.Join();
            }
        }

        public static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[146];
            int bytesRead;

            Console.WriteLine("Client Handler Started");
            while (true)
            {
                bytesRead = 0;
                int position = 0;
                int totalbytes = 0;

                try
                {
                    totalbytes = 0;
                    position = 0;
                    do 
                    {
                        //blocks until a client sends a message
                        bytesRead = clientStream.Read(message, position, 146 - position);
                        totalbytes = totalbytes + bytesRead;
                        position = position + totalbytes;

                    } while (totalbytes < 146);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                //ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));

                fixup_message(ref message);
                clientStream.Write(message, 0, message.Length);
                //string s = System.Text.Encoding.ASCII.GetString(message, 0, message.Length);
                //Console.WriteLine("Len " + message.Length.ToString() + s);
                clientStream.Flush();
            }

            tcpClient.Close();
        }


        public static void fixup_message(ref byte[] message)
        {
            message[2] = (byte)'R';         
            message[3] = (byte)'S';
            message[4] = (byte)'P';
            pc.Fill_Message_Timestamp(ref message, StartTime);

            message[123] = (byte)'J';       //Student Data
            message[124] = (byte)'B';
            message[125] = (byte)'r';
            message[126] = (byte)'o';
            message[127] = (byte)'o';
            message[128] = (byte)'k';
            message[129] = (byte)'s';
            message[130] = (byte)' ';
            message[131] = (byte)'S';
            message[132] = (byte)'e';
            message[133] = (byte)'r';
            message[134] = (byte)'v';
            message[135] = (byte)'e';
            message[136] = (byte)'r';
            message[137] = (byte)' ';
            message[138] = (byte)'R';
            message[139] = (byte)'e';
            message[140] = (byte)'s';
            message[141] = (byte)'p';
            message[142] = (byte)'.';


        }

    }
}
