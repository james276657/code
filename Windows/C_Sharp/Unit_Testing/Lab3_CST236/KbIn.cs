using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3_CST236
{
    /// <summary>
    /// KbIn processes keyboard input from the user.  A valid key must be
    /// selected from the passed in setup strings.  Retry will happen
    /// continuously until a valid key is chosen.
    /// 
    /// James Brooks 10/22/11
    /// </summary>

    public class KbIn
    {
        private string st;
        private ConsoleKeyInfo cki;
        private bool goodkey;
        private int i;

        bool Testing = true;
        //bool Testing = false;

        public string readKey(string[] setup)
        {
            do
            {
                cki = Console.ReadKey(true); // get a char
                //Console.WriteLine("");

                st = cki.Key.ToString();
                goodkey = false;
                for (i = 0; i < setup.Length; i++)
                {
                    if (st == setup[i])
                    {
                        goodkey = true;
                        return st;
                    }
                }
                //Console.Write("Invalid key.  Please try again> ");
            }
            while (goodkey == false);

            Console.WriteLine("Your key is: " + st);

            return st;
        }
        public string backspacehandler(string token, int cc, int cr, string fill)
        {

            if (token.Length > 0)
            {
                token = token.Substring(0, token.Length - 1);
                if (!Testing)
                {
                    Console.SetCursorPosition(cc, cr);
                    Console.Write(fill);
                    Console.SetCursorPosition(cc, cr);
                    Console.Write(token);
                }
            }
            return token;
        }

    }

}
