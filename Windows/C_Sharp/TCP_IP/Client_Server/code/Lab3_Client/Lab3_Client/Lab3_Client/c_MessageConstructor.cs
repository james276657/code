using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CST415_Lab3_Client
{
    class MessageConstructor
    {

        public void nullIcomingMessage(ref byte[] message)
        {
            int i = 0;
            for (i = 0; i < message.Length; i++)
            {
                message[i] = 0;
            }
        }

        public void makeOutgoingMessageTemplate(ref byte[] message)
        {
            message[0] = 0;                 
            message[1] = 144;               //Length of message

            message[2] = (byte)'R';         //MessageType
            message[3] = (byte)'E';
            message[4] = (byte)'Q';

            message[5] = (byte)'|';         //seperator

            message[6] = 0;                 //TimeStamp
            message[7] = 0;
            message[8] = 0;
            message[9] = 0;
            message[10] = 0;
            message[11] = 0;
            message[12] = 0;
            message[13] = 0;
            message[14] = 0;
            message[15] = 0;

            message[16] = (byte)'|';        //seperator

            message[17] = 0;                //Request ID
            message[18] = 0;
            message[19] = 0;
            message[20] = 0;
            message[21] = 0;
            message[22] = 0;
            message[23] = 0;
            message[24] = 0;
            message[25] = 0;
            message[26] = 0;
            message[27] = 0;
            message[28] = 0;
            message[29] = 0;
            message[30] = 0;
            message[31] = 0;
            message[32] = 0;
            message[33] = 0;
            message[34] = 0;
            message[35] = 0;
            message[36] = 0;

            message[37] = (byte)'|';        //seperator

            message[38] = (byte)'B';        //Student Name
            message[39] = (byte)'r';
            message[40] = (byte)'o';
            message[41] = (byte)'o';
            message[42] = (byte)'k';
            message[43] = (byte)'s';
            message[44] = (byte)'J';
            message[45] = (byte)' ';
            message[46] = (byte)' ';
            message[47] = (byte)' ';
            message[48] = (byte)' ';
            message[49] = (byte)' ';
            message[50] = (byte)' ';
            message[51] = (byte)' ';
            message[52] = (byte)' ';
            message[53] = (byte)' ';
            message[54] = (byte)' ';
            message[55] = (byte)' ';
            message[56] = (byte)' ';
            message[57] = (byte)' ';

            message[58] = (byte)'|';        //seperator

            message[59] = (byte)'1';        //Student ID
            message[60] = (byte)'9';
            message[61] = (byte)'-';
            message[62] = (byte)'3';
            message[63] = (byte)'1';
            message[64] = (byte)'6';
            message[65] = (byte)'3';

            message[66] = (byte)'|';        //seperator

            message[67] = 0;                //Server delay in milliseconds
            message[68] = 0;
            message[69] = 0;
            message[70] = 0;
            message[71] = 0;

            message[72] = (byte)'|';        //seperator

            message[73] = 0;               //Client IPV4 Address
            message[74] = 0;
            message[75] = 0;
            message[76] = 0;
            message[77] = 0;
            message[78] = 0;
            message[79] = 0;
            message[80] = 0;
            message[81] = 0;
            message[82] = 0;
            message[83] = 0;
            message[84] = 0;
            message[85] = 0;
            message[86] = 0;
            message[87] = 0;

            message[88] = (byte)'|';        //seperator

            message[89] = 0;                //Client Service Port
            message[90] = 0;
            message[91] = 0;
            message[92] = 0;
            message[93] = 0;

            message[94] = (byte)'|';        //seperator

            message[95] = 0;                //Socket Number
            message[96] = 0;
            message[97] = 0;
            message[98] = 0;
            message[99] = 0;

            message[100] = (byte)'|';       //seperator

            message[101] = 0;               //Foreign Host IPV4 Address
            message[102] = 0;
            message[103] = 0;
            message[104] = 0;
            message[105] = 0;
            message[106] = 0;
            message[107] = 0;
            message[108] = 0;
            message[109] = 0;
            message[110] = 0;
            message[111] = 0;
            message[112] = 0;
            message[113] = 0;
            message[114] = 0;
            message[115] = 0;

            message[116] = (byte)'|';       //seperator

            message[117] = (byte)' ';       //Foreign Host Service Port
            message[118] = (byte)'2';
            message[119] = (byte)'6';
            message[120] = (byte)'0';
            message[121] = (byte)'5';

            message[122] = (byte)'|';       //seperator

            message[123] = (byte)' ';       //Student Data
            message[124] = (byte)' ';
            message[125] = (byte)' ';
            message[126] = (byte)' ';
            message[127] = (byte)' ';
            message[128] = (byte)' ';
            message[129] = (byte)' ';
            message[130] = (byte)' ';
            message[131] = (byte)' ';
            message[132] = (byte)' ';
            message[133] = (byte)' ';
            message[134] = (byte)' ';
            message[135] = (byte)' ';
            message[136] = (byte)' ';
            message[137] = (byte)' ';
            message[138] = (byte)' ';
            message[139] = (byte)' ';
            message[140] = (byte)' ';
            message[141] = (byte)' ';
            message[142] = (byte)' ';

            message[143] = (byte)'|';       //seperator

            message[144] = (byte)'2';       //Test Scenario Number

            message[145] = (byte)'|';       //seperator
        }
    }
}
