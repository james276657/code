using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CST415_Lab3_Server
{
    class rcmessage : IComparable<rcmessage>
    {
        public byte[] msg = new byte[146];
        public string msgAscii;
        public int msgLength;
        public string msgType;
        public string msgTimeStamp;
        public string msgRequestID;
        public string msgStudentName;
        public string msgStudentID;
        public string msgServerDelay;
        public string msgClientIPAddress;
        public string msgClientServicePort;
        public string msgClientSocketNumber;
        public string msgHostIPAddress;
        public string msgHostServicePort;
        public string msgResponseID;
        public string msgScenarioNumber;            //To be filled in after message analysis
        public char seperator = '|';
        public string rcvTimeStamp;

        public int CompareTo(rcmessage obj)
        {
            return msgTimeStamp.CompareTo(obj.msgTimeStamp);
        }
    }
}