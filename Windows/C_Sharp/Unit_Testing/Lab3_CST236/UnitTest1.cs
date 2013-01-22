using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab3_CST236
{
    [TestClass]
    public class UnitTest1
    {
        Class1 class1 = new Class1();
        KbIn keyboard = new KbIn();
        Program program = new Program();

        
        [TestMethod]
        public void Fibonacci_TestMethod()
        {
            int actual, i;
            int[] expected = {0,1,1,2,3,5,8,13,21,33};
            int[] trial = {0,1,2,3,4,5,6,7,8,9};
            for (i = 0; i < 10; i++)
            {
                actual = class1.Fibonacci(trial[i]);
                Assert.AreEqual(actual, expected[i], "Fibonacci");
            }

        }
        [TestMethod]
        public void GetScore_TestMethod()
        {
            string[] test = {"0","1","12","200","55"};
            int[] expected = { 0, 1, 12, 200, 55 };
            int i = 0;
            int actual = 0;

            foreach (string itm in test)
            {

                program.GetScore(itm, ref actual);
                Assert.AreEqual(actual, expected[i], "GetScore");
                i++;
            }

        }
        [TestMethod]
        public void Backspace_TestMethod()
        {
            string test = "Hi There";
            string fill = "   ";
            int i = 2;
            int j = 3;
            string expected = "Hi Ther";
            string actual;

            actual = keyboard.backspacehandler(test, i, j, fill);
            Assert.AreEqual(actual, expected, "Backspace");

        }

        [TestMethod]
        public void GetClassifier_TestMethod()
        {

            string[] IQClassifications = { "Extremley Low", "Borderline", "Low Average", "Average", "High Average", "Superior", "Very Superior" };
            int MinimumIQ = 20, MaximumIQ = 200, ExtremelyLowMin = MinimumIQ, ExtremelyLowMax = 69;
            int BorderlineMin = 70, BorderlineMax = 79, LowAverageMin = 80, LowAverageMax = 89;
            int AverageMin = 90, AverageMax = 109, HighAverageMin = 110, HighAverageMax = 119;
            int SuperiorMin = 120, SuperiorMax = 129, VerySuperiorMin = 130, VerySuperiorMax = MaximumIQ;

            int[] mintestscores = { ExtremelyLowMin, BorderlineMin, LowAverageMin, AverageMin, HighAverageMin, SuperiorMin, VerySuperiorMin };
            int[] maxtestscores = { ExtremelyLowMax, BorderlineMax, LowAverageMax, AverageMax, HighAverageMax, SuperiorMax, VerySuperiorMax };


            int i = 0;
            string actual = "";

            foreach (int itm in mintestscores)
            {

                program.GetClassifier(itm, ref actual);
                Assert.AreEqual(actual, IQClassifications[i], "GetClassifier");
                i++;
            }

            i = 0;

            foreach (int itm in maxtestscores)
            {

                program.GetClassifier(itm, ref actual);
                Assert.AreEqual(actual, IQClassifications[i], "GetClassifier");
                i++;
            }

        }
 
        [TestMethod]
        public void GetOccupation_TestMethod()
        {

            string[] IQOccupations = { "Worker Bee", "Used Car Salesman", "Politician,TV Anchor", "Sports Person", "CEO", "Innovator", "Scientist" };
            int MinimumIQ = 20, MaximumIQ = 200, ExtremelyLowMin = MinimumIQ, ExtremelyLowMax = 69;
            int BorderlineMin = 70, BorderlineMax = 79, LowAverageMin = 80, LowAverageMax = 89;
            int AverageMin = 90, AverageMax = 109, HighAverageMin = 110, HighAverageMax = 119;
            int SuperiorMin = 120, SuperiorMax = 129, VerySuperiorMin = 130, VerySuperiorMax = MaximumIQ;

            int[] mintestscores = { ExtremelyLowMin, BorderlineMin, LowAverageMin, AverageMin, HighAverageMin, SuperiorMin, VerySuperiorMin };
            int[] maxtestscores = { ExtremelyLowMax, BorderlineMax, LowAverageMax, AverageMax, HighAverageMax, SuperiorMax, VerySuperiorMax };


            int i = 0;
            string actual = "";

            foreach (int itm in mintestscores)
            {

                program.GetOccupation(itm, ref actual);
                Assert.AreEqual(actual, IQOccupations[i], "GetOccupation");
                i++;
            }

            i = 0;

            foreach (int itm in maxtestscores)
            {

                program.GetOccupation(itm, ref actual);
                Assert.AreEqual(actual, IQOccupations[i], "GetOccupation");
                i++;
            }

        }
    }
}
