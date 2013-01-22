//To do occupation test cases, main program subs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;


namespace Lab3_CST236
{
    /// <summary>
    /// This is the main driver for the Lab2 program for CST236 OIT Winter 
    /// written by James Brooks
    /// 
    /// This is a test driven development process program.  Please see the word doc spec Lab2_Spec  
    /// 
    /// James Brooks 2/18/12
    /// </summary>

    class Program
    {
        bool Testing = true;
        //bool Testing = false;

        //IQ Range constants

        public const int MinimumIQ = 20, MaximumIQ = 200, ExtremelyLowMin = MinimumIQ, ExtremelyLowMax = 69;

        public const int BorderlineMin = 70, BorderlineMax = 79, LowAverageMin = 80, LowAverageMax = 89;

        public const int AverageMin = 90, AverageMax = 109, HighAverageMin = 110, HighAverageMax = 119;

        public const int SuperiorMin = 120, SuperiorMax = 129, VerySuperiorMin = 130, VerySuperiorMax = MaximumIQ;

        public static readonly string[] IQClassifications = { "Extremley Low", "Borderline", "Low Average", "Average", "High Average", "Superior", "Very Superior" };

        public static readonly string[] IQOccupations = { "Worker Bee", "Used Car Salesman", "Politician,TV Anchor", "Sports Person", "CEO", "Innovator", "Scientist" };

        public const bool debugging = true;

        public const int scoreMinLength = 2, scoreMaxLength = 3;

        

        [STAThread]     //Single thread apartment needed when running OLE OpenFileDialog

