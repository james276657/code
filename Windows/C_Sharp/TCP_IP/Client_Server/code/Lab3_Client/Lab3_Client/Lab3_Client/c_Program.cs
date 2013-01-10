using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CST415_Lab3_Client

{
    class Program
    {
        static int RequestID = 19;                         //Transmit Message number (Request ID)
        static DateTime SendStartTime, SendStopTime, ReceiveStartTime, ReceiveStopTime;
        static int MessageCount = 5000;
        static int LocalDelayRequest_Milliseconds = 10;
        const int RemoteDelayRequest_Milliseconds = 0;
        static List<txmessage> outgoingMessages = new List<txmessage>();
        static List<rcmessage> incomingMessages = new List<rcmessage>();

        static MessageConstructor mc = new MessageConstructor();
        static PopulateClientMessage pc = new PopulateClientMessage();

        static System.Text.Encoding enc = System.Text.Encoding.ASCII;

        static Socket socket;

        static void Main(string[] args)
        {

            //string remoteHostIP = "192.168.101.210";
            //string remoteHostIP = "127.0.0.1";                                  //For testing change later for testing with other servers
            MessageCount = 100;
            LocalDelayRequest_Milliseconds = 10;
            string remoteHostIP = "10.170.4.84";                                  //For testing change later for testing with other servers
            string servername = "James Brooks";
            int remoteHostPort = 2605;
            //string file = "scenario_1_output_log_james_brooks_";
            string file = "Lab3.ScenarioC.BrooksJamesE_";

            string localIP = "127.0.0.1";
            string localPort = "";

            int i = 0;
            int error = 0;

            //Initialize outgoing messages to template values and add in known info
            for (i = 0; i < MessageCount; i++)
            {
                txmessage txmsg = new txmessage();

                //Initialize with predefined template values
                mc.makeOutgoingMessageTemplate(ref txmsg.msg);

                //Unique request id for each message
                //Request ID incremented in Fill_Message_RequestID
                pc.Fill_Message_RequestID(ref txmsg.msg, ref RequestID);

                pc.Fill_Message_SeverDelayTime_Milliseconds(ref txmsg.msg, RemoteDelayRequest_Milliseconds);
                pc.Fill_Message_Client_IP_Address(ref txmsg.msg, pc.GetLocalIP());
                pc.Fill_Message_RemoteHost_IP_Address(ref txmsg.msg, remoteHostIP);
                outgoingMessages.Add(txmsg);
            }

            //Null incoming message array bytes
            for (i = 0; i < MessageCount; i++)
            {
                rcmessage rmsg = new rcmessage();
                mc.nullIcomingMessage(ref rmsg.msg);
                incomingMessages.Add(rmsg);
            }

            //Open Socket Session

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(remoteHostIP), remoteHostPort);
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);

            if (socket != null)
            {
                Console.WriteLine("Connected to " + remoteHostIP + " at " + remoteHostPort.ToString());
            }
            else
            {
                Console.WriteLine("Connection failed to " + remoteHostIP + " at " + remoteHostPort.ToString());
            }

            //Get socket statistics

            localIP = IPAddress.Parse(((IPEndPoint)socket.LocalEndPoint).Address.ToString()).ToString();
            localPort = (((IPEndPoint)socket.LocalEndPoint).Port.ToString()).ToString();

            //Fill in Session info to outgoing messages

            for (i = 0; i < MessageCount; i++)
            {
                pc.Fill_Message_Client_IP_Address(ref outgoingMessages[i].msg, localIP);
                pc.Fill_Message_ClientServicePort(ref outgoingMessages[i].msg, localPort);
                pc.Fill_Message_ClientSocket(ref outgoingMessages[i].msg, localPort);
            }


            //Transmit/Receive Threads

            SendMessages oSendMessages = new SendMessages();
            ReceiveMessages oReceiveMessages = new ReceiveMessages();

            SendStartTime = DateTime.Now;
            Thread oThread1 = new Thread(new ThreadStart(oSendMessages.sndmessages));
            ReceiveStartTime = DateTime.Now;
            Thread oThread2 = new Thread(new ThreadStart(oReceiveMessages.RcvMessages));

            // Start the threads
            oThread1.Start();
            while (!oThread1.IsAlive) ;
            oThread2.Start();
            while (!oThread2.IsAlive) ;

            oThread1.Join();
            oThread2.Join();
      
            // Do half session shutdowns

            // Close the transmitter
            socket.Shutdown(SocketShutdown.Send);

            // Close the listener
            socket.Shutdown(SocketShutdown.Receive);

            // Close the session

            socket.Close();


            // Put messages into ascii strings
            for (i = 0; i < incomingMessages.Count; i++)
            {
                //Put entire message to ascii
                incomingMessages[i].msgAscii = enc.GetString(incomingMessages[i].msg);

                //Insert appropriate response type
                if (incomingMessages[i].msgAscii.Contains("Delayed"))
                {
                    StringBuilder s = new StringBuilder(incomingMessages[i].msgAscii);
                    s[144] = '3';
                    incomingMessages[i].msgAscii = s.ToString();
                    incomingMessages[i].msg[144] = (byte) '3';
                }
                else 
                {
                    StringBuilder s = new StringBuilder(incomingMessages[i].msgAscii);
                    s[144] = '1';
                    incomingMessages[i].msgAscii = s.ToString();
                    incomingMessages[i].msg[144] = (byte)'1';
                }
                string tempstr1 = incomingMessages[i].msgAscii.Substring(2, incomingMessages[i].msgAscii.Length - 2);

                //Make length into int
                byte[] tempbytes = new byte[4];
                tempbytes[2] = 0;
                tempbytes[3] = 0;
                tempbytes[1] = incomingMessages[i].msg[0];
                tempbytes[0] = incomingMessages[i].msg[1];
                int k = BitConverter.ToInt32(tempbytes, 0);

                //Put the components in seperated fields
                string[] messageParts = tempstr1.Split(incomingMessages[i].seperator);
                if (messageParts.Count() == 14)
                {
                    incomingMessages[i].msgLength = k;
                    incomingMessages[i].msgType = messageParts[0];
                    incomingMessages[i].msgTimeStamp = messageParts[1];
                    incomingMessages[i].msgRequestID = messageParts[2];
                    incomingMessages[i].msgStudentName = messageParts[3];
                    incomingMessages[i].msgStudentID = messageParts[4];
                    incomingMessages[i].msgServerDelay = messageParts[5];
                    incomingMessages[i].msgClientIPAddress = messageParts[6];
                    incomingMessages[i].msgClientServicePort = messageParts[7];
                    incomingMessages[i].msgClientSocketNumber = messageParts[8];
                    incomingMessages[i].msgHostIPAddress = messageParts[9];
                    incomingMessages[i].msgHostServicePort = messageParts[10];
                    incomingMessages[i].msgResponseID = messageParts[11];
                    incomingMessages[i].msgScenarioNumber = messageParts[12];
                }
                else
                {
                    error = 1;
                    incomingMessages[i].msgScenarioNumber = "4";
                    incomingMessages[i].msgTimeStamp = "9999999999";
                }
            }

            //Sort incoming messages by time stamp

            incomingMessages = incomingMessages.OrderBy(x => x.msgTimeStamp).ToList();

            //Create the last line of the log

            string LastLine = "Requests transmitted = \t" + MessageCount.ToString().PadLeft(5, '0') + "\r\n";
            LastLine = LastLine + "Responses received = \t" + MessageCount.ToString().PadLeft(5, '0') + "\r\n"; 
            
            TimeSpan et = SendStopTime - SendStartTime;
            LastLine = LastLine + "Req. run duration = \t" + et.TotalMilliseconds.ToString().PadLeft(9, '0') + "\r\n";

            et = ReceiveStopTime - ReceiveStartTime;
            LastLine = LastLine + "Rsp. Run duration = \t" + et.TotalMilliseconds.ToString().PadLeft(9, '0') + "\r\n";

            et = ReceiveStopTime - SendStartTime;
            LastLine = LastLine + "Trans. Duration = \t" + et.TotalMilliseconds.ToString().PadLeft(9, '0') + "\r\n";

            et = SendStopTime - SendStartTime;
            int durtime = (int)et.TotalMilliseconds / MessageCount;
            LastLine = LastLine + "Actual req. pace = \t" + durtime.ToString().PadLeft(5, '0') + "\r\n";

            et = ReceiveStopTime - ReceiveStartTime;
            durtime = (int)et.TotalMilliseconds / MessageCount;
            LastLine = LastLine + "Actual rsp. Pace = \t" + durtime.ToString().PadLeft(5, '0') + "\r\n";

            LastLine = LastLine + "Configured pace = \t" + LocalDelayRequest_Milliseconds.ToString().PadLeft(5, '0') + "\r\n";

            et = ReceiveStopTime - SendStartTime;
            durtime = (int)et.TotalMilliseconds / MessageCount;
            LastLine = LastLine + "Transaction avg. = \t" + durtime.ToString().PadLeft(5, '0') + "\r\n";

            LastLine = LastLine + "Your name: \t" + servername + "\r\n";
            LastLine = LastLine + "Name of student whose client was used: \t" + "James Brooks\r\n";


            //Create a log file

            file = file + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") + ".txt";
            StreamWriter writer = new StreamWriter(file);

            for (i = 0; i < incomingMessages.Count; i++)
            {
                writer.WriteLine(outgoingMessages[i].msgAscii);
                writer.WriteLine(incomingMessages[i].msgAscii);
            }
            writer.WriteLine(LastLine);
            writer.Close();
        }

        public class SendMessages
        {
            public void sndmessages()
            {
                int i;

                for (i = 0; i < MessageCount; i++)
                {

                    DateTime dt1 = DateTime.Now;
                    int diff = 0;

                    //Wait delay time before transmission
                    while (diff < LocalDelayRequest_Milliseconds)
                    {
                        DateTime dt2 = DateTime.Now;
                        TimeSpan ts = dt2.Subtract(dt1);
                        diff = (int)ts.TotalMilliseconds;
                    }
                    if (i == 30){pc.Fill_Message_SeverDelayTime_Milliseconds(ref outgoingMessages[i].msg, 0);}
                    if (i == 60) { pc.Fill_Message_SeverDelayTime_Milliseconds(ref outgoingMessages[i].msg, 0); }
                    pc.Fill_Message_Timestamp(ref outgoingMessages[i].msg, SendStartTime);

                    //make a string for the outgoing message;
                    outgoingMessages[i].msgAscii = enc.GetString(outgoingMessages[i].msg);

                    //Send a message 
                    socket.Send(outgoingMessages[i].msg, outgoingMessages[i].msg.Length, 0);
                    SendStopTime = DateTime.Now;
                    Console.WriteLine("Sent message " + outgoingMessages[i].msgAscii);
                }
            }
        }

        public class ReceiveMessages
        {
            public void RcvMessages()
            {
                int messageSize = 0;
                int i;
                byte[] tempMessage = new byte[146];
                int bytes = 0;

                for (i = 0; i < MessageCount; i++)
                {
                    messageSize = 0;
                    // The following will block until the message is transmitted.
                    do
                    {
                        Console.WriteLine("Listening on IP " + IPAddress.Parse(((IPEndPoint)socket.LocalEndPoint).Address.ToString()).ToString() + "and port " + (((IPEndPoint)socket.LocalEndPoint).Port.ToString()).ToString());
                        bytes = socket.Receive(tempMessage, tempMessage.Length, 0);
                        messageSize = messageSize + bytes;
                    }
                    while (messageSize < 146);

                    ReceiveStopTime = DateTime.Now;

                    //Save the message

                    Array.Copy(tempMessage, 0, incomingMessages[i].msg, 0, incomingMessages[i].msg.Length);

                    Console.WriteLine("Received message " + enc.GetString(incomingMessages[i].msg));

                    if (messageSize != incomingMessages[i].msg.Length)
                    {
                        incomingMessages[i].msgScenarioNumber = "4";
                        Console.WriteLine("Error in incoming message size" + messageSize.ToString());
                    }

                }
            }
        }
    }
}