        static void Main(string[] args)
        {
            //Program class instantiations

            Menu m1 = Menu.Instance;
            KbIn kb1 = new KbIn();

            //local variables

            string[] setup, digits;
            string prompt, input, Occupation, Classification;
            string keyIn, temp;
            int origWidth, origHeight, score;
            string path = "IQProgramErrorLog.txt";
            bool error, debug;
            string announce = "An IQ score of ";
            string announce2 = " is rated ";
            string announce3 = "A recommended occupation for this level of IQ is ";


            //Initialize local variables

            setup = new string[] { "D1", "D2", "D3" };
            digits = new string[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9", "Backspace", "Enter" };
            prompt = "(Numeric value 20 - 200 required) Enter an IQ score > ";
            input = "";
            error = false;
            debug = debugging;

            //Display menu, process user choices - Main loop

            while (true)
            {
                Console.Clear();
                m1.displayMenu();
                keyIn = kb1.readKey(setup);
                switch (keyIn)
                {
                    case "D1":

                        origWidth  = Console.WindowWidth;
                        origHeight = Console.WindowHeight;
                        error = false;
                        input = "";
 
                        //IntegerDivide(0, 0);
                        Fibonacci(8);

                        if (!error) { GetInput(prompt, ref input, ref error); }

                        Console.SetWindowSize(4, 4);                        //Smallify console during tests

                        //Test GetInput input parameter results

                        if (!(debug && error)) { IsInputNotEmpty(input, ref error); }
                        if (!(debug && error)) { IsInputANumber(input, ref error); }

                        //Input tests passed

                        //Convert input to score

                        score = 0;
                        //if (!error) {GetScore(input, ref score);};

                        //Test GetScore parameter results

                        if (!(debug && error)) { IsScoreANumber(score, ref error); }
                        if (!(debug && error)) { IsScoreGreaterThanMinimumIQ(score, MinimumIQ, ref error); }
                        if (!(debug && error)) { IsScoreLessThanMaximumIQ(score, MaximumIQ, ref error); }

                        //Score tests passed

                        Console.SetWindowSize(origWidth, origHeight);       //Restore console after tests

                        Classification = "";
                        //if (!error) { GetClassifier(score, ref Classification); }

                        //Test GetClassification parameter results

                        if (!(debug && error)) { IsClassificationValid(Classification, ref error); }
                        if (!(debug && error)) { IsClassificationRight(score, Classification, ref error); }

                        Occupation = "";
                        //if (!error) { GetOccupation(score, ref Occupation); }

                        //Test GetOccupation parameter results

                        if (!(debug && error)) { IsOccupationValid(Occupation, ref error); }
                        if (!(debug && error)) { IsOccupationRight(score, Occupation, ref error); }


                        Console.WriteLine();
                        Console.WriteLine();
                        if (!error)
                        {

                            temp = announce + input + announce2 + Classification + ".";

                            Console.WriteLine();
                            Console.WriteLine(temp);
                            Console.WriteLine();
                            temp = announce3 + Occupation + ".";
                            Console.WriteLine(temp);
                            Console.WriteLine();
                            Console.WriteLine();

                            Console.Write("IQ processed.  Press enter to return to the main menu> ");
                        }
                        else
                        {
                            Console.Write("IQ processing failed.  Press enter to return to the main menu> ");
                        }
                        do
                        {
                          temp = kb1.readKey(digits);
                        }
                        while (temp != "Enter");

                        break;

                    case "D2":
                        Console.Clear();
                        Console.WriteLine();
                        if (File.Exists(path))
                        {
                            using (StreamReader sr = File.OpenText(path))
                            {
                                string s = "";
                                while ((s = sr.ReadLine()) != null)
                                {
                                    Console.WriteLine(s);
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.Write("Data displayed. Press enter to continue");
                            do
                            {
                                temp = kb1.readKey(digits);
                            }
                            while (temp != "Enter");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.Write("No data file found. Press enter to continue");
                            do
                            {
                                temp = kb1.readKey(digits);
                            }
                            while (temp != "Enter");
                        }

                        break;

                    case "D3":
                        return;

                    default:
                        break;
                }
            }
        }
        
        // IQ Processing subroutines

        static void GetInput(string prompt, ref string token, ref bool error)
        {

            KbIn kb1 = new KbIn();

            string[] alphas, digits, period, alphas_w_space, digits_0_1, digits_1_2, digits_1_9, digits_0_3, all;

            string blanks = "                 ";
            int orgCursorRow, orgCursorColumn;
            bool test;
            string temp;

            digits = new string[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9", "Backspace", "Enter" };
            alphas = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "Backspace", "Enter" };
            period = new string[] { "OemPeriod", "Backspace", "Enter" };
            alphas_w_space = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "Backspace", "Enter", "Spacebar" };
            digits_0_1 = new string[] { "D0", "D1", "NUMPAD0", "NUMPAD1", "Backspace", "Enter" };
            digits_1_2 = new string[] { "D1", "D2", "NUMPAD1", "NUMPAD2", "Backspace", "Enter" };
            digits_1_9 = new string[] { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9", "Backspace", "Enter" };
            digits_0_3 = new string[] { "D0", "D1", "D2", "D3", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "Backspace", "Enter" };
            alphas_w_space = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "Backspace", "Enter", "Spacebar" };

            digits = new string[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9", "Backspace", "Enter" };
            List<String> allkeys = new List<String>(alphas_w_space.Concat<string>(digits));
            all = allkeys.ToArray();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("IQ Classification program.  James Brooks, CST236, Instructor Bhushan Gupta, OIT");
            Console.WriteLine();
            Console.Write(prompt);
            orgCursorRow = Console.CursorTop;
            orgCursorColumn = Console.CursorLeft;
            test = false;
            int testnum = 0;

            while (test == false)
            {
                temp = kb1.readKey(digits);
                if (temp[0] == 'D') { temp = temp.Substring(1, temp.Length - 1); }

                if (temp == "Backspace")
                {
                    token = kb1.backspacehandler(token, orgCursorColumn, orgCursorRow, blanks);
                }
                else if (temp == "Enter")
                {
                    
                    try
                    {
                        testnum = Convert.ToInt32(token);
                        
                    }
                    catch (FormatException e)
                    {
                        error = true;
                    }

                    if ((testnum >= MinimumIQ) && (testnum <= MaximumIQ))
                    {
                        test = true;
                    }
                }
                else
                {
                    if (token.Length < scoreMaxLength)
                    {
                        Console.SetCursorPosition(orgCursorColumn, orgCursorRow);
                        token = token + temp;
                        Console.Write(token);
                    }
                }
            }
        }

        public void GetScore(string input, ref int score)
        {
            bool testbool = false;

            try
            {
                score = Convert.ToInt32(input);
            }
            
            catch (FormatException e)
            {
                LogTestResult("GetScore program regular function failed <<<");
                Debug.Assert(testbool == true, "GetScore function failed.");
            }

        }

        public void GetClassifier(int score, ref string classification)
        {

            classification = "";
            if ((score >= ExtremelyLowMin) && (score <= ExtremelyLowMax)){classification = IQClassifications[0]; }
            if ((score >= BorderlineMin) && (score <= BorderlineMax)){classification = IQClassifications[1]; }
            if ((score >= LowAverageMin) && (score <= LowAverageMax)){classification = IQClassifications[2]; }
            if ((score >= AverageMin) && (score <= AverageMax)){classification = IQClassifications[3]; }
            if ((score >= HighAverageMin) && (score <= HighAverageMax)){classification = IQClassifications[4]; }
            if ((score >= SuperiorMin) && (score <= SuperiorMax)){classification = IQClassifications[5]; }
            if ((score >= VerySuperiorMin) && (score <= VerySuperiorMax)){classification = IQClassifications[6]; }

        }

        public void GetOccupation(int score, ref string occupation)
        {
            occupation = "";
            if ((score >= ExtremelyLowMin) && (score <= ExtremelyLowMax)) { occupation = IQOccupations[0]; }
            if ((score >= BorderlineMin) && (score <= BorderlineMax)) { occupation = IQOccupations[1]; }
            if ((score >= LowAverageMin) && (score <= LowAverageMax)) { occupation = IQOccupations[2]; }
            if ((score >= AverageMin) && (score <= AverageMax)) { occupation = IQOccupations[3]; }
            if ((score >= HighAverageMin) && (score <= HighAverageMax)) { occupation = IQOccupations[4]; }
            if ((score >= SuperiorMin) && (score <= SuperiorMax)) { occupation = IQOccupations[5]; }
            if ((score >= VerySuperiorMin) && (score <= VerySuperiorMax)) { occupation = IQOccupations[6]; }

        }

        static int IntegerDivide(int dividend, int divisor)
        {
            Debug.Assert(divisor != 0);
            return (dividend / divisor);
        }
        // Test Case Logging Function

        static int Fibonacci(int Factor)
		{
			if (Factor < 2) 
            {
                return (Factor);
            }
			int  x = Fibonacci (-- Factor);
			int  y = Fibonacci (-- Factor);
			
			return (x+y);
		}


        static void LogTestResult(string msg)
        {
            string path = "IQProgramErrorLog.txt";
            bool debug = debugging;
            if (debug)
            {
                StreamWriter sw = File.AppendText(path);

                sw.WriteLine(DateTime.Now.ToString() + " - " + msg);
                sw.Close();
            }
        }

        // Test cases

        static int IsInputNotEmpty(string input, ref bool error)
        {
            if (input != "")
            {
                LogTestResult("IsInputNotEmpty test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsInputNotEmpty test case failed <<<");
                error = true;
                //Debug.Assert(input == "", "IsInputEmpty() found input parameter to be empty.");
                MessageBox.Show("Test case failed. IsInputEmpty() found input parameter to be empty.");
                return -1;
            }
        }

        static int IsInputANumber(string input, ref bool error)
        {
            int test;
            //bool testbool = false;

            try
            {
                test = Convert.ToInt32(input);
                LogTestResult("IsInputANumber test case passed.");
                return 0;
            }
            catch (FormatException e)
            {
                LogTestResult("IsInputANumber test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsInputANumber() found input parameter to be not a number.");
                MessageBox.Show("Test case failed. IsInputEmpty() found input parameter to be not a number.");
                return -1;
            }
        }

        static int IsScoreANumber(int score, ref bool error)
        {

            if(typeof(int) == score.GetType())
            {
                LogTestResult("IsScoreANumber test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsScoreANumber test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreANumber() found score parameter to be not an int type.");
                MessageBox.Show("Test case failed. IsScoreANumber() found score parameter to be not an int type.");
                return -1;
            }
        }

        static int IsScoreGreaterThanMinimumIQ(int score, int MinimumIQ, ref bool error)
        {
            if (score >= MinimumIQ)
            {
                LogTestResult("IsScoreGreaterThanMinimumIQ test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsScoreGreaterThanMinimumIQ test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreGreaterThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsScoreGreaterThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                return -1;
            }
        }

        static int IsScoreLessThanMaximumIQ(int score, int MaximumIQ, ref bool error)
        {
            if (score <= MaximumIQ)
            {
                LogTestResult("IsScoreLessThanMaximumIQ test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsScoreLessThanMaximumIQ test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreLessThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsScoreLessThanMaximumIQ() found score parameter to be greater than the MaximumIQ.");
                return -1;
            }
        }

        static int IsClassificationValid(string Classification, ref bool error) 
        {
            bool found = false;

            foreach (string classif in IQClassifications)
            {
                if (Classification == classif)
                {
                    found = true;
                }
            }
    
            if (found)
            {
                LogTestResult("IsClassificationValid test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsClassificationValid test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreLessThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsClassificationValid() didn't match any classication parameter in the classication list.");
                return -1;
            }
        }

        static int IsClassificationRight(int score, string Classification, ref bool error)
        {
            bool found = false;
            if ((score >= ExtremelyLowMin) && (score <= ExtremelyLowMax) && (Classification == IQClassifications[0])) { found = true; }
            if ((score >= BorderlineMin) && (score <= BorderlineMax) && (Classification == IQClassifications[1])) { found = true; }
            if ((score >= LowAverageMin) && (score <= LowAverageMax) && (Classification == IQClassifications[2])) { found = true; }
            if ((score >= AverageMin) && (score <= AverageMax) && (Classification == IQClassifications[3])) { found = true; }
            if ((score >= HighAverageMin) && (score <= HighAverageMax) && (Classification == IQClassifications[4])) { found = true; }
            if ((score >= SuperiorMin) && (score <= SuperiorMax) && (Classification == IQClassifications[5])) { found = true; }
            if ((score >= VerySuperiorMin) && (score <= VerySuperiorMax) && (Classification == IQClassifications[6])) { found = true; }

            if (found)
            {
                LogTestResult("IsClassificationRight test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsClassificationRight test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreLessThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsClassificationRight() didn't match the classication parameter to the IQ score.");
                return -1;
            }
        }
        static int IsOccupationValid(string Occupation, ref bool error)
        {
            bool found = false;


            foreach (string Occup in IQOccupations)
            {
                if (Occupation == Occup)
                {
                    found = true;
                }
            }

            if (found)
            {
                LogTestResult("IsOccupationValid test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsOccupationValid test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreLessThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsOccupationValid() didn't match any occupation parameter in the occupation list.");
                return -1;
            }
        }

        static int IsOccupationRight(int score, string Occupation, ref bool error)
        {
            bool found = false;
            if ((score >= ExtremelyLowMin) && (score <= ExtremelyLowMax) && (Occupation == IQOccupations[0])) { found = true; }
            if ((score >= BorderlineMin) && (score <= BorderlineMax) && (Occupation == IQOccupations[1])) { found = true; }
            if ((score >= LowAverageMin) && (score <= LowAverageMax) && (Occupation == IQOccupations[2])) { found = true; }
            if ((score >= AverageMin) && (score <= AverageMax) && (Occupation == IQOccupations[3])) { found = true; }
            if ((score >= HighAverageMin) && (score <= HighAverageMax) && (Occupation == IQOccupations[4])) { found = true; }
            if ((score >= SuperiorMin) && (score <= SuperiorMax) && (Occupation == IQOccupations[5])) { found = true; }
            if ((score >= VerySuperiorMin) && (score <= VerySuperiorMax) && (Occupation == IQOccupations[6])) { found = true; }

            if (found)
            {
                LogTestResult("IsOccupationRight test case passed.");
                return 0;
            }
            else
            {
                LogTestResult("IsOccupationRight test case failed <<<");
                error = true;
                //Debug.Assert(testbool == true, "IsScoreLessThanMinimumIQ() found score parameter to be less than the MinimumIQ.");
                MessageBox.Show("Test case failed. IsOccupationRight() didn't match the occupation parameter to the IQ score.");
                return -1;
            }
        }
    }
}
